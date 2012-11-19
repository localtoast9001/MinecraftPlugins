/**
 * 
 */
package de.shittyco.TemplesAndGods;

import java.util.logging.Level;

import org.bukkit.configuration.file.*;
import org.bukkit.plugin.java.*;

/**
 * @author jrowlett
 *
 */
public class Plugin extends JavaPlugin{

	/**
	 * 
	 */
	public Plugin() {

	}

	@Override
	public void onEnable() {
		getCommand("pray").setExecutor(new PrayCommandExecutor(this));
		getServer().getPluginManager().registerEvents(new PlayerInteractListener(this),  this);
		
		FileConfiguration config = this.getConfig();
	}
	
	@Override
	public void onDisable() {
		
	}
}
