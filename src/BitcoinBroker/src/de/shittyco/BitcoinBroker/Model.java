/**
 * @author Jon Rowlett
 */
package de.shittyco.BitcoinBroker;

import java.io.IOException;
import java.net.*;

/**
 * @author Jon Rowlett
 * 
 */
public class Model {
	private BrokerageInfo brokerageInfo = new BrokerageInfo();
	private URL bitcoinUrl;
	private String user;
	private String password;
	private String account;
	private HttpURLConnection connection;
	
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
	    this.user = user;
	    this.password = password;
	    Authenticator.setDefault(
	    	new BitcoinAuthenticator(this.user, this.password));
	    
	    try {
	    	this.connection = (HttpURLConnection) this.bitcoinUrl.openConnection();
	    	this.connection.setRequestMethod("POST");
	    	this.connection.getInputStream();
	    	return "TODO";
	    } catch(IOException ex) {
	    	return "Unable to connect.";
	    }
	}
	
	public float getBrokerageBalance() {
		return 0.0f;
	}
}
