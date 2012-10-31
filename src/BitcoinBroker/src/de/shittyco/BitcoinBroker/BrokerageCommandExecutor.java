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
public class BrokerageCommandExecutor implements CommandExecutor {
	private Model model;
	
	public BrokerageCommandExecutor(Model model) {
		this.model = model;
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
		if (sender instanceof Player) {
            Player player = (Player) sender;
            return this.onCommand(player, label);
        }
		
		return this.onServerCommand(sender);
	}

	private boolean onCommand(Player sender, String label) {
		if(!sender.hasPermission(label)) {
     	   return false;
        }
		
		sender.sendMessage(this.model.getBrokerageInfo().toString());
		return true;
	}
	
	private boolean onServerCommand(CommandSender sender) {
		sender.sendMessage(this.model.getBrokerageInfo().toString());
		sender.sendMessage(String.format("Profit: %s", this.model.getBrokerageInfo().getProfitAddress()));
		return true;
	}
}
