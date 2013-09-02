package de.shittyco.Bitcoin;

/**
 * Litecoin currency.
 * @author jrowlett
 *
 */
public final class LTC extends Currency implements Comparable<LTC> {

    /**
     * Serial Version ID.
     */
    private static final long serialVersionUID = -1523099474167857598L;

    /**
     * Precision of LTC values.
     */
    private static final int PRECISION = 8;

    /**
     * Scale to represent LTC fixed precision values as integers.
     */
    private static final long SCALE = 100000000;

    /**
     * The inverse of SCALE.
     */
    private static final double INVERSE_SCALE = 0.00000001;

    /**
     * Name of the units of this currency.
     */
    private static final String UNITS = "LTC";

    /**
     * Initializes a new instance of the LTC class.
     * @param rawValue the raw integer value.
     */
    public LTC(final long rawValue) {
        super(rawValue);
    }

    /**
     * Initializes a new instance of the LTC class.
     * @param value the decimal value.
     */
    public LTC(final float value) {
        super(value, SCALE);
    }

    /**
     * Initializes a new instance of the LTC class.
     * @param value the decimal value.
     */
    public LTC(final double value) {
        super(value, SCALE);
    }

    /**
     * Initializes a new instance of the LTC class.
     * @param value the string value.
     */
    public LTC(final String value) {
        super(value, PRECISION, SCALE);
    }

    @Override
    public int compareTo(final LTC o) {
        return super.compareTo(o);
    }

    /**
     * @param operand1 - first operand
     * @param operand2 - second operand
     * @return the product of the operands in a new object.
     */
    public static LTC mul(final LTC operand1, final double operand2) {
        return new LTC(operand1.doubleValue() * operand2);
    }

    /**
     * @param operand1 - first operand
     * @param operand2 - second operand
     * @return the result of subtracting the second operand from the first
     * in a new object
     */
    public static LTC sub(final LTC operand1, final LTC operand2) {
        return new LTC(operand1.longValue() - operand2.longValue());
    }

    /**
     * @param operand1 - first operand
     * @param operand2 - second operand
     * @return the result of adding the first and second operands
     * in a new object
     */
    public static LTC add(final LTC operand1, final LTC operand2) {
        return new LTC(operand1.longValue() + operand2.longValue());
    }

    @Override
    protected int getPrecision() {
        return PRECISION;
    }

    @Override
    protected double getScale() {
        return SCALE;
    }

    @Override
    protected double getInverseScale() {
        return INVERSE_SCALE;
    }

    @Override
    protected String getUnits() {
        return UNITS;
    }
}
