/**
 * 
 */
package de.shittyco.TemplesAndGods;

import java.util.*;

import org.bukkit.configuration.*;
import org.bukkit.configuration.file.*;
import org.bukkit.configuration.serialization.ConfigurationSerialization;
import org.bukkit.inventory.ItemStack;

/**
 * @author jrowlett
 *
 */
public class Model {
	
	private Map<String, God> gods = new HashMap<String, God>();

	/**
	 * 
	 */
	public Model() {
	}
	
	public Map<String, God> getGods() {
		return this.gods;
	}
	
	/**
	 * Creates a model from configuration.
	 * @param config
	 * @return
	 */
	public static Model load(FileConfiguration config) {
		ConfigurationSerialization.registerClass(God.class);
		ConfigurationSerialization.registerClass(Shrine.class);
		ConfigurationSerialization.registerClass(Trade.class);
		
		Model model = new Model();
		ConfigurationSection godsSection = config.getConfigurationSection("gods");
		Set<String> keys = godsSection.getKeys(false);
		Iterator<String> keyIter = keys.iterator();
		while(keyIter.hasNext()) {
			String key = keyIter.next();
			ConfigurationSection godSection = godsSection.getConfigurationSection(key);
			God god = loadGod(godSection);
			model.gods.put(key, god);
		}
		
		return model;
	}

	/**
	 * Loads configuration for a single god.
	 * @param section
	 * @return
	 */
	private static God loadGod(ConfigurationSection section) {
		God god = God.deserialize(section.getValues(false));
		return god;
	}
		
	/**
	 * Loads pairs of Item Stack to favor points
	 * @param source
	 * @param target
	 */
	private static void loadItemMap(ConfigurationSection source, Map<ItemStack, Integer> target) {
		Set<String> keys = source.getKeys(false);
		Iterator<String> iter = keys.iterator();
		while (iter.hasNext()) {
			String key = iter.next();
			ConfigurationSection pair = source.getConfigurationSection(key);
			ItemStack item = pair.getItemStack("item");
			int favor = pair.getInt("favor", 0);
			target.put(item, favor);
		}
	}
}
