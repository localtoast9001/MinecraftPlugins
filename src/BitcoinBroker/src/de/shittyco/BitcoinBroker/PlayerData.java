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
 * @author jrowlett
 *
 */
@SerializableAs("PlayerData")
public class PlayerData implements ConfigurationSerializable {
	private static String linkedAddressKey = "BTCLINKEDADDRESS";
	private static String transactionsKey = "LATESTTRANSACTIONS";
	
	private String btcLinkedAddress;
	private List<TransactionLogEntry> latestTransactions;
	
	public PlayerData() {
		this.btcLinkedAddress = "";
		this.latestTransactions = new Vector<TransactionLogEntry>();
	}
	
	public PlayerData(String btcLinkedAddress, List<TransactionLogEntry> latestTransactions) {
		this.btcLinkedAddress = btcLinkedAddress;
		this.latestTransactions = latestTransactions;
	}

	public static PlayerData deserialize(Map<String, Object> args) {
		String btcLinkedAddress = (String) args.get(linkedAddressKey);
		List<TransactionLogEntry> latestTransactions = new Vector<TransactionLogEntry>();
		for (Object obj : (List<?>) args.get(transactionsKey)) {
			latestTransactions.add((TransactionLogEntry) obj);
		}
		
		return new PlayerData(btcLinkedAddress, latestTransactions);
	}
	
	/* (non-Javadoc)
	 * @see org.bukkit.configuration.serialization.ConfigurationSerializable#serialize()
	 */
	@Override
	public Map<String, Object> serialize() {
		HashMap<String, Object> result = new HashMap<String, Object>();
		result.put(linkedAddressKey, this.btcLinkedAddress);
		result.put(transactionsKey, this.latestTransactions);
		return result;
	}

	public String getBtcLinkedAddress() {
		return this.btcLinkedAddress;
	}
	
	public void setBtcLinkedAddress(String value) {
		this.btcLinkedAddress = value;
	}
	
	public List<TransactionLogEntry> getLatestTransactions() {
		return this.latestTransactions;
	}
}
