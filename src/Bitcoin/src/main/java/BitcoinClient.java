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
	
	public String getInfo() throws IOException {
		JsonParser parser = this.call("getinfo", new Object[] { });
		return parser.toString();
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