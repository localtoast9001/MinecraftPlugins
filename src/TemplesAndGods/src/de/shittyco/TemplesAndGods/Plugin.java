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
public class Plugin extends JavaPlugin {

	/*
	 * Core game logic model.
	 */
	private Model model;
	
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
		this.model = Model.load(config);
		this.getLogger().log(Level.INFO, "Temples and Gods enabled.");
	}
	
	@Override
	public void onDisable() {
		this.getLogger().log(Level.INFO, "Temples and Gods disabled.");
	}
}
