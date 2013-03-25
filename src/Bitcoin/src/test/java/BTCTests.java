import static org.junit.Assert.*;

import org.junit.Test;
import de.shittyco.Bitcoin.BTC;

/**
 * Tests the BTC class.
 **/
public class BTCTests {

    /**
     * smallest BTC increment.
     */
    private static final float SMALLEST_BTC = 0.00000001f;

    /**
     * Test cases for float conversion, actual and expected pairs.
     */
    private static final float[][] FLOAT_TEST_CASES =
        new float[][] {new float[] {1, 1},
        new float[] {0.99f, 0.99f},
        new float[] {0.00000001f, 0.00000001f}, new float[] {0, 0},
        new float[] {0.000000001f, 0.00000000f}};

    /**
     * Test cases for double conversion, actual and expected pairs.
     */
    private static final double[][] DOUBLE_TEST_CASES =
        new double[][] {new double[] {1, 1 },
            new double[] {0.99, 0.99 },
            new double[] {0.00000001, 0.00000001 }, new double[] {0, 0 },
            new double[] {0.000000001, 0.00000000 } };

    /**
     * Expected values inside the string test.
     */
    private static final long[] EXPECTED_PARSED_VALUES =
        new long[] {1, 2, 99, -1, 100000000, 0, 0, 0, };

    /**
     * Tests floatValue() functionality.
     */
    @Test
    public final void testFloat() {
        for (int i = 0; i < FLOAT_TEST_CASES.length; i++) {
            float testCase = FLOAT_TEST_CASES[i][0];
            float expected = FLOAT_TEST_CASES[i][1];
            BTC target = new BTC(testCase);
            float actual = target.floatValue();
            assertEquals(expected, actual, 0f);
        }

        assertEquals(1, new BTC(SMALLEST_BTC).longValue());
    }

    /**
     * Tests doubleValue() functionality.
     */
    @Test
    public final void testDouble() {

        for (int i = 0; i < DOUBLE_TEST_CASES.length; i++) {
            double testCase = DOUBLE_TEST_CASES[i][0];
            double expected = DOUBLE_TEST_CASES[i][1];
            BTC target = new BTC(testCase);
            double actual = target.doubleValue();
            assertEquals(expected, actual, 0);
        }

        assertEquals(1, new BTC((double) SMALLEST_BTC).longValue());
    }

    /**
     * Tests toString() functionality.
     */
    @Test
    public final void testString() {
        String[] testCases = new String[] {".00000001", "0.00000002",
                "0.00000099", " -.00000001", "1.00", "0", "0.0", "-0" };

        for (int i = 0; i < testCases.length; i++) {
            String testCase = testCases[i];
            long expected = EXPECTED_PARSED_VALUES[i];
            BTC target = new BTC(testCase);
            assertEquals(expected, target.longValue());
            String formatted = target.toString();
            BTC reparsed = new BTC(formatted);
            assertEquals(expected, reparsed.longValue());
        }
    }

    /**
     * Tests the BTC.add functionality.
     */
    @Test
    public final void testAdd() {
        BTC a = new BTC("0.0000001");
        BTC b = new BTC("0.00899980");

        assertEquals(new BTC("0.00899990"), BTC.add(a, b));
    }

    /**
     * Tests the BTC.sub functionality.
     */
    @Test
    public final void testSub() {
        BTC a = new BTC("0.0000001");
        BTC b = new BTC("0.00899980");

        assertEquals(new BTC("0.00899970"), BTC.sub(b, a));
    }
}
