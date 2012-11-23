import static org.junit.Assert.*;

import java.net.URL;

import junit.framework.Assert;

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
	
	@Test
	public void testMove() throws Exception {
		fail("Blocked by Bug in Bitcoin.");
		BitcoinClient client = new BitcoinClient(
			new URL("http://localhost:8332"), 
			"testuser",
			"P0rsche911");
		String a1Name = "BitcoinBroker";
		String a2Name = "localtoast9001";
		
		BTC amount = new BTC(0.0000001);
		BTC a1 = client.getBalance(a1Name);
		BTC a2 = client.getBalance(a2Name);
		client.move(a1Name, a2Name, amount);
		BTC ap1 = client.getBalance(a1Name);
		BTC ap2 = client.getBalance(a2Name);
		assertEquals(ap1, BTC.sub(a1, amount));
		assertEquals(ap2, BTC.add(a2, amount));
		client.move(a2Name, a1Name, amount);
		assertEquals(a1, client.getBalance(a1Name));
		assertEquals(a2, client.getBalance(a2Name));
		
		BTC alot = new BTC(1000000);
		try {
			client.move(a1Name, a2Name, alot);
			fail("Should throw.");
		} catch (Exception ex) {
			assertTrue(ex.getMessage().length() > 0);
		}
	}
	
	@Test
	public void testSendFrom() throws Exception {
		BitcoinClient client = new BitcoinClient(
				new URL("http://localhost:8332"), 
				"testuser",
				"P0rsche911");
		String a1Name = "BitcoinBroker";
		String a2Name = "localtoast9001";
		BTC amount = new BTC(0.0000001);
		BTC a1 = client.getBalance(a1Name);
		assertTrue(a1.floatValue() > amount.floatValue());
		String a2Addr = client.getAccountAddress(a2Name);
		String txId = client.sendFrom(a1Name, a2Addr, amount);
		assertNotNull(txId);
		assertTrue(txId.length() > 0);
	}
}
