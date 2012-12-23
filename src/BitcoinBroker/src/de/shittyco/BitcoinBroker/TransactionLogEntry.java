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
 * @author jrowlett
 *
 */
@SerializableAs("Transaction")
public class TransactionLogEntry implements ConfigurationSerializable {
	private static DateFormat dateFormat = DateFormat.getInstance();
	
	private Date time;
	private String description;
	private BTC btcChange;
	private double coinsChange;
	private BTC btcBalance;
	private double coinsBalance;
	
	public TransactionLogEntry(
		Date time,
		String description,
		BTC btcChange,
		double coinsChange,
		BTC btcBalance,
		double coinsBalance) {
		
		if (time == null) {
			throw new IllegalArgumentException("time");
		}
		
		if (description == null) {
			throw new IllegalArgumentException("description");
		}
		
		if (btcChange == null) {
			throw new IllegalArgumentException("btcChange");
		}
		
		if (btcBalance == null) {
			throw new IllegalArgumentException("btcBalance");
		}
		
		this.time = time;
		this.description = description;
		this.btcChange = btcChange;
		this.coinsChange = coinsChange;
		this.btcBalance = btcBalance;
		this.coinsBalance = coinsBalance;
	}
	
	public static TransactionLogEntry deserialize(Map<String, Object> args) throws Exception {
		Date time = dateFormat.parse(args.get("Time").toString());
		String description = args.get("Description").toString();
		BTC btcChange = new BTC(args.get("BtcChange").toString());
		double coinsChange = (Double)args.get("CoinsChange");
		BTC btcBalance = new BTC(args.get("BtcBalance").toString());
		double coinsBalance = (Double)args.get("CoinsBalance");
		return new TransactionLogEntry(time, description, btcChange, coinsChange, btcBalance, coinsBalance);
	}
	
	@Override
	public Map<String, Object> serialize() {
		HashMap<String, Object> result = new HashMap<String, Object>();
		result.put("Time", dateFormat.format(this.time));
		result.put("Description", this.description);
		result.put("BtcChange", this.btcChange.toString());
		result.put("CoinsChange", this.coinsChange);
		result.put("BtcBalance", this.btcBalance.toString());
		result.put("CoinsBalance", this.coinsBalance);
		return result;
	}
	
	public Date getTime() {
		return this.time;
	}
	
	public String getDescription() {
		return this.description;
	}
	
	public BTC getBtcChange() {
		return this.btcChange;
	}
	
	public double getCoinsChange() {
		return this.coinsChange;
	}
	
	public BTC getBtcBalance() {
		return this.btcBalance;
	}
	
	public double getCoinsBalance() {
		return this.coinsBalance;
	}
}
