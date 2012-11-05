/**
 * 
 */
package de.shittyco.BitcoinBroker;

import org.bukkit.command.Command;
import org.bukkit.command.CommandExecutor;
import org.bukkit.command.CommandSender;
import org.bukkit.entity.*;;

/**
 * @author Jon Rowlett
 *
 */
public class RootCommandExecutor implements CommandExecutor {
	private PlayerCommandProcessor playerCommandProcessors[];
	private ConsoleCommandProcessor consoleCommandProcessors[];
	
	public RootCommandExecutor(Model model) {

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
            if (!player.hasPermission(label)) {
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
		for(int i = 0; i < this.playerCommandProcessors.length; i++) {
			if(this.playerCommandProcessors[i].getCommand().equalsIgnoreCase(args[0])) {
				String remainingArgs[] = skip(args, 1);
				return this.playerCommandProcessors[i].onCommand(player, remainingArgs);
			}
		}
		
		return false;
	}
	
	private Boolean onServerCommand(CommandSender sender, String[] args) {
		for(int i=0; i < this.consoleCommandProcessors.length; i++) {
			if(this.consoleCommandProcessors[i].getCommand().equalsIgnoreCase(args[0])) {
				return this.consoleCommandProcessors[i].onCommand(sender, skip(args, 1));
			}
		}
		
		return false;
	}
}
