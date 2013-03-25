/**
 *
 */
package de.shittyco.BitcoinBroker;

/**
 * Properties of the brokerage.
 * @author Jon Rowlett
 */
public class BrokerageInfo {
    /**
     * Convert ratio to percentage.
     */
    private static final int PERCENT = 100;

    /**
     * The default exchange rate of coins per BTC.
     */
    private static final double DEFAULT_RATE = 1000000;

    /**
     * The default commission margin.
     */
    private static final double DEFAULT_COMMISSION = 0.001;

    /**
     * The BTC to coins conversion rate.
     */
    private double btcToCoinsRate = 1.0 / DEFAULT_RATE;

    /**
     * The Coins to BTC converstion rate.
     */
    private double coinsToBtcRate = DEFAULT_RATE;

    /**
     * The commission margin when converting BTC to coins.
     */
    private double btcToCoinsCommission = DEFAULT_COMMISSION;

    /**
     * The commission margin when converting coins to BTC.
     */
    private double coinsToBtcCommission = DEFAULT_COMMISSION;

    /**
     * The BTC address to send profits from commissions.
     */
    private String profitAddress;

    /**
     * Gets the BTC to coins rate.
     * @return the BTC to coins rate.
     */
    public final double getBtcToCoinsRate() {
        return this.btcToCoinsRate;
    }

    /**
     * Sets the BTC to coins rate.
     * @param value - the new rate.
     */
    public final void setBtcToCoinsRate(final double value) {
        this.btcToCoinsRate = value;
    }

    /**
     * Gets the coins to BTC rate.
     * @return the coins to BTC rate.
     */
    public final double getCoinsToBtcRate() {
        return this.coinsToBtcRate;
    }

    /**
     * Sets the coins to BTC rate.
     * @param value - the new rate.
     */
    public final void setCoinsToBtcRate(final double value) {
        this.coinsToBtcRate = value;
    }

    /**
     * Gets the BTC to coins commission.
     * @return the BTC to coins commission.
     */
    public final double getBtcToCoinsCommission() {
        return this.btcToCoinsCommission;
    }

    /**
     * Sets the BTC to coins commission.
     * @param value - the new commission.
     */
    public final void setBtcToCoinsCommission(final double value) {
        this.btcToCoinsCommission = value;
    }

    /**
     * Gets the Coins to BTC commission.
     * @return the coins to BTC commission.
     */
    public final double getCoinsToBtcCommission() {
        return this.coinsToBtcCommission;
    }

    /**
     * Sets the coins to BTC commission.
     * @param value - the new commission.
     */
    public final void setCoinsToBtcCommission(final double value) {
        this.coinsToBtcCommission = value;
    }

    /**
     * Gets the profit address.
     * @return the profit address.
     */
    public final String getProfitAddress() {
        return this.profitAddress;
    }

    /**
     * Sets the profit address.
     * @param value - the new address.
     */
    public final void setProfitAddress(final String value) {
        this.profitAddress = value;
    }

   /* (non-Javadoc)
    * @see java.lang.Object#toString()
    */
    @Override
    public final String toString() {
        return String.format("Coin: %fBTC - %f%%, BTC: %f Coin - %f%%",
                this.btcToCoinsRate, this.btcToCoinsCommission * PERCENT,
                this.coinsToBtcRate, this.coinsToBtcCommission * PERCENT);
    }
}
