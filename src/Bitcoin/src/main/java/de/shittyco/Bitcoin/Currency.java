package de.shittyco.Bitcoin;

/**
 * Number class that is the base class for all fixed with currency.
 * @author jrowlett
 **/
public abstract class Currency extends Number {

    /**
     * Serial Version ID (not used since abstract).
     */
    private static final long serialVersionUID = -8416960934573730723L;

    /**
     * Radix to parse.
     */
    private static final int RADIX = 10;

    /**
     * internal value storage.
     */
    private long value;

    /**
     * @param rawValue - the raw integer value
     */
    protected Currency(final long rawValue) {
        this.value = rawValue;
    }

    /**
     * @param source - the value string to parse.
     * @param precision - decimal precision of the currency.
     * @param scale - 10 ^ precision.
     */
    protected Currency(
        final String source,
        final int precision,
        final double scale) {
        this.value = parseValue(source, precision, scale);
    }

    /**
     * @param source - the decimal value of BTC.
     * @param scale - 10 ^ decimal precision of the currency.
     */
    protected Currency(
        final float source,
        final float scale) {
        this.value = (long) (source * scale);
    }

    /**
     * @param source - the decimal value of BTC.
     * @param scale - 10 ^ decimal precision or the currency.
     */
    protected Currency(
        final double source,
        final double scale) {
        this.value = (long) (source * scale);
    }

    /* (non-Javadoc)
     * @see java.lang.Number#doubleValue()
     */
    @Override
    public final double doubleValue() {
        return (double) this.value * this.getInverseScale();
    }

    /* (non-Javadoc)
     * @see java.lang.Number#floatValue()
     */
    @Override
    public final float floatValue() {
        return (float) this.value * (float) this.getInverseScale();
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
        long r = this.value;
        if (minus) {
            r = -this.value;
        }

        long digit = 0;
        for (int i = 0; i < this.getPrecision(); i++) {
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

    /* (non-Javadoc)
     * @see java.lang.Object#hashCode()
     */
    @Override
    public final int hashCode() {
        return (int) this.value;
    }

    /* (non-Javadoc)
     * @see java.lang.Object#equals(java.lang.Object)
     */
    @Override
    public final boolean equals(final Object obj) {
        if (obj == null) {
            return false;
        }

        if (!(obj instanceof Currency)) {
            return false;
        }

        if (!this.getClass().equals(obj.getClass())) {
            return false;
        }

        Currency other = (Currency) obj;
        return this.value == other.value;
    }

    /**
     * @param source value in BTC
     * @param precision number of decimal digits
     * @param scale 10 ^ precision
     * @return the parsed value for internal storage
     */
    protected static long parseValue(
        final String source,
        final int precision,
        final double scale) {
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
            r += digit * scale;
            index++;
            valid = true;
        }

        if (index < source.length() && source.charAt(index) == '.') {
            index++;
            long factor = (long) (scale / RADIX);
            for (int i = 0; i < precision && index < source.length()
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

        if (minus) {
            return -r;
        } else {
            return r;
        }
    }

    /**
     * Override to return the currency precision.
     * @return number of decimal places in precision.
     */
    protected abstract int getPrecision();

    /**
     * Override to get the scale which should be a constant 10 ^ precision.
     * @return the scale for multiplying the decimal value to get the raw value.
     */
    protected abstract double getScale();

    /**
     * Override to get the inverse of the scale value.
     * @return the inverse of the scale value.
     */
    protected abstract double getInverseScale();

    /**
     * Gets a string for the units.
     * @return a constant string for the currency units. ex. BTC, LTC, etc...
     */
    protected abstract String getUnits();

    /**
     * Call from a derived class to do the comparison for specific types.
     * @param o the other object to compare against.
     * @return the result of the comparison.
     */
    protected final int compareTo(final Currency o) {
        if (this.value < o.value) {
            if (this.value == o.value) {
                return 0;
            } else {
                return -1;
            }
        } else {
            return 1;
        }
    }
}
