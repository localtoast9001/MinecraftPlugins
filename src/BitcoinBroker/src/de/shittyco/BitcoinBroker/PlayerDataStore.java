/**
 * 
 */
package de.shittyco.BitcoinBroker;

import java.util.List;

import org.bukkit.OfflinePlayer;
import org.bukkit.configuration.file.FileConfiguration;
import org.bukkit.entity.Player;
import org.bukkit.plugin.Plugin;

/**
 * @author jrowlett
 * Manages loading and persisting data associated with each player.
 */
public class PlayerDataStore {
	
	private Plugin plugin;
	private FileConfiguration config;
	
	public PlayerDataStore(Plugin plugin) {
		this.plugin = plugin;
		this.config = plugin.getConfig();
	}

	public String getPlayerLinkedAddress(OfflinePlayer player) {
		PlayerData data = getPlayer(player);
		return data.getBtcLinkedAddress();
	}
	
	public void setLinkedAddress(OfflinePlayer player, String linkedAddress) throws Exception {
		PlayerData data = getPlayer(player);
		data.setBtcLinkedAddress(linkedAddress);
		this.plugin.saveConfig();
	}
	
	public List<TransactionLogEntry> getPlayerLatestTransactions(OfflinePlayer player) {
		PlayerData data = getPlayer(player);
		return data.getLatestTransactions();
	}
	
	public void logTransactionEntry(Player player, TransactionLogEntry entry) {
		PlayerData data = getPlayer(player);
		List<TransactionLogEntry> existing = data.getLatestTransactions();
		existing.add(entry);
		if (existing.size() > 16) {
			existing.remove(0);
		}
		
		this.plugin.saveConfig();
	}	
	
	private PlayerData getPlayer(OfflinePlayer player) {
		PlayerData raw = (PlayerData) this.config.get(player.getName());
		if(raw == null) {
			raw = new PlayerData();
			this.config.set(player.getName(), raw);
		}
		
		return raw;
	}
}
