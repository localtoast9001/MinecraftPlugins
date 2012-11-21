package de.shittyco.TemplesAndGods;

import java.util.LinkedHashMap;
import java.util.Map;

import org.bukkit.configuration.serialization.ConfigurationSerializable;
import org.bukkit.configuration.serialization.SerializableAs;

/**
 * Represents a god in the game.
 * @author jrowlett
 *
 */
@SerializableAs("God")
public class God implements ConfigurationSerializable {

	private String name;
	private Shrine shrine;
	
	public God() {
		
	}

	public String getName() {
		return this.name;
	}
	
	public void setName(String value) {
		this.name = value;
	}
	
	public Shrine getShrine() {
		return this.shrine;
	}
	
	public void setShrine(Shrine value) {
		this.shrine = value;
	}

	@Override
	public Map<String, Object> serialize() {
		Map<String, Object> result = new LinkedHashMap<String, Object>();
		result.put("name", this.getName());
		if( this.getShrine() != null) {
			result.put("shrine", this.getShrine());
		}
		
		return result;
	}
	
	public static God deserialize(Map<String, Object> args) {
		God result = new God();
		result.setName((String) args.get("name"));
		if (args.containsKey("shrine")) {
			result.setShrine((Shrine) args.get("shrine"));
		}
		
		return result;
	}
}
