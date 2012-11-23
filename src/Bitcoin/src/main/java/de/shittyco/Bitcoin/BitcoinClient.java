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
	
	public BitcoinInfo getInfo() throws ServerErrorException {
		JsonParser parser = this.call("getinfo", new Object[] { });
		return parseBitcoinInfo(parser);
	}
	
	public BTC getBalance(String account) throws ServerErrorException {
		Object args[] = account == null || account.isEmpty() ?
			new Object[0] :
			new Object[] { account };
		JsonParser parser = this.call("getbalance", args);
		return parseBalance(parser);
	}
	
	public String getAccountAddress(String account) throws ServerErrorException {
		JsonParser parser = this.call("getaccountaddress", new Object[] { account });
		return parseAddress(parser);
	}
	
	public void move(String fromAccount, String toAccount, BTC amount) throws ServerErrorException {
		Object args[] = new Object[] {
			fromAccount,
			toAccount,
			amount.longValue()
		};
		JsonParser parser = this.call("move", args);
		try {
			parser.close();
		} catch(IOException ex) {
			throw new ServerErrorException(ex);
		}
	}
	
	public String sendFrom(
		String fromAccount, 
		String toAddress, 
		BTC amount) throws ServerErrorException {
		Object args[] = new Object[] {
			fromAccount,
			toAddress,
			amount.floatValue()
		};
		JsonParser parser = this.call("sendfrom", args);
		return parseTransactionID(parser);
	}
	
	private static String parseTransactionID(JsonParser parser) throws ServerErrorException {
		try {
			parser.nextToken();
			while (parser.nextToken() != JsonToken.END_OBJECT) {
				String fieldName = parser.getCurrentName();
				parser.nextToken();
				if (fieldName.equalsIgnoreCase("result")) {
					return parser.getText();
				}
			}
		} catch (Exception ex) {
			throw new ServerErrorException(ex);
		}
		
		return null;
	}
	
	private static BTC parseBalance(JsonParser parser) throws ServerErrorException {
		try {
			parser.nextToken();
			while (parser.nextToken() != JsonToken.END_OBJECT) {
				String fieldName = parser.getCurrentName();
				parser.nextToken();
				if (fieldName.equalsIgnoreCase("result")) {
					return new BTC(parser.getFloatValue());
				}
			}
		} catch (Exception ex) {
			throw new ServerErrorException(ex);
		}
		
		return null;
	}
	
	private static String parseAddress(JsonParser parser) throws ServerErrorException {
		try {
			parser.nextToken();
			while (parser.nextToken() != JsonToken.END_OBJECT) {
				String fieldName = parser.getCurrentName();
				parser.nextToken();
				if (fieldName.equalsIgnoreCase("result")) {
					return parser.getText();
				}
			}
		} catch (Exception ex) {
			throw new ServerErrorException(ex);
		}
		
		return null;
	}
	
	private static BitcoinInfo parseBitcoinInfo(JsonParser parser) throws ServerErrorException {
		try {
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
		} catch (Exception ex) {
			throw new ServerErrorException(ex);
		}
	}
	
	private static ServerErrorException parseError(
		JsonParser parser, 
		Exception inner) throws JsonParseException, IOException {
		String message = "";
		int code = 0;
		
		parser.nextToken();
		while (parser.nextToken() != JsonToken.END_OBJECT) {
			String fieldName = parser.getCurrentName();
			parser.nextToken();
			if (fieldName.equalsIgnoreCase("error")) {
				while (parser.nextToken() != JsonToken.END_OBJECT) {
					String innerFieldName = parser.getCurrentName();
					parser.nextToken();
					if (innerFieldName.equalsIgnoreCase("code")) {
						code = parser.getIntValue();
					} else if (innerFieldName.equalsIgnoreCase("message")) {
						message = parser.getText();
					}
				}
			}
		}
		
		ServerErrorException error = new ServerErrorException(message, inner);
		error.setCode(code);
		return error;
	}
	
	private JsonParser call(String method, Object args[]) throws ServerErrorException {
		Authenticator.setDefault(this.auth);

		try {
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
		    for(int i = 0; i < args.length; i++) {
		    	generator.writeObject(args[i]);
		    }
		    
		    generator.writeEndArray();
		    generator.writeEndObject();
		    generator.close();
	    
		    try {
		    	return this.factory.createJsonParser(connection.getInputStream());
		    } catch (IOException ex) {
		    	if (!connection.getContentType().contains("json")) {
		    		throw new ServerErrorException(ex);
		    	}
	    	
	    		JsonParser errorParser = this.factory.createJsonParser(connection.getErrorStream());
	    		throw parseError(errorParser, ex);
		    }
    	} catch (IOException exex) {
    		throw new ServerErrorException(exex);
    	}
	}
}