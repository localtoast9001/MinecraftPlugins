/**
 * 
 */
package de.shittyco.Platforms;

import org.bukkit.configuration.file.*;
import org.bukkit.plugin.java.*;

/**
 * @author jrowlett
 *
 */
public class Plugin extends JavaPlugin {

	/**
	 * 
	 */
	public Plugin() {
		
	}

	@Override
	public void onEnable() {
		getServer().getPluginManager().registerEvent(null, null, null, null, null);

	}
	
	@Override
	public void onDisable() {
		
	}
}
