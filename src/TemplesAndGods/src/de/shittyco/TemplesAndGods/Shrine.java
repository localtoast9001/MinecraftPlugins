package de.shittyco.TemplesAndGods;

import java.util.*;

import org.bukkit.configuration.serialization.ConfigurationSerializable;
import org.bukkit.inventory.*;

/**
 * Models a shrine of a particular god.
 * @author jrowlett
 *
 */
public class Shrine implements ConfigurationSerializable{
	private int constructionFavor;
	private Map<ItemStack, Integer> sacrifices = new HashMap<ItemStack, Integer>();
	private Map<ItemStack, Integer> blessings = new HashMap<ItemStack, Integer>();
	
	public Shrine() {
		
	}

	public int getConstructionFavor() {
		return this.constructionFavor;
	}
	
	public void setConstructionFavor(int value) {
		this.constructionFavor = value;
	}
	
	public Map<ItemStack, Integer> getSacrifices() {
		return this.sacrifices;
	}
	
	public Map<ItemStack, Integer> getBlessings() {
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
		if(args.containsKey("construction")) {
			result.setConstructionFavor(((Integer) args.get("construction")).intValue());
		}
		
		return result;
	}
}
