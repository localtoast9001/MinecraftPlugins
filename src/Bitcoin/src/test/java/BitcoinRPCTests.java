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

}
