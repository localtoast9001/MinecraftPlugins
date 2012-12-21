package de.shittyco.Bitcoin;

public class BTC extends Number implements Comparable<BTC> {

	/**
	 * 
	 */
	private static final long serialVersionUID = -4074238215798891733L;
	
	private long value;
	
	public BTC(long rawValue) {
		this.value = rawValue;
	}
	
	public BTC(String source) throws NumberFormatException {
		this.value = parseValue(source);
	}
	
	public BTC(float source) {
		this.value = (long)(source * 100000000f);
	}
	
	public BTC(double source) {
		this.value = (long)(source * 100000000);
	}

	public int compareTo(BTC o) {
		return this.value < o.value ?
			(this.value == o.value ? 0 : -1) :
			1;
	}
	
	@Override
	public boolean equals(Object obj) {
		if(obj == null) {
			return false;
		}
		
		if(!(obj instanceof BTC)) {
			return false;
		}
		
		BTC other = (BTC)obj;
		return this.value == other.value;
	}
	
	@Override
	public int hashCode() {
		return (int) this.value;
	}
	
	public static BTC mul(BTC operand1, float operand2) {
		return new BTC(operand1.floatValue() * operand2);
	}
	
	public static BTC sub(BTC operand1, BTC operand2) {
		return new BTC(operand1.longValue() - operand2.longValue());
	}
	
	public static BTC add(BTC operand1, BTC operand2) {
		return new BTC(operand1.longValue() + operand2.longValue());
	}

	@Override
	public double doubleValue() {
		return (double)this.value * 0.00000001;
	}

	@Override
	public float floatValue() {
		return (float)this.value * 0.00000001f;
	}

	@Override
	public int intValue() {
		return (int)this.doubleValue();
	}

	@Override
	public long longValue() {
		return this.value;
	}

	@Override
	public String toString() {
		StringBuilder sb = new StringBuilder();
		boolean minus = this.value < 0;
		long r = minus ? -this.value : this.value;
		long digit = 0;
		for(int i = 0; i < 8; i++) {
			digit = r % 10;
			r /= 10;
			sb.insert(0, (char)('0' + (char)digit));
		}
		
		sb.insert(0, '.');
		do {
			digit = r % 10;
			r /= 10;
			sb.insert(0, (char)('0' + (char)digit));
		} while(r > 0);
		
		if(minus) {
			sb.insert(0, '-');
		}
		
		return sb.toString();
	}
	
	private static long parseValue(String source) throws NumberFormatException {
		long r = 0;
		int index = 0;
		boolean valid = false;
		while(index < source.length() && Character.isWhitespace(source.charAt(index))) {
			index++;
		}
		
		boolean minus = false;
		if(index < source.length() && source.charAt(index) == '-') {
			minus = true;
			index++;
		}
		
		while(index < source.length() && Character.isDigit(source.charAt(index))) {
			long digit = source.charAt(index) - '0';
			r *= 10;
			r += digit * 100000000;
			index++;
			valid = true;
		}
		
		if(index < source.length() && source.charAt(index) == '.') {
			index++;
			long factor = 10000000;
			for(int i = 0; i < 8 && index < source.length() && 
				Character.isDigit(source.charAt(index)); i++, index++) {
					long digit = source.charAt(index) - '0';
					r += digit * factor;
					factor /= 10;
					valid = true;
			}
			
			if(index < source.length() && Character.isDigit(source.charAt(index))) {
				valid = false;
			}
		}
		
		if(!valid) {
			throw new NumberFormatException(source);
		}
		
		return minus ? -r : r;
	}
}
