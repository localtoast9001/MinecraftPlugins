package de.shittyco.BitcoinBroker;

import org.bukkit.entity.Player;

import de.shittyco.Bitcoin.BTC;

public class SellPlayerCommandProcessor extends PlayerCommandProcessor {

	public SellPlayerCommandProcessor(Model model) {
		super("sell", model);

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
			this.getModel().sell(sender, btc);
		} catch (Exception ex) {
			sender.sendMessage(ex.getMessage());
		}
		
		return true;
	}

}
