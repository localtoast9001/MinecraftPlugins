/**
 * @author Jon Rowlett
 */
package de.shittyco.BitcoinBroker;

import java.io.IOException;
import java.net.*;
import de.shittyco.Bitcoin.*;

/**
 * @author Jon Rowlett
 * 
 */
public class Model {
	private BrokerageInfo brokerageInfo = new BrokerageInfo();
	private URL bitcoinUrl;
	private String account;
	private BitcoinClient client;
	
	public BrokerageInfo getBrokerageInfo() {
		return this.brokerageInfo;
	}
	
	public String getBitcoinUrl() {
		return this.bitcoinUrl.toString();
	}
	
	public void setBitcoinUrl(String value) throws MalformedURLException {
		this.bitcoinUrl = new URL(value);
	}
	
	public void setAccount(String value) {
		this.account = value;
	}
	
	public String getAccount() {
		return this.account;
	}
	
	public String init(String user, String password) {
		this.client = new BitcoinClient(
			this.bitcoinUrl,
			user,
			password);
		try {
			BitcoinInfo info = this.client.getInfo();
			return String.format(
				"Connections=%d, Total Server Balance=%s", 
				info.getConnections(),
				info.getBalance().toString());
		} catch (IOException e) {
			return e.toString();
		}
	}
	
	public float getBrokerageBalance() {
		return 0.0f;
	}
}
