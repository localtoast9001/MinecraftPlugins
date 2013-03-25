/**
 * @author jrowlett
 */
package de.shittyco.BitcoinBroker;

import org.bukkit.entity.Player;

import de.shittyco.Bitcoin.BTC;

/**
 * Processes a Buy command issued by the player.
 * @author jrowlett
 */
public class BuyPlayerCommandProcessor extends PlayerCommandProcessor {

    /**
     * @param model
     *            - The model for the plugin.
     */
    public BuyPlayerCommandProcessor(final Model model) {
        super("buy", model);

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
            TransactionLogEntry entry = this.getModel().buy(sender, btc);
            TransactionLogPrinter.print(sender, entry);
        } catch (Exception ex) {
            sender.sendMessage(ex.toString());
        }

        return true;
    }

}
