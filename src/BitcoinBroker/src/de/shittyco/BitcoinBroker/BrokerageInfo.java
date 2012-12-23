/**
 * 
 */
package de.shittyco.BitcoinBroker;

/**
 * @author Jon Rowlett
 *
 */
public class BrokerageInfo {
	private double btcToCoinsRate = 0.000001;
	private double coinsToBtcRate = 1000000;
	private double btcToCoinsCommission = 0.001;
	private double coinsToBtcCommission = 0.001;
	private String profitAddress;
	
	public double getBtcToCoinsRate() {
		return this.btcToCoinsRate;
	}
	
	public void setBtcToCoinsRate(double value) {
		this.btcToCoinsRate = value;
	}
	
	public double getCoinsToBtcRate() {
		return this.coinsToBtcRate;
	}
	
	public void setCoinsToBtcRate(double value) { 
		this.coinsToBtcRate = value;
	}
	
	public double getBtcToCoinsCommission() { 
		return this.btcToCoinsCommission;
	}
	
	public void setBtcToCoinsCommission(double value) {
		this.btcToCoinsCommission = value;
	}
	
	public double getCoinsToBtcCommission() {
		return this.coinsToBtcCommission;
	}
	
	public void setCoinsToBtcCommission(double value) {
		this.coinsToBtcCommission = value;
	}
	
	public String getProfitAddress() {
		return this.profitAddress;
	}
	
	public void setProfitAddress(String value) {
		this.profitAddress = value;
	}
	
	public String toString() {
		return String.format(
			"Coin: %fBTC - %f%%, BTC: %f Coin - %f%%", 
			this.btcToCoinsRate,
			this.btcToCoinsCommission * 100,
			this.coinsToBtcRate,
			this.coinsToBtcCommission * 100);
	}
}
