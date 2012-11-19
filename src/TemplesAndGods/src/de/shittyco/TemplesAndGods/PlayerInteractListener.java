package de.shittyco.TemplesAndGods;

import org.bukkit.Material;
import org.bukkit.block.*;
import org.bukkit.entity.Player;
import org.bukkit.event.*;
import org.bukkit.event.player.*;
import org.bukkit.event.block.BlockPlaceEvent;

public class PlayerInteractListener implements Listener {
	private Plugin plugin;
	
	public PlayerInteractListener(Plugin plugin) {
		this.plugin = plugin;
	}

	@EventHandler
	public void onBlockPlace(BlockPlaceEvent event) {
		Block placedBlock = event.getBlockPlaced();
		if (placedBlock.getType() == Material.CHEST) {
			Player player = event.getPlayer();
		}
	}
}
