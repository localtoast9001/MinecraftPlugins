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
public class BuyPlayerCommandProcessor extends PlayerCommandProcessor {

	/**
	 * @param model
	 */
	public BuyPlayerCommandProcessor(Model model) {
		super("buy", model);

	}

	/* (non-Javadoc)
	 * @see de.shittyco.BitcoinBroker.PlayerCommandProcessor#onCommand(org.bukkit.entity.Player, java.lang.String[])
	 */
	@Override
	public Boolean onCommand(Player sender, String[] args) {
		if (args.length != 1) {
			return false;
		}
		
		try {
			BTC btc = new BTC(args[0]);
			TransactionLogEntry entry = this.getModel().buy(sender, btc);
			TransactionLogPrinter.print(sender, entry);
		} catch (Exception ex) {
			sender.sendMessage(ex.toString());
		}
		
		return true;
	}

}
