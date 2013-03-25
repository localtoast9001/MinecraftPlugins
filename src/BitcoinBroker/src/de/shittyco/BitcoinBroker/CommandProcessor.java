/**
 *
 */
package de.shittyco.BitcoinBroker;

/**
 * Base class for all command processors used to encapsulate command
 * implementations.
 * @author jrowlett
 *
 */
public abstract class CommandProcessor {
    /**
     * The name of the command.
     */
    private String commandName;

    /**
     * The reference to the internal model of the plugin.
     */
    private Model modelRef;

    /**
     * Initializes a new instance of the CommandProcessor class.
     * @param command - the name of the command.
     * @param model - the reference to the plugin model.
     */
    protected CommandProcessor(final String command, final Model model) {
        this.commandName = command;
        this.modelRef = model;
    }

    /**
     * Gets the command name.
     * @return the name of the command.
     */
    public final String getCommand() {
        return this.commandName;
    }

    /**
     * Gets the model reference.
     * @return the model reference.
     */
    protected final Model getModel() {
        return this.modelRef;
    }
}
