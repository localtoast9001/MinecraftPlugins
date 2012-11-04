package de.shittyco.Bitcoin;
/**
 * 
 */
import java.io.*;
import java.net.*;

import org.codehaus.jackson.*;

/**
 * @author jrowlett
 * 
 */
public class BitcoinClient {
	private BitcoinAuthenticator auth;
	private URL url;
	private JsonFactory factory = new JsonFactory();
	
	public BitcoinClient(URL url, String userName, String password) {
		this.auth = new BitcoinAuthenticator(userName, password);
		this.url = url;
	}
	
	public BitcoinInfo getInfo() throws IOException {
		JsonParser parser = this.call("getinfo", new Object[] { });
		return parseBitcoinInfo(parser);
	}
	
	private static BitcoinInfo parseBitcoinInfo(JsonParser parser) throws JsonParseException, IOException {
		BitcoinInfo result = new BitcoinInfo();
		parser.nextToken();
		while (parser.nextToken() != JsonToken.END_OBJECT) {
			String fieldName = parser.getCurrentName();
			parser.nextToken();
			if (fieldName.equalsIgnoreCase("version")) {
				result.setVersion(parser.getIntValue());
			} else if (fieldName.equalsIgnoreCase("protocolversion")) {
				result.setProtocolVersion(parser.getIntValue());
			} else if (fieldName.equalsIgnoreCase("walletversion")) {
				result.setWalletVersion(parser.getIntValue());
			} else if (fieldName.equalsIgnoreCase("balance")) {
				result.setBalance(new BTC(parser.getFloatValue()));
			} else if (fieldName.equalsIgnoreCase("blocks")) {
				result.setBlocks(parser.getIntValue());
			} else if (fieldName.equalsIgnoreCase("connections")) {
				result.setConnections(parser.getIntValue());
			} else if (fieldName.equalsIgnoreCase("proxy")) {
				result.setProxy(parser.getText());
			} else if (fieldName.equalsIgnoreCase("difficulty")) {
				result.setDifficulty(parser.getDoubleValue());
			} else if (fieldName.equalsIgnoreCase("testnet")) {
				result.setTestnet(parser.getBooleanValue());
			} else if (fieldName.equalsIgnoreCase("keypoololdest")) {
				result.setKeyPoolOldest(parser.getIntValue());
			} else if (fieldName.equalsIgnoreCase("keypoolsize")) {
				result.setKeyPoolSize(parser.getIntValue());
			} else if (fieldName.equalsIgnoreCase("paytxfee")) {
				result.setPayTxFee(new BTC(parser.getFloatValue()));
			} else if (fieldName.equalsIgnoreCase("errors")) {
				result.setErrors(parser.getText());
			}
		}
		
		return result;
	}
	
	private JsonParser call(String method, Object args[]) throws IOException {
		Authenticator.setDefault(this.auth);

		HttpURLConnection connection = (HttpURLConnection) this.url.openConnection();
	    connection.setRequestMethod("POST");
	    connection.addRequestProperty("ContentType", "application/json-rpc");
	    connection.setDoOutput(true);
	    OutputStream requestStream = connection.getOutputStream();
	    JsonGenerator generator = this.factory.createJsonGenerator(requestStream);
	    generator.writeStartObject();
	    generator.writeStringField("jsonrpc", "1.0");
	    generator.writeStringField("id", "1");
	    generator.writeStringField("method", method);
	    generator.writeArrayFieldStart("params");
	    for(int i=args.length - 1; i >= 0; i--) {
	    	generator.writeObject(args[i]);
	    }
	    
	    generator.writeEndArray();
	    generator.writeEndObject();
	    generator.close();

	    
	    return this.factory.createJsonParser(connection.getInputStream());
	}
}