/**
 * 
 */
package de.shittyco.BitcoinBroker;

/**
 * @author jrowlett
 *
 */
public abstract class CommandProcessor {
	private String command;
	private Model model;
	
	protected CommandProcessor(String command, Model model) {
		this.command = command;
		this.model = model;
	}
	
	public String getCommand() {
		return this.command;
	}
	
	protected Model getModel() { 
		return this.model;
	}
}
