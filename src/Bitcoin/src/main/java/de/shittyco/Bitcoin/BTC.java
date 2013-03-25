package de.shittyco.Bitcoin;

/**
 * Number class that represents the fixed-width currency value used by Bitcoin.
 * @author jrowlett
 *
 */
public class BTC extends Number implements Comparable<BTC> {

    /**
     * Version UID for serialization.
     */
    private static final long serialVersionUID = -4074238215798891733L;

    /**
     * Radix to parse.
     */
    private static final int RADIX = 10;

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
     * internal value storage.
     */
    private long value;

    /**
     * @param rawValue - the raw integer value
     */
    public BTC(final long rawValue) {
        this.value = rawValue;
    }

    /**
     * @param source - the value string to parse.
     */
    public BTC(final String source) {
        this.value = parseValue(source);
    }

    /**
     * @param source - the decimal value of BTC.
     */
    public BTC(final float source) {
        this.value = (long) (source * (float) SCALE);
    }

    /**
     * @param source - the decimal value of BTC.
     */
    public BTC(final double source) {
        this.value = (long) (source * SCALE);
    }

    /* (non-Javadoc)
     * @see java.lang.Comparable#compareTo(java.lang.Object)
     */
    @Override
    public final int compareTo(final BTC o) {
        return this.value < o.value ? (this.value == o.value ? 0 : -1) : 1;
    }

    /* (non-Javadoc)
     * @see java.lang.Object#equals(java.lang.Object)
     */
    @Override
    public final boolean equals(final Object obj) {
        if (obj == null) {
            return false;
        }

        if (!(obj instanceof BTC)) {
            return false;
        }

        BTC other = (BTC) obj;
        return this.value == other.value;
    }

    /* (non-Javadoc)
     * @see java.lang.Object#hashCode()
     */
    @Override
    public final int hashCode() {
        return (int) this.value;
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

    /* (non-Javadoc)
     * @see java.lang.Number#doubleValue()
     */
    @Override
    public final double doubleValue() {
        return (double) this.value * INVERSE_SCALE;
    }

    /* (non-Javadoc)
     * @see java.lang.Number#floatValue()
     */
    @Override
    public final float floatValue() {
        return (float) this.value * (float) INVERSE_SCALE;
    }

    /* (non-Javadoc)
     * @see java.lang.Number#intValue()
     */
    @Override
    public final int intValue() {
        return (int) this.doubleValue();
    }

    /* (non-Javadoc)
     * @see java.lang.Number#longValue()
     */
    @Override
    public final long longValue() {
        return this.value;
    }

    /* (non-Javadoc)
     * @see java.lang.Object#toString()
     */
    @Override
    public final String toString() {
        StringBuilder sb = new StringBuilder();
        boolean minus = this.value < 0;
        long r = minus ? -this.value : this.value;
        long digit = 0;
        for (int i = 0; i < PRECISION; i++) {
            digit = r % RADIX;
            r /= RADIX;
            sb.insert(0, (char) ('0' + (char) digit));
        }

        sb.insert(0, '.');
        do {
            digit = r % RADIX;
            r /= RADIX;
            sb.insert(0, (char) ('0' + (char) digit));
        } while (r > 0);

        if (minus) {
            sb.insert(0, '-');
        }

        return sb.toString();
    }

    /**
     * @param source value in BTC
     * @return the parsed value for internal storage
     */
    private static long parseValue(final String source) {
        long r = 0;
        int index = 0;
        boolean valid = false;
        while (index < source.length()
                && Character.isWhitespace(source.charAt(index))) {
            index++;
        }

        boolean minus = false;
        if (index < source.length() && source.charAt(index) == '-') {
            minus = true;
            index++;
        }

        while (index < source.length()
                && Character.isDigit(source.charAt(index))) {
            long digit = source.charAt(index) - '0';
            r *= RADIX;
            r += digit * SCALE;
            index++;
            valid = true;
        }

        if (index < source.length() && source.charAt(index) == '.') {
            index++;
            long factor = SCALE;
            for (int i = 0; i < PRECISION && index < source.length()
                    && Character.isDigit(source.charAt(index)); i++, index++) {
                long digit = source.charAt(index) - '0';
                r += digit * factor;
                factor /= RADIX;
                valid = true;
            }

            if (index < source.length()
                    && Character.isDigit(source.charAt(index))) {
                valid = false;
            }
        }

        if (!valid) {
            throw new NumberFormatException(source);
        }

        return minus ? -r : r;
    }
}
