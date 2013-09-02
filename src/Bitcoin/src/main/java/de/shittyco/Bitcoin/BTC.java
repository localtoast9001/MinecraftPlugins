package de.shittyco.Bitcoin;

/**
 * Number class that represents the fixed-width currency value used by Bitcoin.
 * @author jrowlett
 *
 */
public class BTC extends Currency implements Comparable<BTC> {

    /**
     * Version UID for serialization.
     */
    private static final long serialVersionUID = -4074238215798891733L;

    /**
     * Precision of BTC values.
     */
    private static final int PRECISION = 8;

    /**
     * Scale to represent BTC fixed precision values as integers.
     */
    private static final long SCALE = 100000000;

    /**
     * The inverse of SCALE.
     */
    private static final double INVERSE_SCALE = 0.00000001;

    /**
     * Name of the units of this currency.
     */
    private static final String UNITS = "BTC";

    /**
     * @param rawValue - the raw integer value
     */
    public BTC(final long rawValue) {
        super(rawValue);
    }

    /**
     * @param source - the value string to parse.
     */
    public BTC(final String source) {
        super(source, PRECISION, SCALE);
    }

    /**
     * @param source - the decimal value of BTC.
     */
    public BTC(final float source) {
        super(source, SCALE);
    }

    /**
     * @param source - the decimal value of BTC.
     */
    public BTC(final double source) {
        super(source, SCALE);
    }

    /* (non-Javadoc)
     * @see java.lang.Comparable#compareTo(java.lang.Object)
     */
    @Override
    public final int compareTo(final BTC o) {
        return super.compareTo(o);
    }

    /**
     * @param operand1 - first operand
     * @param operand2 - second operand
     * @return the product of the operands in a new object.
     */
    public static BTC mul(final BTC operand1, final float operand2) {
        return new BTC(operand1.floatValue() * operand2);
    }

    /**
     * @param operand1 - first operand
     * @param operand2 - second operand
     * @return the result of subtracting the second operand from the first
     * in a new object
     */
    public static BTC sub(final BTC operand1, final BTC operand2) {
        return new BTC(operand1.longValue() - operand2.longValue());
    }

    /**
     * @param operand1 - first operand
     * @param operand2 - second operand
     * @return the result of adding the first and second operands
     * in a new object
     */
    public static BTC add(final BTC operand1, final BTC operand2) {
        return new BTC(operand1.longValue() + operand2.longValue());
    }

    @Override
    protected final int getPrecision() {
        return PRECISION;
    }

    @Override
    protected final double getScale() {
        return SCALE;
    }

    @Override
    protected final double getInverseScale() {
        return INVERSE_SCALE;
    }

    @Override
    protected final String getUnits() {
        return UNITS;
    }
}
