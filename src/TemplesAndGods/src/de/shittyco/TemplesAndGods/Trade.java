/**
 * 
 */
package de.shittyco.TemplesAndGods;

import java.util.*;

import org.bukkit.configuration.serialization.ConfigurationSerializable;
import org.bukkit.configuration.serialization.SerializableAs;
import org.bukkit.inventory.ItemStack;

/**
 * @author jrowlett
 * Describes either a blessing or a sacrifice in terms of the item stack and equivalent favor.
 */
@SerializableAs("Trade")
public class Trade implements ConfigurationSerializable {

	private ItemStack item;
	private int favor;
	
	/**
	 * @throws Exception 
	 * 
	 */
	public Trade(ItemStack item, int favor) throws IllegalArgumentException {
		if (item == null) {
			throw new IllegalArgumentException("item");
		}
		
		if (favor <= 0) {
			throw new IllegalArgumentException("favor");
		}
		
		this.item = item;
		this.favor = favor;
	}
	
	public ItemStack getItem() {
		return this.item;
	}
	
	public int getFavor() {
		return this.favor;
	}

	@Override
	public Map<String, Object> serialize() {
		Map<String, Object> result = new LinkedHashMap<String, Object>();
		result.put("item", this.item);
		result.put("favor", this.favor);
		return result;
	}

	public static Trade deserialize(Map<String, Object> args) throws Exception {
		ItemStack item = (ItemStack) args.get("item");
		int favor = ((Integer) args.get("favor")).intValue();
		return new Trade(item, favor);
	}
}
