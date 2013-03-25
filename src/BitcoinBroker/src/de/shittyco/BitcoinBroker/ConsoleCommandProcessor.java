package de.shittyco.BitcoinBroker;

import org.bukkit.command.CommandSender;

/**
 * Base class for command processors that process commands from the console.
 * @author jrowlett
 *
 */
public abstract class ConsoleCommandProcessor extends CommandProcessor {

    /**
     * Initializes a new instance of the ConsoleCommandProcessor.
     * @param command - the name of the command.
     * @param model - the plugin model.
     */
    protected ConsoleCommandProcessor(final String command, final Model model) {
        super(command, model);
    }

    /**
     * Processes the command from the console.
     * @param sender - the console sender.
     * @param args - the remaining command args.
     * @return - whether or not the command was processed.
     */
    public abstract Boolean onCommand(CommandSender sender, String[] args);
}
