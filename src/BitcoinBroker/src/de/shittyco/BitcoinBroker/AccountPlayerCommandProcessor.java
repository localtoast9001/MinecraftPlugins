/**
 *
 */
package de.shittyco.BitcoinBroker;

import org.bukkit.entity.Player;

/**
 * @author jrowlett
 *
 */
public class AccountPlayerCommandProcessor extends PlayerCommandProcessor {

    /**
     * @param model - the plug-in model
     */
    public AccountPlayerCommandProcessor(final Model model) {
        super("account", model);
        // TODO Auto-generated constructor stub
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
        if (args.length == 0) {
            try {
                PlayerAccountInfo accountInfo = this.getModel().getAccountInfo(
                        sender);
                sender.sendMessage(accountInfo.toString());
                TransactionLogPrinter.print(sender,
                        accountInfo.getLatestTransactions());
            } catch (Exception e) {
                sender.sendMessage(e.getMessage());
            }

            return true;
        }

        if (args.length == 2 && args[0].equalsIgnoreCase("link")) {
            try {
                this.getModel().setLinkedAddress(sender, args[1]);
                sender.sendMessage(String.format("Set Linked Address to [%s]",
                        args[1]));
            } catch (Exception e) {
                sender.sendMessage(e.toString());
            }

            return true;
        }

        return false;
    }

}
