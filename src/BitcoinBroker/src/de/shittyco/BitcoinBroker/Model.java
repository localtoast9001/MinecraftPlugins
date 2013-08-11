/**
 * @author Jon Rowlett
 */
package de.shittyco.BitcoinBroker;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.Date;
import java.util.List;
import java.util.ResourceBundle;

import net.milkbowl.vault.economy.Economy;
import net.milkbowl.vault.economy.EconomyResponse;

import org.bukkit.OfflinePlayer;
import org.bukkit.entity.Player;
import org.bukkit.plugin.Plugin;

import de.shittyco.Bitcoin.BTC;
import de.shittyco.Bitcoin.BitcoinClient;
import de.shittyco.Bitcoin.BitcoinInfo;
import de.shittyco.Bitcoin.ServerErrorException;

/**
 * Model for the plugin in the MVC pattern.
 * @author Jon Rowlett
 */
public class Model {

    /**
     * Minimum commission.
     */
    private static final BTC MIN_COMMISSION = new BTC(0.00000001);

    /**
     * Resource Bundle for this class.
     */
    private static ResourceBundle bundle = ResourceBundle.getBundle("Model");

    /**
     * Config properties for the brokerage.
     */
    private BrokerageInfo brokerageInfo = new BrokerageInfo();

    /**
     * URL to contact a node on the Bitcoin network.
     */
    private URL bitcoinUrl;

    /**
     * Clearing house account.
     */
    private String account;

    /**
     * Account for commissions.
     */
    private String commissionAccount;

    /**
     * Client used to connect to the node on the Bitcoin network.
     */
    private BitcoinClient client;

    /**
     * Reference to the Vault economy service.
     */
    private Economy econ;

    /**
     * Reference back to the container plugin.
     */
    private Plugin plugin;

    /**
     * Config store for per-player data.
     */
    private PlayerDataStore playerDataStore;

    /**
     * Initializes a new instance of the Model class.
     * @param pluginRef - reference to the plugin.
     * @param econRef - reference to the economy service.
     */
    public Model(final Plugin pluginRef, final Economy econRef) {
        this.plugin = pluginRef;
        this.econ = econRef;
        this.playerDataStore = new PlayerDataStore(this.plugin);
    }

    /**
     * Gets the brokerage info.
     * @return the brokerage info reference.
     */
    public final BrokerageInfo getBrokerageInfo() {
        return this.brokerageInfo;
    }

    /**
     * Gets the Bitcoin URL.
     * @return the Bitcoin URL.
     */
    public final String getBitcoinUrl() {
        return this.bitcoinUrl.toString();
    }

    /**
     * Sets the bitcoin URL.
     * @param value - the new value.
     * @throws MalformedURLException - if the URL is not valid.
     */
    public final void setBitcoinUrl(
        final String value) throws MalformedURLException {
        this.bitcoinUrl = new URL(value);
    }

    /**
     * Sets the account name to use as a clearing house.
     * @param value - the account name.
     */
    public final void setAccount(final String value) {
        this.account = value;
    }

    /**
     * Gets the name of the account used as a clearing house.
     * @return the name of the account.
     */
    public final String getAccount() {
        return this.account;
    }

    /**
     * Sets the name of the account used for commission profit.
     * @param value - the name of the account.
     */
    public final void setCommissionAccount(final String value) {
        this.commissionAccount = value;
    }

    /**
     * Gets the name of the account used for commissions.
     * @return the name of the account.
     */
    public final String getCommissionAccount() {
        return this.commissionAccount;
    }

    /**
     * Initializes the model by connecting to the Bitcoin node.
     * @param user - the user name to connect.
     * @param password - the password to connect.
     * @return A display string to show in the console.
     */
    public final String init(final String user, final String password) {
        this.client = new BitcoinClient(this.bitcoinUrl, user, password);
        try {
            BitcoinInfo info = this.client.getInfo();
            return String.format("Connections=%d, Total Server Balance=%s",
                    info.getConnections(), info.getBalance().toString());
        } catch (ServerErrorException e) {
            return e.toString();
        }
    }

    /**
     * Gets the brokerage's balance.
     * @return - the brokerages balance.
     */
    public final String getBrokerageBalance() {
        try {
            return this.client.getBalance(this.account).toString();
        } catch (ServerErrorException e) {
            return "???";
        }
    }

    /**
     * Gets the commissions balance.
     * @return the commissions balance.
     */
    public final String getCommissionsBalance() {
        try {
            return this.client.getBalance(this.commissionAccount).toString();
        } catch (ServerErrorException e) {
            return "???";
        }
    }

    /**
     * Gets account info for a player.
     * @param player - the player to lookup.
     * @return the player account info.
     * @throws Exception - a communication exception.
     */
    public final PlayerAccountInfo getAccountInfo(final OfflinePlayer player)
            throws Exception {
        String linkedAddress = this.playerDataStore
                .getPlayerLinkedAddress(player);
        List<TransactionLogEntry> latestTransactions = this.playerDataStore
                .getPlayerLatestTransactions(player);
        PlayerAccountInfo result = new PlayerAccountInfo(
                this.client.getAccountAddress(player.getName()),
                this.client.getBalance(player.getName()), linkedAddress,
                latestTransactions);
        return result;
    }

    /**
     * Sets the linked address for the player.
     * @param player - the player to update.
     * @param linkedAddress - the new value.
     * @throws Exception - a communication exception.
     */
    public final void setLinkedAddress(
        final OfflinePlayer player,
        final String linkedAddress)
            throws Exception {
        this.playerDataStore.setLinkedAddress(player, linkedAddress);
    }

    /**
     * Sells BTC on behalf of the player.
     * @param player - the player to process.
     * @param value - the BTC to sell.
     * @return A transaction log entry to display.
     * @throws Exception - a communication exception or insufficient funds.
     */
    public final TransactionLogEntry sell(
        final Player player,
        final BTC value) throws Exception {
        BTC commission = BTC.mul(value,
                (float) this.brokerageInfo.getBtcToCoinsCommission());
        if (commission.equals(new BTC(0))
                && this.brokerageInfo.getBtcToCoinsCommission() > 0) {
            commission = MIN_COMMISSION;
        }

        BTC tradeValue = BTC.sub(value, commission);
        double coins = tradeValue.doubleValue()
                / this.brokerageInfo.getBtcToCoinsRate();
        this.client.move(player.getName(), this.account, value);
        EconomyResponse response = this.econ.depositPlayer(player.getName(),
                coins);
        if (!response.transactionSuccess()) {
            this.client.move(this.account, player.getName(), value);
            throw new Exception("Transaction failed.");
        }

        if (!commission.equals(new BTC(0))) {
            this.client.move(this.account, this.commissionAccount, commission);
        }

        String description = String.format("Sell %s BTC, Fee=%s BTC", value,
                commission);
        Date time = new Date();
        TransactionLogEntry entry = new TransactionLogEntry(time, description,
                BTC.sub(new BTC(0), value), coins,
                this.client.getBalance(player.getName()),
                this.econ.getBalance(player.getName()));
        this.playerDataStore.logTransactionEntry(player, entry);
        return entry;
    }

    /**
     * Buys BTC using coins.
     * @param player - the player making the purchase.
     * @param value - the amount of BTC desired.
     * @return the transaction log entry.
     * @throws Exception when insufficient funds or an error from Bitcoin.
     */
    public final TransactionLogEntry buy(
        final Player player,
        final BTC value) throws Exception {
        BTC commission = BTC.mul(value, (float) this.getBrokerageInfo()
                .getCoinsToBtcCommission());
        if (commission.equals(new BTC(0))
                && this.brokerageInfo.getCoinsToBtcCommission() > 0) {
            commission = MIN_COMMISSION;
        }

        BTC tradeValue = BTC.add(value, commission);
        if (tradeValue.compareTo(this.client.getBalance(this.account)) > 0) {
            throw new Exception(
                bundle.getString("Error_InsufficientFunds"));
        }

        double coins = tradeValue.doubleValue()
                * this.brokerageInfo.getCoinsToBtcRate();
        EconomyResponse response = this.econ.withdrawPlayer(player.getName(),
                coins);
        if (!response.transactionSuccess()) {
            throw new Exception("Transaction failed.");
        }

        this.client.move(this.account, player.getName(), value);
        if (!commission.equals(new BTC(0))) {
            this.client.move(this.account, this.commissionAccount, commission);
        }

        double commissionCoins = commission.doubleValue()
                * this.brokerageInfo.getCoinsToBtcRate();

        TransactionLogEntry entry = new TransactionLogEntry(new Date(),
                String.format("Buy %s BTC, Fee=%s coins", value,
                        ((Double) commissionCoins).toString()), value, -coins,
                this.client.getBalance(player.getName()),
                this.econ.getBalance(player.getName()));
        this.playerDataStore.logTransactionEntry(player, entry);
        return entry;
    }

    /**
     * Transfers BTC outside the brokerage to the player's linked BTC address.
     * @param player - the player making the transfer
     * @param value - the value to transfer.
     * @return - the transaction log entry.
     * @throws Exception - if insufficient funds or error from Bitcoin.
     */
    public final TransactionLogEntry transfer(
        final Player player,
        final BTC value)
        throws Exception {
        String linkedAddress = this.playerDataStore
                .getPlayerLinkedAddress(player);
        if (linkedAddress.length() == 0) {
            throw new Exception(
                bundle.getString("Error_NoPlayerLinkedAddress"));
        }

        String txid = this.client.sendFrom(player.getName(), linkedAddress,
                value);
        String description = String.format(
                "Sent %s BTC to [%s]. Transaction ID=%s", value, linkedAddress,
                txid);
        TransactionLogEntry entry = new TransactionLogEntry(new Date(),
                description, BTC.sub(new BTC(0), value), 0,
                this.client.getBalance(player.getName()),
                this.econ.getBalance(player.getName()));
        this.playerDataStore.logTransactionEntry(player, entry);
        return entry;
    }

    /**
     * Transfers from the brokerage's profit to an external account specified in
     * configuration.
     *
     * @param value
     *            - the amount to transfer.
     * @return the transaction id.
     * @throws Exception
     *             - there is no account set up, insufficient funds, or error
     *             from Bitcoin.
     */
    public final String cashOut(final BTC value) throws Exception {
        String profitAddress = this.brokerageInfo.getProfitAddress();
        if (profitAddress == null || profitAddress.length() == 0) {
            throw new Exception(
                    bundle.getString("Error_NoBrokerageProfitAddress"));
        }

        return this.client.sendFrom(this.commissionAccount, profitAddress,
                value);
    }
}
