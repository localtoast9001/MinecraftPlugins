package de.shittyco.BitcoinBroker;

import org.bukkit.command.CommandSender;

public class BrokerageConsoleCommandProcessor extends ConsoleCommandProcessor {
	public BrokerageConsoleCommandProcessor(Model model) {
		super("brokerage", model);
	}

	@Override
	public Boolean onCommand(CommandSender sender, String[] args) {
		sender.sendMessage(this.getModel().getBrokerageInfo().toString());
		sender.sendMessage(String.format("Current Holdings: %s", this.getModel().getBrokerageBalance()));
		sender.sendMessage(String.format("Profit: %s", this.getModel().getBrokerageInfo().getProfitAddress()));
		return true;
	}
}
