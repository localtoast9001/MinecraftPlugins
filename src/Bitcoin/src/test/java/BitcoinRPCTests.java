import static org.junit.Assert.*;

import java.net.URL;

import org.junit.Test;


public class BitcoinRPCTests {

	@Test
	public void testGetInfo() throws Exception {
		BitcoinClient client = new BitcoinClient(
			new URL("http://localhost:8332"), 
			"testuser",
			"P0rsche911");
		String result = client.getInfo();
		fail(result);
	}

}
