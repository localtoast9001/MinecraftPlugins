package de.shittyco.Bitcoin;

public class BitcoinInfo {
	private int version; 
	private int protocolVersion;
	private int walletVersion;
	private BTC balance;
	private int blocks;
	private int connections;
	private String proxy;
	private double difficulty;
	private boolean testnet;
	private int keyPoolOldest;
	private int keyPoolSize;
	private BTC payTxFee;
	private String errors;
	
	public int getVersion() { return this.version; }
	public void setVersion(int value) { this.version = value; }
	public int getProtocolVersion() { return this.protocolVersion; }
	public void setProtocolVersion(int value) { this.protocolVersion = value; }
	public int getWalletVersion() { return this.walletVersion; }
	public void setWalletVersion(int value) { this.walletVersion = value; }
	public BTC getBalance() { return this.balance; }
	public void setBalance(BTC value) { this.balance = value; }
	public int getBlocks() { return this.blocks; }
	public void setBlocks(int value) { this.blocks = value; }
	public int getConnections() { return this.connections; }
	public void setConnections(int value) { this.connections = value; }
	public String getProxy() { return this.proxy; }
	public void setProxy(String value) { this.proxy = value; }
	public double getDifficulty() { return this.difficulty; }
	public void setDifficulty(double value) { this.difficulty = value; }
	public boolean getTestnet() { return this.testnet; }
	public void setTestnet(boolean value) { this.testnet = value; }
	public int getKeyPoolOldest() { return this.keyPoolOldest; }
	public void setKeyPoolOldest(int value) { this.keyPoolOldest = value; }
	public int getKeyPoolSize() { return this.keyPoolSize; }
	public void setKeyPoolSize(int value) { this.keyPoolSize = value; }
	public BTC getPayTxFee() { return this.payTxFee; }
	public void setPayTxFee(BTC value) { this.payTxFee = value; }
	public String getErrors() { return this.errors; }
	public void setErrors(String value) { this.errors = value; }
}
