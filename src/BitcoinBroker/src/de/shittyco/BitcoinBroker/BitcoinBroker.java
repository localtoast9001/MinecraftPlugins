/**
 * @author Jon Rowlett
 */
package de.shittyco.BitcoinBroker;

import java.net.MalformedURLException;
import java.util.logging.Level;

import org.bukkit.configuration.file.*;
import org.bukkit.configuration.serialization.ConfigurationSerialization;
import org.bukkit.plugin.RegisteredServiceProvider;
import org.bukkit.plugin.java.*;
import net.milkbowl.vault.economy.Economy;

/**
 * Bukkit Plugin for the Bitcoin Broker.
 * @author Jon Rowlett
 *
 */
public class BitcoinBroker extends JavaPlugin {

    /**
     * The internal object model for the plug-in that holds state.
     */
    private Model model = null;

    /**
     * Constructs the plugin.
     */
    public BitcoinBroker() {
        ConfigurationSerialization.registerClass(PlayerData.class);
        ConfigurationSerialization.registerClass(TransactionLogEntry.class);
    }

    /* (non-Javadoc)
     * @see org.bukkit.plugin.java.JavaPlugin#onEnable()
     */
    @Override
    public final void onEnable() {
        Economy econ = this.initEconomy();
        if (econ == null) {
            getLogger().log(
                Level.SEVERE,
                "Vault Plugin is not loaded. Bitcoin Broker requires it. Make sure it is installed.");
            return;
        }

        this.model = new Model(this, econ);
        this.initModel();
        getCommand("btc").setExecutor(
                new RootCommandExecutor(this.model, this.getServer()));
        getLogger().info(this.model.getBrokerageInfo().toString());
    }

    /* (non-Javadoc)
     * @see org.bukkit.plugin.java.JavaPlugin#onDisable()
     */
    @Override
    public final void onDisable() {
        getLogger().info("Brokerage is now disabled.");
    }

    /**
     * @return the economy object provided by Vault
     */
    private Economy initEconomy() {
        if (this.getServer().getPluginManager().getPlugin("Vault") == null) {
            return null;
        }

        RegisteredServiceProvider<Economy> rsp = getServer()
                .getServicesManager().getRegistration(Economy.class);
        if (rsp == null) {
            return null;
        }

        Economy econ = rsp.getProvider();
        return econ;
    }

    /**
     * Performs initialization.
     */
    private void initModel() {
        FileConfiguration config = this.getConfig();
        BrokerageInfo brokerageInfo = this.model.getBrokerageInfo();
        brokerageInfo.setBtcToCoinsRate((float) config
                .getDouble("brokerage.btcToCoinsRate"));
        brokerageInfo.setCoinsToBtcRate((float) config
                .getDouble("brokerage.coinsToBtcRate"));
        brokerageInfo.setBtcToCoinsCommission((float) config
                .getDouble("brokerage.btcToCoinsCommission"));
        brokerageInfo.setCoinsToBtcCommission((float) config
                .getDouble("brokerage.coinsToBtcCommission"));
        if (!config.contains("brokerage.profitAddress")) {
            if (brokerageInfo.getCoinsToBtcCommission() > 0
                    || brokerageInfo.getBtcToCoinsCommission() > 0) {
                getLogger().log(
                    Level.WARNING,
                    "brokerage.profitAddress is missing. No commissions can be transferred out.");
                brokerageInfo.setBtcToCoinsCommission(0);
                brokerageInfo.setCoinsToBtcCommission(0);
            }
        } else {
            brokerageInfo.setProfitAddress(config
                    .getString("brokerage.profitAddress"));
        }

        try {
            this.model.setBitcoinUrl(config.getString("bitcoinUrl"));
        } catch (MalformedURLException e) {
            getLogger().log(Level.SEVERE, e.getLocalizedMessage());
            return;
        }

        String account = config.getString("account");
        String commissionAccount = config.getString("commissionAccount");
        String rpcUser = config.getString("rpcuser");
        if (rpcUser == null || rpcUser.isEmpty()) {
            getLogger().log(
                Level.SEVERE,
                "Empty user name. Set rpcuser in BitcoinBroker/config.yml");
            return;
        }

        String rpcPassword = config.getString("rpcpassword");
        if (rpcPassword == null || rpcPassword.isEmpty()) {
            getLogger().log(
                Level.SEVERE,
                "Empty password. Set rpcpassword in BitcoinBroker/config.yml");
            return;
        }

        this.model.setAccount(account);
        this.model.setCommissionAccount(commissionAccount);
        getLogger().info(this.model.init(rpcUser, rpcPassword));
        getLogger().info(
                String.format("URL: %s, Account: %s",
                        this.model.getBitcoinUrl(), this.model.getAccount()));
    }
}
