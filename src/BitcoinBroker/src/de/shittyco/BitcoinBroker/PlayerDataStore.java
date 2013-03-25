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
 * @author jrowlett Manages loading and persisting data associated with each
 *         player.
 */
public class PlayerDataStore {

    /**
     * Max transaction history to keep.
     */
    private static final int MAX_HISTORY = 16;

    /**
     * Reference to the plugin to interface load/save.
     */
    private Plugin pluginRef;

    /**
     * Reference to the file configuration used for storage.
     */
    private FileConfiguration config;

    /**
     * Initializes a new instance of the PlayerDataStore class.
     * @param plugin - the plugin used to interface storage.
     */
    public PlayerDataStore(final Plugin plugin) {
        this.pluginRef = plugin;
        this.config = plugin.getConfig();
    }

    /**
     * Gets the player's linked BTC address.
     * @param player - the player for which to get the value.
     * @return - the linked BTC address.
     */
    public final String getPlayerLinkedAddress(final OfflinePlayer player) {
        PlayerData data = getPlayer(player);
        return data.getBtcLinkedAddress();
    }

    /**
     * Sets the player's linked BTC address.
     * @param player - the player for which to set the value.
     * @param linkedAddress - the new value.
     * @throws Exception - unexpected storage exception.
     */
    public final void setLinkedAddress(
        final OfflinePlayer player,
        final String linkedAddress)
            throws Exception {
        PlayerData data = getPlayer(player);
        data.setBtcLinkedAddress(linkedAddress);
        this.pluginRef.saveConfig();
    }

    /**
     * Gets the player's latest transactions.
     * @param player - the player in scope.
     * @return - the list of transactions.
     */
    public final List<TransactionLogEntry> getPlayerLatestTransactions(
        final OfflinePlayer player) {
        PlayerData data = getPlayer(player);
        return data.getLatestTransactions();
    }

    /**
     * Logs a new transaction entry.
     * @param player - the player for which the transaction applies.
     * @param entry - the log entry for the transaction.
     */
    public final void logTransactionEntry(
        final Player player,
        final TransactionLogEntry entry) {
        PlayerData data = getPlayer(player);
        List<TransactionLogEntry> existing = data.getLatestTransactions();
        existing.add(entry);
        if (existing.size() > MAX_HISTORY) {
            existing.remove(0);
        }

        this.pluginRef.saveConfig();
    }

    /**
     * Loads player data from configuration.
     * @param player - the player to load.
     * @return - the player data.
     */
    private PlayerData getPlayer(final OfflinePlayer player) {
        PlayerData raw = (PlayerData) this.config.get(player.getName());
        if (raw == null) {
            raw = new PlayerData();
            this.config.set(player.getName(), raw);
        }

        return raw;
    }
}
