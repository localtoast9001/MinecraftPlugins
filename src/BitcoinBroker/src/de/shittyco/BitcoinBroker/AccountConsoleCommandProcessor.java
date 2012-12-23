/**
 * 
 */
package de.shittyco.BitcoinBroker;

import org.bukkit.OfflinePlayer;
import org.bukkit.Server;
import org.bukkit.command.CommandSender;

/**
 * @author jrowlett
 *
 */
public class AccountConsoleCommandProcessor extends ConsoleCommandProcessor {

	private Server server;
	
	public AccountConsoleCommandProcessor(Model model, Server server) {
		super("account", model);
		this.server = server;
	}
	
	/* (non-Javadoc)
	 * @see de.shittyco.BitcoinBroker.ConsoleCommandProcessor#onCommand(org.bukkit.command.CommandSender, java.lang.String[])
	 */
	@Override
	public Boolean onCommand(CommandSender sender, String[] args) {
		if (args.length != 1) {
			return false;
		}
		
		String playerName = args[0];
		OfflinePlayer player = this.server.getOfflinePlayer(playerName);
		
		try {
			PlayerAccountInfo accountInfo = this.getModel().getAccountInfo(player);
			sender.sendMessage(accountInfo.toString());
			TransactionLogPrinter.print(sender, accountInfo.getLatestTransactions());
		} catch (Exception e) {
			String message = e.toString();
			sender.sendMessage(message);
		}
		
		return true;
	}

}
