/**
 * 
 */
package de.shittyco.TemplesAndGods;

import java.util.*;

import org.bukkit.configuration.file.*;

/**
 * @author jrowlett
 *
 */
public class Model {
	
	/**
	 * Configuration for all gods.
	 */
	private GodsSection godsSection = new GodsSection();

	/**
	 * Initializes an empty model for testing.
	 */
	public Model() {
		
	}
	
	public Map<String, God> getGods() {
		return this.godsSection.getGods();
	}
	
	/**
	 * Creates a model from configuration.
	 * @param config
	 * @return
	 */
	public static Model load(FileConfiguration config) {		
		Model model = new Model();
		model.godsSection = (GodsSection) config.get("gods");

		return model;
	}
}
