/**
 * @author Jon Rowlett
 */
package de.shittyco.BitcoinBroker;

import java.net.MalformedURLException;
import java.util.logging.Level;

import org.bukkit.configuration.file.*;
import org.bukkit.plugin.RegisteredServiceProvider;
import org.bukkit.plugin.java.*;
import net.milkbowl.vault.Vault;
import net.milkbowl.vault.chat.Chat;
import net.milkbowl.vault.economy.Economy;
import net.milkbowl.vault.economy.EconomyResponse;
import net.milkbowl.vault.permission.Permission;

/**
 * @author Jon Rowlett
 *
 */
public class BitcoinBroker extends JavaPlugin {
	private Model model = null;
	
	public void onEnable() {
		Economy econ = this.initEconomy();
		if (econ == null) {
			getLogger().log(Level.SEVERE,  "Vault Plugin is not loaded. Bitcoin Broker requires it. Make sure it is installed.");
			return;
		}
		
		this.model = new Model(this, econ);
		this.initModel();
		getCommand("btc").setExecutor(new RootCommandExecutor(this.model));
		getLogger().info(this.model.getBrokerageInfo().toString());
	}
 
	public void onDisable() {
		getLogger().info("Brokerage is now disabled.");
	}
	
    private Economy initEconomy() {
        if (this.getServer().getPluginManager().getPlugin("Vault") == null) {
            return null;
        }
        
        RegisteredServiceProvider<Economy> rsp = getServer().getServicesManager().getRegistration(Economy.class);
        if (rsp == null) {
            return null;
        }
        
        Economy econ = rsp.getProvider();
        return econ;
    }	
	
	private void initModel() {
		FileConfiguration config = this.getConfig();
		BrokerageInfo brokerageInfo = this.model.getBrokerageInfo();
		brokerageInfo.setBtcToCoinsRate((float) config.getDouble("brokerage.btcToCoinsRate"));
		brokerageInfo.setCoinsToBtcRate((float) config.getDouble("brokerage.coinsToBtcRate"));
		brokerageInfo.setBtcToCoinsCommission((float) config.getDouble("brokerage.btcToCoinsCommission"));
		brokerageInfo.setCoinsToBtcCommission((float) config.getDouble("brokerage.coinsToBtcCommission"));
		if(!config.contains("brokerage.profitAddress")) {
			if(brokerageInfo.getCoinsToBtcCommission() > 0 || brokerageInfo.getBtcToCoinsCommission() > 0) {
				getLogger().log(Level.WARNING, "brokerage.profitAddress is missing. No commissions will be collected.");
				brokerageInfo.setBtcToCoinsCommission(0);
				brokerageInfo.setCoinsToBtcCommission(0);
			}
		} else {
			brokerageInfo.setProfitAddress(config.getString("brokerage.profitAddress"));
		}
		
		try {
			this.model.setBitcoinUrl(config.getString("bitcoinUrl"));
		} catch (MalformedURLException e) {
			getLogger().log(Level.SEVERE, e.getLocalizedMessage());
			return;
		}
		
		String account = config.getString("account");
		String rpcUser = config.getString("rpcuser");
		if (rpcUser == null || rpcUser.isEmpty()) {
			getLogger().log(Level.SEVERE, "Empty user name. Set rpcuser in BitcoinBroker/config.yml");
			return;
		}
		
		String rpcPassword = config.getString("rpcpassword");
		if (rpcPassword == null || rpcPassword.isEmpty()) {
			getLogger().log(Level.SEVERE, "Empty password. Set rpcpassword in BitcoinBroker/config.yml");
			return;
		}

		this.model.setAccount(account);
		getLogger().info(
			this.model.init(rpcUser, rpcPassword));
		getLogger().info(String.format("URL: %s, Account: %s", this.model.getBitcoinUrl(), this.model.getAccount()));
	}
}
