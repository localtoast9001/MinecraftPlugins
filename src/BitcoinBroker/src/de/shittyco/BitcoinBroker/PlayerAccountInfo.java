package de.shittyco.BitcoinBroker;

import de.shittyco.Bitcoin.BTC;

public class PlayerAccountInfo {
	private String address;
	private BTC balance;
	private String linkedAddress;

	public PlayerAccountInfo(
		String address,
		BTC balance,
		String linkedAddress) {
		this.address = address;
		this.balance = balance;
		this.linkedAddress = linkedAddress;
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
	
	@Override
	public String toString() {
		return String.format(
				"Deposit by sending BTC here: %s\nBalance: %s\n Withdrawl BTC here: %s",
				this.address,
				this.balance.toString(),
				this.linkedAddress);
	}
}
