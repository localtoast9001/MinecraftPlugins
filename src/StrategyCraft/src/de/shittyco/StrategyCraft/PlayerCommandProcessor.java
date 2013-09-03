/**
 * @author jrowlett
 */
package de.shittyco.StrategyCraft;

import org.bukkit.entity.Player;

/**
 * @author jrowlett
 *
 */
public abstract class PlayerCommandProcessor extends CommandProcessor {

    /**
     * @param command
     *            - the name of the command
     * @param model
     *            - the plug-in model
     */
    protected PlayerCommandProcessor(
        final String command,
        final Model model) {
        super(command, model);
    }

    /**
     * Processes an individual command.
     * @param sender the player issuing the command.
     * @param args the remaining command args
     * @return whether or not the command was processed.
     */
    public abstract Boolean onCommand(Player sender, String[] args);
}
