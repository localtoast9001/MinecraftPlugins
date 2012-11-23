package de.shittyco.Bitcoin;

public class ServerErrorException extends Exception {
	
	/**
	 * 
	 */
	private static final long serialVersionUID = 5594170348749072463L;
	
	private int code;

	public ServerErrorException() {
		// TODO Auto-generated constructor stub
	}

	public ServerErrorException(String arg0) {
		super(arg0);
		// TODO Auto-generated constructor stub
	}

	public ServerErrorException(Throwable arg0) {
		super(arg0);
		// TODO Auto-generated constructor stub
	}

	public ServerErrorException(String arg0, Throwable arg1) {
		super(arg0, arg1);
		// TODO Auto-generated constructor stub
	}

	public ServerErrorException(String arg0, Throwable arg1, boolean arg2,
			boolean arg3) {
		super(arg0, arg1, arg2, arg3);
		// TODO Auto-generated constructor stub
	}

	public int getCode() {
		return this.code;
	}
	
	public void setCode(int value) {
		this.code = value;
	}
}
