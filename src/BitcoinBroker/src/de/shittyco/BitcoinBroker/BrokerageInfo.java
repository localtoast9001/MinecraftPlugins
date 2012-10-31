/**
 * 
 */
package de.shittyco.BitcoinBroker;

/**
 * @author Jon Rowlett
 *
 */
public class BrokerageInfo {
	private float btcToCoinsRate = 0.000001f;
	private float coinsToBtcRate = 1000000f;
	private float btcToCoinsCommission = 0.001f;
	private float coinsToBtcCommission = 0.001f;
	private String profitAddress;
	
	public float getBtcToCoinsRate() {
		return this.btcToCoinsRate;
	}
	
	public void setBtcToCoinsRate(float value) {
		this.btcToCoinsRate = value;
	}
	
	public float getCoinsToBtcRate() {
		return this.coinsToBtcRate;
	}
	
	public void setCoinsToBtcRate(float value) { 
		this.coinsToBtcRate = value;
	}
	
	public float getBtcToCoinsCommission() { 
		return this.btcToCoinsCommission;
	}
	
	public void setBtcToCoinsCommission(float value) {
		this.btcToCoinsCommission = value;
	}
	
	public float getCoinsToBtcCommission() {
		return this.coinsToBtcCommission;
	}
	
	public void setCoinsToBtcCommission(float value) {
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
