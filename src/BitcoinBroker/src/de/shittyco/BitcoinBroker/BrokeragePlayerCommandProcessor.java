package de.shittyco.BitcoinBroker;

import org.bukkit.entity.Player;

/**
 * Processes the brokerage command issued by a player.
 * @author jrowlett
 */
public class BrokeragePlayerCommandProcessor extends PlayerCommandProcessor {

    /**
     * Initializes a new instance of the BrokeragePlayerCommandProcessor.
     * @param model the reference to the model
     */
    public BrokeragePlayerCommandProcessor(final Model model) {
        super("brokerage", model);
    }

    /* (non-Javadoc)
     * @see de.shittyco.BitcoinBroker.PlayerCommandProcessor#onCommand(
     * org.bukkit.entity.Player, java.lang.String[])
     */
    @Override
    public final Boolean onCommand(final Player sender, final String[] args) {
        sender.sendMessage(this.getModel().getBrokerageInfo().toString());
        return true;
    }

}
