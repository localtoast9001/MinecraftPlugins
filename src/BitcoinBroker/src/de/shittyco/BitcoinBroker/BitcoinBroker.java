/**
 * @author Jon Rowlett
 */
package de.shittyco.BitcoinBroker;

import java.net.MalformedURLException;
import java.util.logging.Level;

import org.bukkit.configuration.file.*;
import org.bukkit.plugin.java.*;

/**
 * @author Jon Rowlett
 *
 */
public class BitcoinBroker extends JavaPlugin {
	private Model model = new Model();
	
	public void onEnable() {
		this.initModel();
		getCommand("brokerage").setExecutor(new BrokerageCommandExecutor(this.model));
		getLogger().info(this.model.getBrokerageInfo().toString());
	}
 
	public void onDisable() {
		getLogger().info("Brokerage is now disabled.");
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
		}
		this.model.setAccount(config.getString("account"));
		this.model.init(config.getString("rpcuser"), config.getString("rpcpassword"));
		getLogger().info(String.format("URL: %s, Account: %s", this.model.getBitcoinUrl(), this.model.getAccount()));
	}
}
