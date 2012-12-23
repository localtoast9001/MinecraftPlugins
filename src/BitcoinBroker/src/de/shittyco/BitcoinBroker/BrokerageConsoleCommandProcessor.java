package de.shittyco.BitcoinBroker;

import org.bukkit.command.CommandSender;

import de.shittyco.Bitcoin.BTC;

public class BrokerageConsoleCommandProcessor extends ConsoleCommandProcessor {
	public BrokerageConsoleCommandProcessor(Model model) {
		super("brokerage", model);
	}

	@Override
	public Boolean onCommand(CommandSender sender, String[] args) {
		if(args.length > 0) {
			if (args[0].equalsIgnoreCase("cashout")) {
				if(args.length != 2) {
					return false;
				}
				
				BTC value = new BTC(args[1]);
				try {
					String txid = this.getModel().cashOut(value);
					sender.sendMessage(String.format("Sent %s BTC. Transaction ID=%s", value, txid));
				} catch (Exception ex) {
					sender.sendMessage(ex.getMessage());
				}
				
				return true;
			}
			
			return false;
		} else {
			sender.sendMessage(this.getModel().getBrokerageInfo().toString());
			sender.sendMessage(String.format("Current Holdings: %s", this.getModel().getBrokerageBalance()));
			sender.sendMessage(String.format("Current Commissions: %s", this.getModel().getCommissionsBalance()));
			sender.sendMessage(String.format("Profit: %s", this.getModel().getBrokerageInfo().getProfitAddress()));
		}
		return true;
	}
}
