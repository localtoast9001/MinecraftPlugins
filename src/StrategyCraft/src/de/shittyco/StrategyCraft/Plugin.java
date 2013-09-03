/**
 * @author jrowlett
 */
package de.shittyco.StrategyCraft;

import java.util.ResourceBundle;
import java.util.logging.Level;

import org.bukkit.plugin.java.JavaPlugin;

/**
 * @author jrowlett
 *
 */
public class Plugin extends JavaPlugin {
    /**
     * Resource bundle for this class.
     */
    private static ResourceBundle bundle = ResourceBundle.getBundle("Plugin");

    /**
     * Internal object model.
     */
    private Model model = new Model();

    /**
     * Handles normal commands.
     */
    private RootCommandExecutor commandExecutor;

    /**
     * Handles admin commands.
     */
    private RootCommandExecutor adminCommandExecutor;

    @Override
    public final void onEnable() {
        this.commandExecutor = new RootCommandExecutor(
            this.model,
            this.getServer());
        this.adminCommandExecutor = new RootCommandExecutor(
            this.model,
            this.getServer());
        this.getCommand("sc").setExecutor(this.commandExecutor);
        this.getCommand("scadmin").setExecutor(this.adminCommandExecutor);
        this.getLogger().log(Level.INFO, bundle.getString("OnEnableMessage"));
    }

    @Override
    public final void onDisable() {
        this.getLogger().log(Level.INFO, bundle.getString("OnDisableMessage"));
    }
}
