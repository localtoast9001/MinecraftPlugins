package de.shittyco.Bitcoin;

import java.net.Authenticator;
import java.net.PasswordAuthentication;

/**
 * @author jrowlett
 *
 */
public class BitcoinAuthenticator extends Authenticator {
    /**
     * the user name.
     */
    private String user;

    /**
     * the password.
     */
    private String password;

    /**
     * @param userValue - the user name
     * @param passwordValue - the password
     */
    public BitcoinAuthenticator(
        final String userValue,
        final String passwordValue) {
        this.user = userValue;
        this.password = passwordValue;
    }

    /* (non-Javadoc)
     * @see java.net.Authenticator#getPasswordAuthentication()
     */
    @Override
    protected final PasswordAuthentication getPasswordAuthentication() {
        return new PasswordAuthentication(this.user,
                this.password.toCharArray());
    }
}
