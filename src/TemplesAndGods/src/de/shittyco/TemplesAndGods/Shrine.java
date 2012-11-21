package de.shittyco.TemplesAndGods;

import java.util.*;

import org.bukkit.configuration.serialization.ConfigurationSerializable;
import org.bukkit.configuration.serialization.SerializableAs;

/**
 * Models a shrine of a particular god.
 * @author jrowlett
 *
 */
@SerializableAs("Shrine")
public class Shrine implements ConfigurationSerializable{
	private int constructionFavor;
	private Map<String, Trade> sacrifices = new HashMap<String, Trade>();
	private Map<String, Trade> blessings = new HashMap<String, Trade>();
	
	public Shrine() {
		
	}

	public int getConstructionFavor() {
		return this.constructionFavor;
	}
	
	public void setConstructionFavor(int value) {
		this.constructionFavor = value;
	}
	
	public Map<String, Trade> getSacrifices() {
		return this.sacrifices;
	}
	
	public Map<String, Trade> getBlessings() {
		return this.blessings;
	}

	@Override
	public Map<String, Object> serialize() {
		Map<String, Object> result = new LinkedHashMap<String, Object>();
		result.put("construction", this.getConstructionFavor());
		return result;
	}
	
	public static Shrine deserialize(Map<String, Object> args) {
		Shrine result = new Shrine();
		if (args.containsKey("construction")) {
			result.setConstructionFavor(((Integer) args.get("construction")).intValue());
		}
		
		if (args.containsKey("sacrifices")) {
			Object raw = args.get("sacrifices");
			if (raw instanceof Map<?, ?>) {
				Map<?, ?> sacSection = (Map<?, ?>) raw;
				deserializeTradeMap(sacSection, result.sacrifices);
			}
		}
		
		if (args.containsKey("blessings")) {
			Object raw = args.get("blessings");
			if (raw instanceof Map<?, ?>) {
				Map<?, ?> blessSection = (Map<?, ?>) raw;
				deserializeTradeMap(blessSection, result.blessings);
			}
		}
		
		return result;
	}
	
	private static void deserializeTradeMap(
		Map<?, ?> source, 
		Map<String, Trade> target) {
		for (Object key : source.keySet()) {
			Object raw = source.get(key);
			if(raw instanceof Trade) {
				Trade value = (Trade) raw;
				target.put((String) key, value);
			}
		}
	}
}
