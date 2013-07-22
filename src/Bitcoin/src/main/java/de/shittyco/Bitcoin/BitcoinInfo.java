package de.shittyco.Bitcoin;

/**
 * Basic information about the bitcoin server.
 * @author jrowlett
 */
public class BitcoinInfo {
    /**
     * version number.
     */
    private int version;

    /**
     * version of the protocol in use.
     */
    private int protocolVersion;

    /**
     * Version of the wallet file.
     */
    private int walletVersion;

    /**
     * total balance.
     */
    private BTC balance;

    /**
     * Number of blocks.
     */
    private int blocks;

    /**
     * Number of peer connections.
     */
    private int connections;

    /**
     * proxy being used.
     */
    private String proxy;

    /**
     * Current block difficulty.
     */
    private double difficulty;

    /**
     * whether or not this is the test network.
     */
    private boolean testnet;

    /**
     * Oldest key pool.
     */
    private int keyPoolOldest;

    /**
     * Size of the key pool.
     */
    private int keyPoolSize;

    /**
     * transaction fee.
     */
    private BTC payTxFee;

    /**
     * Any errors.
     */
    private String errors;

    /**
     * Gets the version.
     * @return the version.
     */
    public final int getVersion() {
        return this.version;
    }

    /**
     * Gets the version.
     * @param value the new version.
     */
    public final void setVersion(final int value) {
        this.version = value;
    }

    /**
     * Gets the protocol version.
     * @return the protocol version.
     */
    public final int getProtocolVersion() {
        return this.protocolVersion;
    }

    /**
     * Sets the protocol version.
     * @param value The version value.
     */
    public final void setProtocolVersion(final int value) {
        this.protocolVersion = value;
    }

    /**
     * Gets the wallet version.
     * @return the wallet version.
     */
    public final int getWalletVersion() {
        return this.walletVersion;
    }

    /**
     * Sets the wallet version.
     * @param value the version value.
     */
    public final void setWalletVersion(final int value) {
        this.walletVersion = value;
    }

    /**
     * Gets the balance.
     * @return the balance.
     */
    public final BTC getBalance() {
        return this.balance;
    }

    /**
     * Sets the balance.
     * @param value the balance value.
     */
    public final void setBalance(final BTC value) {
        this.balance = value;
    }

    /**
     * Gets the number of blocks.
     * @return the number of blocks.
     */
    public final int getBlocks() {
        return this.blocks;
    }

    /**
     * Sets the number of blocks.
     * @param value number of blocks value.
     */
    public final void setBlocks(final int value) {
        this.blocks = value;
    }

    /**
     * Gets the number of peer connections.
     * @return the number of peer connections.
     */
    public final int getConnections() {
        return this.connections;
    }

    /**
     * Sets the number of peer connections.
     * @param value the number of peer connections.
     */
    public final void setConnections(final int value) {
        this.connections = value;
    }

    /**
     * Gets the proxy.
     * @return the proxy address or null.
     */
    public final String getProxy() {
        return this.proxy;
    }

    /**
     * Sets the proxy.
     * @param value the proxy address.
     */
    public final void setProxy(final String value) {
        this.proxy = value;
    }

    /**
     * Gets the difficulty.
     * @return the current block difficulty.
     */
    public final double getDifficulty() {
        return this.difficulty;
    }

    /**
     * Sets the difficulty.
     * @param value the difficulty to set.
     */
    public final void setDifficulty(final double value) {
        this.difficulty = value;
    }

    /**
     * Gets whether or not the server is on the test network.
     * @return whether or not the test network is being used.
     */
    public final boolean getTestnet() {
        return this.testnet;
    }

    /**
     * Sets whether or not the server is on the test network.
     * @param value the value to set.
     */
    public final void setTestnet(final boolean value) {
        this.testnet = value;
    }

    /**
     * Gets the oldest key pool.
     * @return the oldest key pool.
     */
    public final int getKeyPoolOldest() {
        return this.keyPoolOldest;
    }

    /**
     * Sets the oldest key pool.
     * @param value the new value.
     */
    public final void setKeyPoolOldest(final int value) {
        this.keyPoolOldest = value;
    }

    /**
     * Gets the key pool size.
     * @return the key pool size.
     */
    public final int getKeyPoolSize() {
        return this.keyPoolSize;
    }

    /**
     * Sets the key pool size.
     * @param value the new value.
     */
    public final void setKeyPoolSize(final int value) {
        this.keyPoolSize = value;
    }

    /**
     * Gets the transaction fee.
     * @return the transaction fee.
     */
    public final BTC getPayTxFee() {
        return this.payTxFee;
    }

    /**
     * Sets the transaction fee.
     * @param value the new value.
     */
    public final void setPayTxFee(final BTC value) {
        this.payTxFee = value;
    }

    /**
     * Gets the errors.
     * @return the errors.
     */
    public final String getErrors() {
        return this.errors;
    }

    /**
     * Sets the errors.
     * @param value the error value.
     */
    public final void setErrors(final String value) {
        this.errors = value;
    }
}
