/**
 *
 */
package de.shittyco.BitcoinBroker;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Vector;

import org.bukkit.configuration.serialization.ConfigurationSerializable;
import org.bukkit.configuration.serialization.SerializableAs;

/**
 * Per-player config storage. This object is de-serialized from YML.
 * @author jrowlett
 *
 */
@SerializableAs("PlayerData")
public class PlayerData implements ConfigurationSerializable {
    /**
     * Name of the linked address property in storage.
     */
    private static final String LINKED_ADDRESS_KEY = "BTCLINKEDADDRESS";

    /**
     * Name of the key to locate the transaction history.
     */
    private static final String TRANSACTIONS_KEY = "LATESTTRANSACTIONS";

    /**
     * The player's linked address.
     */
    private String btcLinkedAddress;

    /**
     * The player's latest transactions.
     */
    private List<TransactionLogEntry> latestTransactions;

    /**
     * Initializes a new instance of the PlayerData class.
     */
    public PlayerData() {
        this.btcLinkedAddress = "";
        this.latestTransactions = new Vector<TransactionLogEntry>();
    }

    /**
     * Initializes a new instance of the PlayerData class.
     * @param linkedAddress - the linked address.
     * @param latestTransactionsList - the latest transactions.
     */
    public PlayerData(
        final String linkedAddress,
        final List<TransactionLogEntry> latestTransactionsList) {
        this.btcLinkedAddress = linkedAddress;
        this.latestTransactions = latestTransactionsList;
    }

    /**
     * Deserializes config data into a new instance of the object.
     * @param args - the raw config data.
     * @return - the deserialized player data.
     */
    public static PlayerData deserialize(final Map<String, Object> args) {
        String btcLinkedAddress = (String) args.get(LINKED_ADDRESS_KEY);
        List<TransactionLogEntry> latestTransactions =
            new Vector<TransactionLogEntry>();
        for (Object obj : (List<?>) args.get(TRANSACTIONS_KEY)) {
            latestTransactions.add((TransactionLogEntry) obj);
        }

        return new PlayerData(btcLinkedAddress, latestTransactions);
    }

    /*
     * (non-Javadoc)
     *
     * @see
     * org.bukkit.configuration.serialization
     * .ConfigurationSerializable#serialize
     * ()
     */
    @Override
    public final Map<String, Object> serialize() {
        HashMap<String, Object> result = new HashMap<String, Object>();
        result.put(LINKED_ADDRESS_KEY, this.btcLinkedAddress);
        result.put(TRANSACTIONS_KEY, this.latestTransactions);
        return result;
    }

    /**
     * Gets the linked BTC address.
     * @return - the linked address.
     */
    public final String getBtcLinkedAddress() {
        return this.btcLinkedAddress;
    }

    /**
     * Sets the linked Address.
     * @param value - the new address.
     */
    public final void setBtcLinkedAddress(final String value) {
        this.btcLinkedAddress = value;
    }

    /**
     * Gets the latest list of transactions.
     * @return - the list of transactions.
     */
    public final List<TransactionLogEntry> getLatestTransactions() {
        return this.latestTransactions;
    }
}
