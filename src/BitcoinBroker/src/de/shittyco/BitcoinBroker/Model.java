/**
 * @author Jon Rowlett
 */
package de.shittyco.BitcoinBroker;

import java.net.*;
import java.util.Date;
import java.util.List;

import net.milkbowl.vault.economy.Economy;
import net.milkbowl.vault.economy.EconomyResponse;
import de.shittyco.Bitcoin.*;

import org.bukkit.OfflinePlayer;
import org.bukkit.entity.*;
import org.bukkit.plugin.Plugin;

/**
 * @author Jon Rowlett
 * 
 */
public class Model {
	
	private BrokerageInfo brokerageInfo = new BrokerageInfo();
	private URL bitcoinUrl;
	private String account;
	private String commissionAccount;
	private BitcoinClient client;
	private Economy econ;
	private Plugin plugin;
	private PlayerDataStore playerDataStore;
	
	public Model(
		Plugin plugin,
		Economy econ) {
		this.plugin = plugin;
		this.econ = econ;
		this.playerDataStore = new PlayerDataStore(this.plugin);
	}
	
	public BrokerageInfo getBrokerageInfo() {
		return this.brokerageInfo;
	}
	
	public String getBitcoinUrl() {
		return this.bitcoinUrl.toString();
	}
	
	public void setBitcoinUrl(String value) throws MalformedURLException {
		this.bitcoinUrl = new URL(value);
	}
	
	public void setAccount(String value) {
		this.account = value;
	}
	
	public String getAccount() {
		return this.account;
	}
	
	public void setCommissionAccount(String value) {
		this.commissionAccount = value;
	}
	
	public String getCommissionAccount() {
		return this.commissionAccount;
	}
	
	public String init(String user, String password) {
		this.client = new BitcoinClient(
			this.bitcoinUrl,
			user,
			password);
		try {
			BitcoinInfo info = this.client.getInfo();
			return String.format(
				"Connections=%d, Total Server Balance=%s", 
				info.getConnections(),
				info.getBalance().toString());
		} catch (ServerErrorException e) {
			return e.toString();
		}
	}
	
	public String getBrokerageBalance() {
		try {
			return this.client.getBalance(this.account).toString();
		} catch (ServerErrorException e) {
			return "???";
		}
	}
	
	public String getCommissionsBalance() {
		try {
			return this.client.getBalance(this.commissionAccount).toString();
		} catch (ServerErrorException e) {
			return "???";
		}
	}
	
	public PlayerAccountInfo getAccountInfo(OfflinePlayer player) throws Exception {
		String linkedAddress = this.playerDataStore.getPlayerLinkedAddress(player);
		List<TransactionLogEntry> latestTransactions = this.playerDataStore.getPlayerLatestTransactions(player);
		PlayerAccountInfo result = new PlayerAccountInfo(
			this.client.getAccountAddress(player.getName()),
			this.client.getBalance(player.getName()),
			linkedAddress,
			latestTransactions);
		return result;
	}
	
	public void setLinkedAddress(OfflinePlayer player, String linkedAddress) throws Exception {
		this.playerDataStore.setLinkedAddress(player, linkedAddress);
	}
	
	public TransactionLogEntry sell(Player player, BTC value) throws Exception {
		BTC commission = BTC.mul(value, (float) this.brokerageInfo.getBtcToCoinsCommission());
		if (commission.equals(new BTC(0)) && this.brokerageInfo.getBtcToCoinsCommission() > 0) {
			commission = new BTC(0.00000001);
		}
		
		BTC tradeValue = BTC.sub(value, commission);
		double coins = tradeValue.doubleValue() / this.brokerageInfo.getBtcToCoinsRate();
		this.client.move(player.getName(), this.account, value);
		EconomyResponse response = this.econ.depositPlayer(player.getName(), coins);
		if (!response.transactionSuccess()) {
			this.client.move(this.account, player.getName(), value);
			throw new Exception("Transaction failed.");
		}
		
		if (!commission.equals(new BTC(0))) {
			this.client.move(this.account, this.commissionAccount, commission);
		}
		
		String description = String.format("Sell %s BTC, Fee=%s BTC", value, commission);
		Date time = new Date();
		TransactionLogEntry entry = new TransactionLogEntry(
			time,
			description,
			BTC.sub(new BTC(0), value),
			coins,
			this.client.getBalance(player.getName()),
			this.econ.getBalance(player.getName()));
		this.playerDataStore.logTransactionEntry(player, entry);
		return entry;
	}
	
	public TransactionLogEntry buy(Player player, BTC value) throws Exception {
		BTC commission = BTC.mul(value, (float) this.getBrokerageInfo().getCoinsToBtcCommission());
		if (commission.equals(new BTC(0)) && this.brokerageInfo.getCoinsToBtcCommission() > 0) {
			commission = new BTC(0.00000001);
		}
		
		BTC tradeValue = BTC.add(value, commission);
		if (tradeValue.compareTo(this.client.getBalance(this.account)) > 0) {
			throw new Exception("Insufficient brokerage funds. Contact the server administrator.");
		}
		
		double coins = tradeValue.doubleValue() * this.brokerageInfo.getCoinsToBtcRate();
		EconomyResponse response = this.econ.withdrawPlayer(player.getName(), coins);
		if(!response.transactionSuccess()) {
			throw new Exception("Transaction failed.");
		}
		
		this.client.move(this.account, player.getName(), value);
		if(!commission.equals(new BTC(0))) {
			this.client.move(this.account, this.commissionAccount, commission);
		}
		
		double commissionCoins = commission.doubleValue() * this.brokerageInfo.getCoinsToBtcRate();
		
		TransactionLogEntry entry = new TransactionLogEntry(
			new Date(),
			String.format("Buy %s BTC, Fee=%s coins", value, ((Double)commissionCoins).toString()),
			value,
			-coins,
			this.client.getBalance(player.getName()),
			this.econ.getBalance(player.getName()));
		this.playerDataStore.logTransactionEntry(player, entry);
		return entry;
	}
	
	public TransactionLogEntry transfer(Player player, BTC value) throws Exception {
		String linkedAddress = this.playerDataStore.getPlayerLinkedAddress(player);
		if(linkedAddress.length() == 0) {
			throw new Exception("There is no linked address on file. Set one with the /btc account link command.");
		}
		
		String txid = this.client.sendFrom(player.getName(), linkedAddress, value);
		String description = String.format("Sent %s BTC to [%s]. Transaction ID=%s", value, linkedAddress, txid);
		TransactionLogEntry entry = new TransactionLogEntry(
			new Date(),
			description,
			BTC.sub(new BTC(0), value),
			0,
			this.client.getBalance(player.getName()),
			this.econ.getBalance(player.getName()));
		this.playerDataStore.logTransactionEntry(player, entry);
		return entry;
	}
	
	public String cashOut(BTC value) throws Exception {
		String profitAddress = this.brokerageInfo.getProfitAddress();
		if(profitAddress == null || profitAddress.length() == 0) {
			throw new Exception("There is no profit address set in config. Update BitcoinBroker/config.yml with a valid brokerage.profitAddress field.");
		}
		
		return this.client.sendFrom(this.commissionAccount, profitAddress, value);
	}
}
