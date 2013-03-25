import static org.junit.Assert.*;

import java.net.URL;

import org.junit.Test;
import de.shittyco.Bitcoin.BTC;
import de.shittyco.Bitcoin.BitcoinClient;
import de.shittyco.Bitcoin.BitcoinInfo;

/**
 * Tests basic BitcoinClient functionality.
 * @author jrowlett
 *
 */
public class BitcoinRPCTests {

    /**
     * A small amount.
     */
    private static final BTC SMALL_AMOUNT = new BTC(0.0000001);

    /**
     * A large amount.
     */
    private static final BTC LARGE_AMOUNT = new BTC(1000000);

    /**
     * Bitcoin address length.
     */
    private static final int ADDRESS_LENGTH = 34;

    /**
     * Tests GetInfo.
     * @throws Exception - communication error.
     */
    @Test
    public final void testGetInfo() throws Exception {
        BitcoinClient client = new BitcoinClient(new URL(
                "http://localhost:8332"), "testuser", "P0rsche911");
        BitcoinInfo info = client.getInfo();
        assertNotNull(info);
    }

    /**
     * Tests getBalance functionality.
     * @throws Exception communication error.
     */
    @Test
    public final void testGetBalance() throws Exception {
        BitcoinClient client = new BitcoinClient(new URL(
                "http://localhost:8332"), "testuser", "P0rsche911");
        BTC balance = client.getBalance(null);
        assertNotNull(balance);
        balance = client.getBalance("localtoast9001");
        assertNotNull(balance);
    }

    /**
     * Tests getAccountAddress functionality.
     * @throws Exception communication error.
     */
    @Test
    public final void testGetAccountAddress() throws Exception {
        BitcoinClient client = new BitcoinClient(new URL(
                "http://localhost:8332"), "testuser", "P0rsche911");
        String address = client.getAccountAddress("localtoast9001");
        assertNotNull(address);
        assertTrue(address.length() == ADDRESS_LENGTH);
        String otherAddress = client.getAccountAddress("BitcoinBroker");
        assertNotNull(otherAddress);
        assertTrue(address.length() == ADDRESS_LENGTH);
        assertTrue(!address.equalsIgnoreCase(otherAddress));
    }

    /**
     * Tests move functionality.
     * @throws Exception communication error.
     */
    @Test
    public final void testMove() throws Exception {
        BitcoinClient client = new BitcoinClient(new URL(
                "http://localhost:8332"), "testuser", "P0rsche911");
        String a1Name = "BitcoinBroker";
        String a2Name = "localtoast9001";

        BTC amount = SMALL_AMOUNT;
        BTC a1 = client.getBalance(a1Name);
        BTC a2 = client.getBalance(a2Name);
        client.move(a1Name, a2Name, amount);
        BTC ap1 = client.getBalance(a1Name);
        BTC ap2 = client.getBalance(a2Name);
        assertEquals(BTC.sub(a1, amount), ap1);
        assertEquals(BTC.add(a2, amount), ap2);
        client.move(a2Name, a1Name, amount);
        assertEquals(a1, client.getBalance(a1Name));
        assertEquals(a2, client.getBalance(a2Name));

        BTC alot = LARGE_AMOUNT;
        try {
            client.move(a1Name, a2Name, alot);
            fail("Should throw.");
        } catch (Exception ex) {
            assertTrue(ex.getMessage().length() > 0);
        }
    }

    /**
     * Tests sendFrom functionality.
     * @throws Exception - communication error with Bitcoin.
     */
    @Test
    public final void testSendFrom() throws Exception {
        BitcoinClient client = new BitcoinClient(new URL(
                "http://localhost:8332"), "testuser", "P0rsche911");
        String a1Name = "BitcoinBroker";
        String a2Name = "localtoast9001";
        BTC amount = SMALL_AMOUNT;
        BTC a1 = client.getBalance(a1Name);
        assertTrue(a1.floatValue() > amount.floatValue());
        String a2Addr = client.getAccountAddress(a2Name);
        String txId = client.sendFrom(a1Name, a2Addr, amount);
        assertNotNull(txId);
        assertTrue(txId.length() > 0);
    }
}
