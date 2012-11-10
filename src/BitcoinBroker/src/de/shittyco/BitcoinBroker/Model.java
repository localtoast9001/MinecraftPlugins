/**
 * @author Jon Rowlett
 */
package de.shittyco.BitcoinBroker;

import java.io.IOException;
import java.net.*;

import net.milkbowl.vault.economy.Economy;
import de.shittyco.Bitcoin.*;
import org.bukkit.entity.*;
import org.bukkit.metadata.FixedMetadataValue;
import org.bukkit.metadata.MetadataValue;
import org.bukkit.plugin.Plugin;

/**
 * @author Jon Rowlett
 * 
 */
public class Model {
	private static String metadataKey = "BTCLINKEDADDRESS";
	
	private BrokerageInfo brokerageInfo = new BrokerageInfo();
	private URL bitcoinUrl;
	private String account;
	private BitcoinClient client;
	private Economy econ;
	private Plugin plugin;
	
	public Model(
		Plugin plugin,
		Economy econ) {
		this.plugin = plugin;
		this.econ = econ;
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
		} catch (IOException e) {
			return e.toString();
		}
	}
	
	public String getBrokerageBalance() {
		try {
			return this.client.getBalance(this.account).toString();
		} catch (IOException e) {
			return "???";
		}
	}
	
	public PlayerAccountInfo getAccountInfo(Player player) throws Exception {
		String linkedAddress = "";
		if(player.hasMetadata(metadataKey)) {
			linkedAddress = player.getMetadata(metadataKey).get(0).asString();
		}
		
		PlayerAccountInfo result = new PlayerAccountInfo(
			this.client.getAccountAddress(player.getName()),
			this.client.getBalance(player.getName()),
			linkedAddress);
		return result;
	}
	
	public void setLinkedAddress(Player player, String linkedAddress) throws Exception {
		player.setMetadata(
			metadataKey, 
			new FixedMetadataValue(this.plugin, linkedAddress));
	}
}
