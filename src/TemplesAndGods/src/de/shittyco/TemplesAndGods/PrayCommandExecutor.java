package de.shittyco.TemplesAndGods;

import org.bukkit.entity.*;
import org.bukkit.command.*;

public class PrayCommandExecutor implements CommandExecutor {
	private Plugin plugin;
	
	public PrayCommandExecutor(Plugin plugin) {
		this.plugin = plugin;
	}

	@Override
	public boolean onCommand(
		CommandSender sender, 
		Command command, 
		String label,
		String[] args) {

		if(!(sender instanceof Player)) {
			return false;
		}
		
		Player player = (Player)sender;
		if (!player.hasPermission("TemplesAndGods." + label)) {
        	player.sendMessage(String.format("You don't have the [%s] permission.", label));
        	return false;
        }
		
		return false;
	}

}
