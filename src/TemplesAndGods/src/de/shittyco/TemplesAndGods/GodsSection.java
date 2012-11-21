/**
 * 
 */
package de.shittyco.TemplesAndGods;

import java.util.HashMap;
import java.util.Map;

import org.bukkit.configuration.serialization.ConfigurationSerializable;
import org.bukkit.configuration.serialization.ConfigurationSerialization;
import org.bukkit.configuration.serialization.SerializableAs;

/**
 * @author jrowlett
 * 
 * The root of all definitions for all gods in the config.
 */
@SerializableAs("GodsSection")
public class GodsSection implements ConfigurationSerializable {
	
	private Map<String, God> gods = new HashMap<String, God>();

	/**
	 * Registers with the config serialization system.
	 */
	public static void registerClass() {
		ConfigurationSerialization.registerClass(GodsSection.class);
		ConfigurationSerialization.registerClass(God.class);
		ConfigurationSerialization.registerClass(Shrine.class);
		ConfigurationSerialization.registerClass(Trade.class);	
	}
	
	/**
	 * Initializes a new instance of the GodsSection class.
	 */
	public GodsSection() {

	}
	
	/**
	 * Gets the gods
	 * @return the map of gods.
	 */
	public Map<String, God> getGods() {
		return this.gods;
	}
	
	/**
	 * Deserializes the GodsSection 
	 * @param args
	 * @return the deserialized instance.
	 */
	public static GodsSection deserialize(Map<String, Object> args) {
		GodsSection result = new GodsSection();
		for (String key : args.keySet()) {
			Object raw = args.get(key);
			if (raw instanceof God) {
				result.gods.put(key, (God) raw);
			}
		}
		
		return result;
	}

	/* (non-Javadoc)
	 * @see org.bukkit.configuration.serialization.ConfigurationSerializable#serialize()
	 */
	@Override
	public Map<String, Object> serialize() {

		return null;
	}
}
