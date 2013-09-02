/**
 * @author jrowlett
 */
package de.shittyco.NPCTest;

import java.util.logging.Level;
import java.util.HashMap;

import org.bukkit.Location;
import org.bukkit.World;
import org.bukkit.command.Command;
import org.bukkit.command.CommandExecutor;
import org.bukkit.command.CommandSender;
import org.bukkit.entity.Entity;
import org.bukkit.entity.EntityType;
import org.bukkit.entity.Player;
import org.bukkit.entity.Villager;
import org.bukkit.entity.Villager.Profession;
import org.bukkit.plugin.java.JavaPlugin;

/**
 * @author jrowlett
 *
 */
public class NPCTestPlugin extends JavaPlugin implements CommandExecutor {

    /**
     * lookup map of created entities.
     */
    private HashMap<Integer, Villager> entities =
        new HashMap<Integer, Villager>();

    @Override
    public final void onEnable() {
        this.getLogger().log(Level.INFO, "NPCTest is now enabled.");
        this.getCommand("npctest").setExecutor(this);
    }

    @Override
    public final void onDisable() {
        this.getLogger().log(Level.INFO, "NPCTest is now disabled.");
    }

    @Override
    public final boolean onCommand(
        final CommandSender sender,
        final Command command,
        final String label,
        final String[] args) {
        if (args.length < 1) {
            return false;
        }

        if (!(sender instanceof Player)) {
            return false;
        }

        Player player = (Player) sender;

        if ("create".equalsIgnoreCase(args[0])) {
            Location loc = player.getLocation();
            World world = loc.getWorld();
            Entity entity = world.spawnEntity(loc, EntityType.VILLAGER);
            Villager npc = (Villager) entity;
            this.entities.put(npc.getEntityId(), npc);
            npc.setCustomName("Bob");
            npc.setCustomNameVisible(true);
            npc.setProfession(Profession.FARMER);
            return true;
        }

        return false;
    }
}
