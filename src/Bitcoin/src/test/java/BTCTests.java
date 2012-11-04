import static org.junit.Assert.*;

import org.junit.Test;
import de.shittyco.Bitcoin.*;

public class BTCTests {

	@Test
	public void testFloat() {
		float testCases[][] = new float[][] {
			new float[] { 1, 1 },
			new float[] { 0.99f, 0.99f },
			new float[] { 0.00000001f, 0.00000001f },
			new float[] { 0, 0 },
			new float[] { 0.000000001f, 0.00000000f }
		};
		
		for(int i = 0; i < testCases.length; i++) {
			float testCase = testCases[i][0];
			float expected = testCases[i][1];
			BTC target = new BTC(testCase);
			float actual = target.floatValue();
			assertEquals(expected, actual, 0f);
		}
		
		assertEquals(1, new BTC(0.00000001f).longValue());
	}

	@Test
	public void testDouble() {
		double testCases[][] = new double[][] {
				new double[] { 1, 1 },
				new double[] { 0.99, 0.99 },
				new double[] { 0.00000001, 0.00000001 },
				new double[] { 0, 0 },
				new double[] { 0.000000001, 0.00000000 }
			};
			
			for(int i = 0; i < testCases.length; i++) {
				double testCase = testCases[i][0];
				double expected = testCases[i][1];
				BTC target = new BTC(testCase);
				double actual = target.doubleValue();
				assertEquals(expected, actual, 0);
			}		

		assertEquals(1, new BTC(0.00000001).longValue());
	}
	
	@Test
	public void testString() {
		long expecteds[] = new long[] {
			1,
			2,
			99,
			-1,
			100000000,
			0,
			0,
			0,
		};
		
		String testCases[] = new String[] {
			".00000001",
			"0.00000002",
			"0.00000099",
			" -.00000001",
			"1.00",
			"0",
			"0.0",
			"-0"
		};
		
		for(int i=0; i < testCases.length; i++) {
			String testCase = testCases[i];
			long expected = expecteds[i];
			BTC target = new BTC(testCase);
			assertEquals(expected, target.longValue());
			String formatted = target.toString();
			BTC reparsed = new BTC(formatted);
			assertEquals(expected, reparsed.longValue());
		}
	}
}
