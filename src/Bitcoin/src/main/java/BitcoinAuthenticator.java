import java.net.Authenticator;
import java.net.PasswordAuthentication;


public class BitcoinAuthenticator extends Authenticator {
	private String user;
	private String password;
	
	public BitcoinAuthenticator(String user, String password) {
		this.user = user;
		this.password = password;
	}
	
	protected PasswordAuthentication getPasswordAuthentication() {
		return new PasswordAuthentication(this.user, this.password.toCharArray());
	}
}
