/**
 * 
 */
package de.shittyco.BitcoinBroker;

import java.util.*;

import org.bukkit.Server;
import org.bukkit.command.Command;
import org.bukkit.command.CommandExecutor;
import org.bukkit.command.CommandSender;
import org.bukkit.entity.*;

/**
 * @author Jon Rowlett
 *
 */
public class RootCommandExecutor implements CommandExecutor {
	private Collection<PlayerCommandProcessor> playerCommandProcessors = new ArrayList<PlayerCommandProcessor>();
	private Collection<ConsoleCommandProcessor> consoleCommandProcessors = new ArrayList<ConsoleCommandProcessor>();
	
	public RootCommandExecutor(Model model, Server server) {
		this.playerCommandProcessors.add(new BuyPlayerCommandProcessor(model));
		this.playerCommandProcessors.add(new SellPlayerCommandProcessor(model));
		this.playerCommandProcessors.add(new TransferPlayerCommandProcessor(model));
		this.playerCommandProcessors.add(new AccountPlayerCommandProcessor(model));
		this.playerCommandProcessors.add(new BrokeragePlayerCommandProcessor(model));
		this.consoleCommandProcessors.add(new BrokerageConsoleCommandProcessor(model));
		this.consoleCommandProcessors.add(new AccountConsoleCommandProcessor(model, server));
	}
	
	/* (non-Javadoc)
	 * @see org.bukkit.command.CommandExecutor#onCommand(org.bukkit.command.CommandSender, org.bukkit.command.Command, java.lang.String, java.lang.String[])
	 */
	@Override
	public boolean onCommand(
		CommandSender sender, 
		Command command, 
		String label,
		String[] args) {
		if (args.length < 1) {
			return false;
		}
		
		if (sender instanceof Player) {
            Player player = (Player) sender;
            if (!player.hasPermission("BitcoinBroker." + label)) {
            	player.sendMessage(String.format("You don't have the [%s] permission.", label));
            	return false;
            }
            
            return this.onPlayerCommand(player, args);
        }
		
		return this.onServerCommand(sender, args);
	}
	
	private static String[] skip(String source[], int count) {
		if(count >= source.length) {
			return new String[0];
		}
		
		String result[] = new String[source.length - count];
		for(int i = 0, j = count; j < source.length; i++, j++) {
			result[i] = source[j];
		}
		
		return result;
	}
	
	private Boolean onPlayerCommand(Player player, String[] args) {
		Iterator<PlayerCommandProcessor> i = this.playerCommandProcessors.iterator(); 
		while(i.hasNext()) {
			PlayerCommandProcessor e = i.next();
			if(e.getCommand().equalsIgnoreCase(args[0])) {
				String remainingArgs[] = skip(args, 1);
				return e.onCommand(player, remainingArgs);
			}
		}
		
		return false;
	}
	
	private Boolean onServerCommand(CommandSender sender, String[] args) {
		Iterator<ConsoleCommandProcessor> i = this.consoleCommandProcessors.iterator();
		while(i.hasNext()) {
			ConsoleCommandProcessor e = i.next();
			if(e.getCommand().equalsIgnoreCase(args[0])) {
				return e.onCommand(sender, skip(args, 1));
			}
		}
		
		return false;
	}
}
