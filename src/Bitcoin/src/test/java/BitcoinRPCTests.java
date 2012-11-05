import static org.junit.Assert.*;

import java.net.URL;

import org.junit.Test;
import de.shittyco.Bitcoin.*;

public class BitcoinRPCTests {

	@Test
	public void testGetInfo() throws Exception {
		BitcoinClient client = new BitcoinClient(
			new URL("http://localhost:8332"), 
			"testuser",
			"P0rsche911");
		BitcoinInfo info = client.getInfo();
		assertNotNull(info);
	}

	@Test
	public void testGetBalance() throws Exception {
		BitcoinClient client = new BitcoinClient(
			new URL("http://localhost:8332"), 
			"testuser",
			"P0rsche911");
		BTC balance = client.getBalance(null);
		assertNotNull(balance);
		balance = client.getBalance("localtoast9001");
		assertNotNull(balance);
	}
	
	@Test
	public void testGetAccountAddress() throws Exception {
		BitcoinClient client = new BitcoinClient(
			new URL("http://localhost:8332"), 
			"testuser",
			"P0rsche911");
		String address = client.getAccountAddress("localtoast9001");
		assertNotNull(address);
		assertTrue(address.length() == 34);
		String otherAddress = client.getAccountAddress("BitcoinBroker");
		assertNotNull(otherAddress);
		assertTrue(address.length() == 34);
		assertTrue(!address.equalsIgnoreCase(otherAddress));
	}
}
