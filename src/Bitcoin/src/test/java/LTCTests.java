import static org.junit.Assert.assertEquals;

import org.junit.Test;
import de.shittyco.Bitcoin.LTC;

/**
 * Tests the LTC class.
 **/
public class LTCTests {

    /**
     * smallest LTC increment.
     */
    private static final float SMALLEST_LTC = 0.00000001f;

    /**
     * smallest LTC increment as a double.
     */
    private static final double SMALLEST_LTCD = 0.00000001;

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
            LTC target = new LTC(testCase);
            float actual = target.floatValue();
            assertEquals(expected, actual, 0f);
        }

        assertEquals(1, new LTC(SMALLEST_LTC).longValue());
    }

    /**
     * Tests doubleValue() functionality.
     */
    @Test
    public final void testDouble() {

        for (int i = 0; i < DOUBLE_TEST_CASES.length; i++) {
            double testCase = DOUBLE_TEST_CASES[i][0];
            double expected = DOUBLE_TEST_CASES[i][1];
            LTC target = new LTC(testCase);
            double actual = target.doubleValue();
            assertEquals(expected, actual, 0);
        }

        assertEquals(1, new LTC(SMALLEST_LTCD).longValue());
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
            LTC target = new LTC(testCase);
            assertEquals(expected, target.longValue());
            String formatted = target.toString();
            LTC reparsed = new LTC(formatted);
            assertEquals(expected, reparsed.longValue());
        }
    }

    /**
     * Tests the LTC.add functionality.
     */
    @Test
    public final void testAdd() {
        LTC a = new LTC("0.0000001");
        LTC b = new LTC("0.00899980");

        assertEquals(new LTC("0.00899990"), LTC.add(a, b));
    }

    /**
     * Tests the LTC.sub functionality.
     */
    @Test
    public final void testSub() {
        LTC a = new LTC("0.0000001");
        LTC b = new LTC("0.00899980");

        assertEquals(new LTC("0.00899970"), LTC.sub(b, a));
    }

    /**
     * Tests the LTC.mul functionality.
     */
    @Test
    public final void testMul() {
        LTC a = new LTC("0.01");
        final double b = 50000.0;
        assertEquals(new LTC("500"), LTC.mul(a, b));
    }
}
