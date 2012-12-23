/**
 * 
 */
package de.shittyco.BitcoinBroker;

import org.bukkit.entity.Player;
import de.shittyco.Bitcoin.BTC;

/**
 * @author jrowlett
 *
 */
public class TransferPlayerCommandProcessor extends PlayerCommandProcessor {

	public TransferPlayerCommandProcessor(Model model) {
		super("transfer", model);
	}
	
	/* (non-Javadoc)
	 * @see de.shittyco.BitcoinBroker.PlayerCommandProcessor#onCommand(org.bukkit.entity.Player, java.lang.String[])
	 */
	@Override
	public Boolean onCommand(Player sender, String[] args) {
		if(args.length != 1) {
			return false;
		}
		
		BTC value = new BTC(args[0]);
		try {
			TransactionLogEntry entry = this.getModel().transfer(sender, value);
			TransactionLogPrinter.print(sender, entry);
		} catch(Exception ex) {
			sender.sendMessage(ex.toString());
		}
		
		return true;
	}

}
