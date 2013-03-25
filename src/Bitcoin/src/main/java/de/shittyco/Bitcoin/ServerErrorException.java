package de.shittyco.Bitcoin;

/**
 * Custom error to throw to indicate an error with processing Bitcoin
 * transactions.
 * @author jrowlett
 */
public class ServerErrorException extends Exception {

    /**
     * Serialization id.
     */
    private static final long serialVersionUID = 5594170348749072463L;

    /**
     * error code.
     */
    private int code;

    /**
     * Initializes a new instance of the ServerErrorException class.
     */
    public ServerErrorException() {
    }

    /**
     * Initializes a new instance of the ServerErrorException class.
     * @param arg0 the message.
     */
    public ServerErrorException(final String arg0) {
        super(arg0);
        // TODO Auto-generated constructor stub
    }

    /**
     * Initializes a new instance of the ServerErrorException class.
     * @param arg0 the copy
     */
    public ServerErrorException(final Throwable arg0) {
        super(arg0);
        // TODO Auto-generated constructor stub
    }

    /**
     * Initializes a new instance of the ServerErrorException class.
     * @param arg0 message
     * @param arg1 inner error
     */
    public ServerErrorException(final String arg0, final Throwable arg1) {
        super(arg0, arg1);
        // TODO Auto-generated constructor stub
    }

    /**
     * @return the error code.
     */
    public final int getCode() {
        return this.code;
    }

    /**
     * @param value - the error code.
     */
    public final void setCode(final int value) {
        this.code = value;
    }
}
