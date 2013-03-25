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

    public final int getVersion() {
        return this.version;
    }

    public final void setVersion(final int value) {
        this.version = value;
    }

    public final int getProtocolVersion() {
        return this.protocolVersion;
    }

    public final void setProtocolVersion(final int value) {
        this.protocolVersion = value;
    }

    public final int getWalletVersion() {
        return this.walletVersion;
    }

    public final void setWalletVersion(final int value) {
        this.walletVersion = value;
    }

    public final BTC getBalance() {
        return this.balance;
    }

    public final void setBalance(final BTC value) {
        this.balance = value;
    }

    public final int getBlocks() {
        return this.blocks;
    }

    public final void setBlocks(final int value) {
        this.blocks = value;
    }

    public final int getConnections() {
        return this.connections;
    }

    public final void setConnections(final int value) {
        this.connections = value;
    }

    public final String getProxy() {
        return this.proxy;
    }

    public final void setProxy(final String value) {
        this.proxy = value;
    }

    public final double getDifficulty() {
        return this.difficulty;
    }

    public final void setDifficulty(final double value) {
        this.difficulty = value;
    }

    public final boolean getTestnet() {
        return this.testnet;
    }

    public final void setTestnet(final boolean value) {
        this.testnet = value;
    }

    public final int getKeyPoolOldest() {
        return this.keyPoolOldest;
    }

    public final void setKeyPoolOldest(final int value) {
        this.keyPoolOldest = value;
    }

    public final int getKeyPoolSize() {
        return this.keyPoolSize;
    }

    public final void setKeyPoolSize(final int value) {
        this.keyPoolSize = value;
    }

    public final BTC getPayTxFee() {
        return this.payTxFee;
    }

    public final void setPayTxFee(final BTC value) {
        this.payTxFee = value;
    }

    public final String getErrors() {
        return this.errors;
    }

    public final void setErrors(final String value) {
        this.errors = value;
    }
}
