package de.shittyco.BitcoinBroker;

import java.util.List;
import java.util.ResourceBundle;
import java.util.Vector;

import de.shittyco.Bitcoin.BTC;

/**
 * Information about an individual player's account.
 * @author jrowlett
 */
public class PlayerAccountInfo {

    /**
     * Resource bundle for this class.
     */
    private static ResourceBundle bundle =
        ResourceBundle.getBundle("PlayerAccountInfo");

    /**
     * The player's address to send funds.
     */
    private String address;

    /**
     * the balance on the server for the player.
     */
    private BTC balance;

    /**
     * The player's linked address for pay out.
     */
    private String linkedAddress;

    /**
     * the player's latest transactions.
     */
    private Vector<TransactionLogEntry> logEntries;

    /**
     * Initializes a new instance of the PlayerAccountInfo class.
     * @param btcAddress the player's BTC address.
     * @param btcBalance the player's BTC balance.
     * @param btcLinkedAddress the player's linked BTC address.
     * @param transactionLogEntries the latest log entries.
     */
    public PlayerAccountInfo(
        final String btcAddress,
        final BTC btcBalance,
        final String btcLinkedAddress,
        final List<TransactionLogEntry> transactionLogEntries) {
        this.logEntries = new Vector<TransactionLogEntry>();
        this.address = btcAddress;
        this.balance = btcBalance;
        this.linkedAddress = btcLinkedAddress;
        this.logEntries.addAll(transactionLogEntries);
    }

    /**
     * Gets the deposit address.
     * @return the deposit address.
     */
    public final String getAddress() {
        return this.address;
    }

    /**
     * Gets the balance.
     * @return the player balance
     */
    public final BTC getBalance() {
        return this.balance;
    }

    /**
     * Gets the linked address for cash out.
     * @return the linked address.
     */
    public final String getLinkedAddress() {
        return this.linkedAddress;
    }

    /**
     * Gets the latest transactions.
     * @return the list of transactions.
     */
    public final List<TransactionLogEntry> getLatestTransactions() {
        return this.logEntries;
    }

    /* (non-Javadoc)
     * @see java.lang.Object#toString()
     */
    @Override
    public final String toString() {
        return String.format(
            bundle.getString("ToStringFormat"),
            this.address,
            this.balance.toString(),
            this.linkedAddress);
    }
}
