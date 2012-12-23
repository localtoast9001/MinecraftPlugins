/**
 * 
 */
package de.shittyco.BitcoinBroker;

import java.util.List;
import java.util.Vector;

import org.bukkit.command.CommandSender;

import de.shittyco.Bitcoin.BTC;

/**
 * @author jrowlett
 *
 */
public class TransactionLogPrinter {
	private static String row1Format = "|%12s|%40s";
	private static String row2Format = "|%10s|%12s|%11s|%13s";
	private static String rowDelimiter = "+----------+------------+-----------+-------------";
	
	public static void print(CommandSender sender, TransactionLogEntry entry) {
		Vector<TransactionLogEntry> entries = new Vector<TransactionLogEntry>();
		entries.add(entry);
		print(sender, entries);
	}
	
	public static void print(CommandSender sender, List<TransactionLogEntry> entries) {
		Vector<String> rows = new Vector<String>();
		for (String s : printHeader(sender)) {
			rows.add(s);
		}
		
		for (TransactionLogEntry entry : entries) {
			for (String s : printBody(entry)) {
				rows.add(s);
			}
		}
		
		sender.sendMessage(rows.toArray(new String[] {}));
	}
	
	private static String[] printHeader(CommandSender sender) {
		return new String[] {
			"|Time           |Description                      ",
			"|BTC Change|Coins Change|BTC Balance|Coins Balance",
			rowDelimiter
		};
	}
	
	private static String[] printBody(TransactionLogEntry entry) {
		Vector<String> rows = new Vector<String>();
		String description = entry.getDescription();
		BTC btcChange = entry.getBtcChange();
		String row1 = String.format(
			row1Format, 
			entry.getTime(),
			description);
		String lastRow = String.format(
			row2Format,
			btcChange,
			new Double(entry.getCoinsChange()).toString(),
			entry.getBtcBalance(),
			new Double(entry.getCoinsBalance()).toString());
		rows.add(row1);
		while(description.length() > 40) {
			description = description.substring(40, description.length() - 1);
			String row = String.format(row1Format, "", description);
			rows.add(row);
		}
		
		rows.add(lastRow);
		rows.add(rowDelimiter);
		return rows.toArray(new String[] {});
	}
}
