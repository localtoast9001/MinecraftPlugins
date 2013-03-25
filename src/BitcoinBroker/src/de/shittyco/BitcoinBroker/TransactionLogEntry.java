package de.shittyco.BitcoinBroker;

import java.text.DateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;

import org.bukkit.configuration.serialization.ConfigurationSerializable;
import org.bukkit.configuration.serialization.SerializableAs;

import de.shittyco.Bitcoin.BTC;

/**
 * Information about a transaction for display.
 *
 * @author jrowlett
 *
 */
@SerializableAs("Transaction")
public class TransactionLogEntry implements ConfigurationSerializable {
    /**
     * The date format to use for time display.
     */
    private static DateFormat dateFormat = DateFormat.getInstance();

    /**
     * the time of the transaction.
     */
    private Date time;
    /**
     * the description of the transaction.
     */
    private String description;
    /**
     * the change in BTC balance.
     */
    private BTC btcChange;
    /**
     * the change in coins balance.
     */
    private double coinsChange;
    /**
     * the new BTC balance.
     */
    private BTC btcBalance;
    /**
     * the new coins balance.
     */
    private double coinsBalance;

    /**
     * Initializes a new instance of the TransactionLogEntry class.
     * @param timeValue - the time of the transaction.
     * @param descriptionValue - the description of the transaction.
     * @param btcChangeValue - the change in BTC.
     * @param coinsChangeValue - the change in coins.
     * @param btcBalanceValue - the new BTC balance.
     * @param coinsBalanceValue - the new coins balance.
     */
    public TransactionLogEntry(
        final Date timeValue,
        final String descriptionValue,
        final BTC btcChangeValue,
        final double coinsChangeValue,
        final BTC btcBalanceValue,
        final double coinsBalanceValue) {

        if (timeValue == null) {
            throw new IllegalArgumentException("time");
        }

        if (descriptionValue == null) {
            throw new IllegalArgumentException("description");
        }

        if (btcChangeValue == null) {
            throw new IllegalArgumentException("btcChange");
        }

        if (btcBalanceValue == null) {
            throw new IllegalArgumentException("btcBalance");
        }

        this.time = timeValue;
        this.description = descriptionValue;
        this.btcChange = btcChangeValue;
        this.coinsChange = coinsChangeValue;
        this.btcBalance = btcBalanceValue;
        this.coinsBalance = coinsBalanceValue;
    }

    /**
     * @param args property bag to deserialize
     * @return a deserialized object
     * @throws Exception when input fails to validate
     */
    public static TransactionLogEntry deserialize(
        final Map<String, Object> args)
        throws Exception {
        Date time = dateFormat.parse(args.get("Time").toString());
        String description = args.get("Description").toString();
        BTC btcChange = new BTC(args.get("BtcChange").toString());
        double coinsChange = (Double) args.get("CoinsChange");
        BTC btcBalance = new BTC(args.get("BtcBalance").toString());
        double coinsBalance = (Double) args.get("CoinsBalance");
        return new TransactionLogEntry(time, description, btcChange,
                coinsChange, btcBalance, coinsBalance);
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
        result.put("Time", dateFormat.format(this.time));
        result.put("Description", this.description);
        result.put("BtcChange", this.btcChange.toString());
        result.put("CoinsChange", this.coinsChange);
        result.put("BtcBalance", this.btcBalance.toString());
        result.put("CoinsBalance", this.coinsBalance);
        return result;
    }

    /**
     * @return the time of transaction
     */
    public final Date getTime() {
        return this.time;
    }

    /**
     * @return the description of the transaction
     */
    public final String getDescription() {
        return this.description;
    }

    /**
     * @return the change in the BTC balance.
     */
    public final BTC getBtcChange() {
        return this.btcChange;
    }

    /**
     * @return the change in the coins balance.
     */
    public final double getCoinsChange() {
        return this.coinsChange;
    }

    /**
     * @return the new BTC balance after the transaction.
     */
    public final BTC getBtcBalance() {
        return this.btcBalance;
    }

    /**
     * @return the new coins balance after the transaction.
     */
    public final double getCoinsBalance() {
        return this.coinsBalance;
    }
}
