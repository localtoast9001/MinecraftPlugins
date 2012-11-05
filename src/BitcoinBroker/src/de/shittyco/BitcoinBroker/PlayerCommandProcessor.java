/**
 * 
 */
package de.shittyco.BitcoinBroker;

import org.bukkit.entity.Player;

/**
 * @author jrowlett
 *
 */
public abstract class PlayerCommandProcessor extends CommandProcessor {

	/**
	 * @param command
	 */
	protected PlayerCommandProcessor(String command, Model model) {
		super(command, model);
	}

	public abstract Boolean onCommand(Player sender, String args[]);
}
