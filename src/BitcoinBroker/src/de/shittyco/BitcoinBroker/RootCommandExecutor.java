/**
 * @author jrowlett
 */
package de.shittyco.BitcoinBroker;

import java.util.Iterator;
import java.util.ArrayList;
import java.util.Collection;

import org.bukkit.Server;
import org.bukkit.command.Command;
import org.bukkit.command.CommandExecutor;
import org.bukkit.command.CommandSender;
import org.bukkit.entity.Player;

/**
 * Dispatcher for all commands.
 * @author Jon Rowlett
 */
public class RootCommandExecutor implements CommandExecutor {
    /**
     * List of player command processors.
     */
    private Collection<PlayerCommandProcessor> playerCommandProcessors =
            new ArrayList<PlayerCommandProcessor>();

    /**
     * List of console command processors.
     */
    private Collection<ConsoleCommandProcessor> consoleCommandProcessors =
            new ArrayList<ConsoleCommandProcessor>();

    /**
     * Initializes a new instance of the RootCommandExecutor.
     * @param model the plugin's internal model.
     * @param server the bukkit server.
     */
    public RootCommandExecutor(final Model model, final Server server) {
        this.playerCommandProcessors.add(new BuyPlayerCommandProcessor(model));
        this.playerCommandProcessors.add(new SellPlayerCommandProcessor(model));
        this.playerCommandProcessors.add(new TransferPlayerCommandProcessor(
                model));
        this.playerCommandProcessors.add(new AccountPlayerCommandProcessor(
                model));
        this.playerCommandProcessors.add(new BrokeragePlayerCommandProcessor(
                model));
        this.consoleCommandProcessors.add(new BrokerageConsoleCommandProcessor(
                model));
        this.consoleCommandProcessors.add(new AccountConsoleCommandProcessor(
                model, server));
    }

    /*
     * (non-Javadoc)
     *
     * @see
     * org.bukkit.command.CommandExecutor#onCommand(org.bukkit.command.
     * CommandSender, org.bukkit.command.Command, java.lang.String,
     * java.lang.String[])
     */
    @Override
    public final boolean onCommand(
        final CommandSender sender,
        final Command command,
        final String label,
        final String[] args) {
        if (args.length < 1) {
            return false;
        }

        if (sender instanceof Player) {
            Player player = (Player) sender;
            if (!player.hasPermission("BitcoinBroker." + label)) {
                player.sendMessage(String.format(
                        "You don't have the [%s] permission.", label));
                return false;
            }

            return this.onPlayerCommand(player, args);
        }

        return this.onServerCommand(sender, args);
    }

    /**
     * Skips the first N items as determined by count.
     * @param source the source array.
     * @param count the number of items to skip
     * @return the trimmed array
     */
    private static String[] skip(
        final String[] source,
        final int count) {
        if (count >= source.length) {
            return new String[0];
        }

        String[] result = new String[source.length - count];
        for (int i = 0, j = count; j < source.length; i++, j++) {
            result[i] = source[j];
        }

        return result;
    }

    /**
     * Dispatches a player command.
     * @param player the player that issued the command
     * @param args the command arguments.
     * @return a value indicating whether the command was dispatched.
     */
    private Boolean onPlayerCommand(
        final Player player,
        final String[] args) {
        Iterator<PlayerCommandProcessor> i = this.playerCommandProcessors
                .iterator();
        while (i.hasNext()) {
            PlayerCommandProcessor e = i.next();
            if (e.getCommand().equalsIgnoreCase(args[0])) {
                String[] remainingArgs = skip(args, 1);
                return e.onCommand(player, remainingArgs);
            }
        }

        return false;
    }

    /**
     * Dispatches a server command.
     * @param sender the console sender.
     * @param args the remaining command arguments.
     * @return a value indicating whether or not the command could be processed.
     */
    private Boolean onServerCommand(
        final CommandSender sender,
        final String[] args) {
        Iterator<ConsoleCommandProcessor> i = this.consoleCommandProcessors
                .iterator();
        while (i.hasNext()) {
            ConsoleCommandProcessor e = i.next();
            if (e.getCommand().equalsIgnoreCase(args[0])) {
                return e.onCommand(sender, skip(args, 1));
            }
        }

        return false;
    }
}
