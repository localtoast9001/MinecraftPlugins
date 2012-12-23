package de.shittyco.BitcoinBroker;

import java.util.List;
import java.util.Vector;

import de.shittyco.Bitcoin.BTC;

public class PlayerAccountInfo {
	private String address;
	private BTC balance;
	private String linkedAddress;
	private Vector<TransactionLogEntry> logEntries;

	public PlayerAccountInfo(
		String address,
		BTC balance,
		String linkedAddress,
		List<TransactionLogEntry> logEntries) {
		this.logEntries = new Vector<TransactionLogEntry>();
		this.address = address;
		this.balance = balance;
		this.linkedAddress = linkedAddress;
		this.logEntries.addAll(logEntries);
	}

	public String getAddress() {
		return this.address;
	}
	
	public BTC getBalance() {
		return this.balance;
	}
	
	public String getLinkedAddress() {
		return this.linkedAddress;
	}
	
	public List<TransactionLogEntry> getLatestTransactions() {
		return this.logEntries;
	}
	
	@Override
	public String toString() {
		return String.format(
				"Deposit by sending BTC here: %s\nBalance: %s\n Withdrawl BTC here: %s",
				this.address,
				this.balance.toString(),
				this.linkedAddress);
	}
}
