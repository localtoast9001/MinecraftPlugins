package de.shittyco.BitcoinBroker;

import org.bukkit.entity.Player;

import de.shittyco.Bitcoin.BTC;

/**
 * Processes a sell command initiated by a player.
 * @author jrowlett
 *
 */
public class SellPlayerCommandProcessor extends PlayerCommandProcessor {

    /**
     * Initializes a new instance of the SellPlayerCommandProcessor class.
     * @param model - reference to the plugin's model to process the command.
     */
    public SellPlayerCommandProcessor(final Model model) {
        super("sell", model);

    }

    /*
     * (non-Javadoc)
     *
     * @see
     * de.shittyco.BitcoinBroker.PlayerCommandProcessor#onCommand(org.bukkit
     * .entity.Player, java.lang.String[])
     */
    @Override
    public final Boolean onCommand(final Player sender, final String[] args) {
        if (args.length != 1) {
            return false;
        }

        try {
            BTC btc = new BTC(args[0]);
            TransactionLogEntry entry = this.getModel().sell(sender, btc);
            TransactionLogPrinter.print(sender, entry);
        } catch (Exception ex) {
            sender.sendMessage(ex.toString());
        }

        return true;
    }

}
