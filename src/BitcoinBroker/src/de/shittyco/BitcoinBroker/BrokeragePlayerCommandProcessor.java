package de.shittyco.BitcoinBroker;

import org.bukkit.entity.Player;

public class BrokeragePlayerCommandProcessor extends PlayerCommandProcessor {

	public BrokeragePlayerCommandProcessor(Model model) {
		super("brokerage", model);
	}

	@Override
	public Boolean onCommand(Player sender, String[] args) {
		sender.sendMessage(this.getModel().getBrokerageInfo().toString());
		return true;
	}

}
