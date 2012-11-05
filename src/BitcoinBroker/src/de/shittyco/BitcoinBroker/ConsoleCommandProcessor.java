package de.shittyco.BitcoinBroker;

import org.bukkit.command.CommandSender;

public abstract class ConsoleCommandProcessor extends CommandProcessor {

	protected ConsoleCommandProcessor(String command, Model model) {
		super(command, model);
	}

	public abstract Boolean onCommand(
		CommandSender sender, 
		String[] args);
}
