/**
 * 
 */
package test;

import java.io.File;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.UUID;
import java.util.logging.Logger;

import org.bukkit.GameMode;
import org.bukkit.OfflinePlayer;
import org.bukkit.Server;
import org.bukkit.Warning.WarningState;
import org.bukkit.World;
import org.bukkit.WorldCreator;
import org.bukkit.command.CommandException;
import org.bukkit.command.CommandSender;
import org.bukkit.command.ConsoleCommandSender;
import org.bukkit.command.PluginCommand;
import org.bukkit.entity.Player;
import org.bukkit.event.inventory.InventoryType;
import org.bukkit.help.HelpMap;
import org.bukkit.inventory.Inventory;
import org.bukkit.inventory.InventoryHolder;
import org.bukkit.inventory.ItemStack;
import org.bukkit.inventory.Recipe;
import org.bukkit.map.MapView;
import org.bukkit.plugin.Plugin;
import org.bukkit.plugin.PluginManager;
import org.bukkit.plugin.ServicesManager;
import org.bukkit.plugin.messaging.Messenger;
import org.bukkit.scheduler.BukkitScheduler;

import com.avaje.ebean.config.ServerConfig;

/**
 * @author jrowlett
 *
 */
public class MockServer implements Server {
	private Logger logger;

	/**
	 * 
	 */
	public MockServer() {
		this.logger = Logger.getLogger("MockBukkitServer");
	}

	/* (non-Javadoc)
	 * @see org.bukkit.plugin.messaging.PluginMessageRecipient#sendPluginMessage(org.bukkit.plugin.Plugin, java.lang.String, byte[])
	 */
	@Override
	public void sendPluginMessage(Plugin source, String channel, byte[] message) {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.plugin.messaging.PluginMessageRecipient#getListeningPluginChannels()
	 */
	@Override
	public Set<String> getListeningPluginChannels() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getName()
	 */
	@Override
	public String getName() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getVersion()
	 */
	@Override
	public String getVersion() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getBukkitVersion()
	 */
	@Override
	public String getBukkitVersion() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getOnlinePlayers()
	 */
	@Override
	public Player[] getOnlinePlayers() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getMaxPlayers()
	 */
	@Override
	public int getMaxPlayers() {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getPort()
	 */
	@Override
	public int getPort() {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getViewDistance()
	 */
	@Override
	public int getViewDistance() {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getIp()
	 */
	@Override
	public String getIp() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getServerName()
	 */
	@Override
	public String getServerName() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getServerId()
	 */
	@Override
	public String getServerId() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getWorldType()
	 */
	@Override
	public String getWorldType() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getGenerateStructures()
	 */
	@Override
	public boolean getGenerateStructures() {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getAllowEnd()
	 */
	@Override
	public boolean getAllowEnd() {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getAllowNether()
	 */
	@Override
	public boolean getAllowNether() {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#hasWhitelist()
	 */
	@Override
	public boolean hasWhitelist() {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#setWhitelist(boolean)
	 */
	@Override
	public void setWhitelist(boolean value) {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getWhitelistedPlayers()
	 */
	@Override
	public Set<OfflinePlayer> getWhitelistedPlayers() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#reloadWhitelist()
	 */
	@Override
	public void reloadWhitelist() {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#broadcastMessage(java.lang.String)
	 */
	@Override
	public int broadcastMessage(String message) {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getUpdateFolder()
	 */
	@Override
	public String getUpdateFolder() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getUpdateFolderFile()
	 */
	@Override
	public File getUpdateFolderFile() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getConnectionThrottle()
	 */
	@Override
	public long getConnectionThrottle() {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getTicksPerAnimalSpawns()
	 */
	@Override
	public int getTicksPerAnimalSpawns() {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getTicksPerMonsterSpawns()
	 */
	@Override
	public int getTicksPerMonsterSpawns() {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getPlayer(java.lang.String)
	 */
	@Override
	public Player getPlayer(String name) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getPlayerExact(java.lang.String)
	 */
	@Override
	public Player getPlayerExact(String name) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#matchPlayer(java.lang.String)
	 */
	@Override
	public List<Player> matchPlayer(String name) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getPluginManager()
	 */
	@Override
	public PluginManager getPluginManager() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getScheduler()
	 */
	@Override
	public BukkitScheduler getScheduler() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getServicesManager()
	 */
	@Override
	public ServicesManager getServicesManager() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getWorlds()
	 */
	@Override
	public List<World> getWorlds() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#createWorld(org.bukkit.WorldCreator)
	 */
	@Override
	public World createWorld(WorldCreator creator) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#unloadWorld(java.lang.String, boolean)
	 */
	@Override
	public boolean unloadWorld(String name, boolean save) {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#unloadWorld(org.bukkit.World, boolean)
	 */
	@Override
	public boolean unloadWorld(World world, boolean save) {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getWorld(java.lang.String)
	 */
	@Override
	public World getWorld(String name) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getWorld(java.util.UUID)
	 */
	@Override
	public World getWorld(UUID uid) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getMap(short)
	 */
	@Override
	public MapView getMap(short id) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#createMap(org.bukkit.World)
	 */
	@Override
	public MapView createMap(World world) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#reload()
	 */
	@Override
	public void reload() {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getLogger()
	 */
	@Override
	public Logger getLogger() {
		return this.logger;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getPluginCommand(java.lang.String)
	 */
	@Override
	public PluginCommand getPluginCommand(String name) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#savePlayers()
	 */
	@Override
	public void savePlayers() {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#dispatchCommand(org.bukkit.command.CommandSender, java.lang.String)
	 */
	@Override
	public boolean dispatchCommand(CommandSender sender, String commandLine)
			throws CommandException {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#configureDbConfig(com.avaje.ebean.config.ServerConfig)
	 */
	@Override
	public void configureDbConfig(ServerConfig config) {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#addRecipe(org.bukkit.inventory.Recipe)
	 */
	@Override
	public boolean addRecipe(Recipe recipe) {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getRecipesFor(org.bukkit.inventory.ItemStack)
	 */
	@Override
	public List<Recipe> getRecipesFor(ItemStack result) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#recipeIterator()
	 */
	@Override
	public Iterator<Recipe> recipeIterator() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#clearRecipes()
	 */
	@Override
	public void clearRecipes() {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#resetRecipes()
	 */
	@Override
	public void resetRecipes() {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getCommandAliases()
	 */
	@Override
	public Map<String, String[]> getCommandAliases() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getSpawnRadius()
	 */
	@Override
	public int getSpawnRadius() {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#setSpawnRadius(int)
	 */
	@Override
	public void setSpawnRadius(int value) {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getOnlineMode()
	 */
	@Override
	public boolean getOnlineMode() {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getAllowFlight()
	 */
	@Override
	public boolean getAllowFlight() {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#useExactLoginLocation()
	 */
	@Override
	public boolean useExactLoginLocation() {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#shutdown()
	 */
	@Override
	public void shutdown() {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#broadcast(java.lang.String, java.lang.String)
	 */
	@Override
	public int broadcast(String message, String permission) {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getOfflinePlayer(java.lang.String)
	 */
	@Override
	public OfflinePlayer getOfflinePlayer(String name) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getIPBans()
	 */
	@Override
	public Set<String> getIPBans() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#banIP(java.lang.String)
	 */
	@Override
	public void banIP(String address) {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#unbanIP(java.lang.String)
	 */
	@Override
	public void unbanIP(String address) {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getBannedPlayers()
	 */
	@Override
	public Set<OfflinePlayer> getBannedPlayers() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getOperators()
	 */
	@Override
	public Set<OfflinePlayer> getOperators() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getDefaultGameMode()
	 */
	@Override
	public GameMode getDefaultGameMode() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#setDefaultGameMode(org.bukkit.GameMode)
	 */
	@Override
	public void setDefaultGameMode(GameMode mode) {
		// TODO Auto-generated method stub

	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getConsoleSender()
	 */
	@Override
	public ConsoleCommandSender getConsoleSender() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getWorldContainer()
	 */
	@Override
	public File getWorldContainer() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getOfflinePlayers()
	 */
	@Override
	public OfflinePlayer[] getOfflinePlayers() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getMessenger()
	 */
	@Override
	public Messenger getMessenger() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getHelpMap()
	 */
	@Override
	public HelpMap getHelpMap() {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#createInventory(org.bukkit.inventory.InventoryHolder, org.bukkit.event.inventory.InventoryType)
	 */
	@Override
	public Inventory createInventory(InventoryHolder owner, InventoryType type) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#createInventory(org.bukkit.inventory.InventoryHolder, int)
	 */
	@Override
	public Inventory createInventory(InventoryHolder owner, int size) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#createInventory(org.bukkit.inventory.InventoryHolder, int, java.lang.String)
	 */
	@Override
	public Inventory createInventory(InventoryHolder owner, int size,
			String title) {
		// TODO Auto-generated method stub
		return null;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getMonsterSpawnLimit()
	 */
	@Override
	public int getMonsterSpawnLimit() {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getAnimalSpawnLimit()
	 */
	@Override
	public int getAnimalSpawnLimit() {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getWaterAnimalSpawnLimit()
	 */
	@Override
	public int getWaterAnimalSpawnLimit() {
		// TODO Auto-generated method stub
		return 0;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#isPrimaryThread()
	 */
	@Override
	public boolean isPrimaryThread() {
		// TODO Auto-generated method stub
		return false;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getMotd()
	 */
	@Override
	public String getMotd() {
		// TODO Auto-generated method stub
		return "LOL WUT?";
	}

	/* (non-Javadoc)
	 * @see org.bukkit.Server#getWarningState()
	 */
	@Override
	public WarningState getWarningState() {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public int getAmbientSpawnLimit() {
		// TODO Auto-generated method stub
		return 0;
	}

	@Override
	public boolean isHardcore() {
		// TODO Auto-generated method stub
		return false;
	}

}
