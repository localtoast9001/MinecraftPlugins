/**
 * @author jrowlett
 */
package de.shittyco.BitcoinBroker;

import java.util.List;
import java.util.Vector;

import org.bukkit.command.CommandSender;

import de.shittyco.Bitcoin.BTC;

/**
 * Controls formatting and printing of the transaction log.
 * @author jrowlett
 */
public final class TransactionLogPrinter {

    /**
     * The width of the description column.
     */
    private static final int DESCRIPTION_WIDTH = 40;

    /**
     * first row format.
     */
    private static String row1Format =
        "|%12s|%40s";

    /**
     * second row format.
     */
    private static String row2Format =
        "|%10s|%12s|%11s|%13s";

    /**
     * delimiter between rows.
     */
    private static String rowDelimiter =
        "+----------+------------+-----------+-------------";

    /**
     * Declare utility class.
     */
    private TransactionLogPrinter() {

    }

    /**
     * @param sender - the command sender which exposes the output stream.
     * @param entry - the entry to print.
     */
    public static void print(
        final CommandSender sender,
        final TransactionLogEntry entry) {
        Vector<TransactionLogEntry> entries = new Vector<TransactionLogEntry>();
        entries.add(entry);
        print(sender, entries);
    }

    /**
     * @param sender - the command sender which exposes the output stream.
     * @param entries - the set of entries to print.
     */
    public static void print(
        final CommandSender sender,
        final List<TransactionLogEntry> entries) {
        Vector<String> rows = new Vector<String>();
        for (String s : printHeader()) {
            rows.add(s);
        }

        for (TransactionLogEntry entry : entries) {
            for (String s : printBody(entry)) {
                rows.add(s);
            }
        }

        sender.sendMessage(rows.toArray(new String[] {}));
    }

    /**
     * Gets the header to print.
     * @return - the header to print.
     */
    private static String[] printHeader() {
        return new String[] {
                "|Time           |Description                      ",
                "|BTC Change|Coins Change|BTC Balance|Coins Balance",
                rowDelimiter };
    }

    /**
     * @param entry - the entry to print.
     * @return - the lines of text to print.
     */
    private static String[] printBody(final TransactionLogEntry entry) {
        Vector<String> rows = new Vector<String>();
        String description = entry.getDescription();
        BTC btcChange = entry.getBtcChange();
        String row1 = String.format(row1Format, entry.getTime(), description);
        String lastRow = String.format(row2Format, btcChange,
                new Double(entry.getCoinsChange()).toString(),
                entry.getBtcBalance(),
                new Double(entry.getCoinsBalance()).toString());
        rows.add(row1);
        while (description.length() > DESCRIPTION_WIDTH) {
            description = description.substring(
                DESCRIPTION_WIDTH,
                description.length() - 1);
            String row = String.format(row1Format, "", description);
            rows.add(row);
        }

        rows.add(lastRow);
        rows.add(rowDelimiter);
        return rows.toArray(new String[] {});
    }
}
