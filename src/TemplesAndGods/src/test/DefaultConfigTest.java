package test;

import static org.junit.Assert.*;

import java.io.File;
import java.util.*;

import de.shittyco.TemplesAndGods.*;
import org.junit.Test;
import org.bukkit.configuration.*;
import org.bukkit.configuration.file.*;


public class DefaultConfigTest {

	@Test
	public void testDefaultConfig() {
		File configFile = new File("config.yml");
		FileConfiguration file = YamlConfiguration.loadConfiguration(configFile);
		Model model = Model.load(file);
		assertNotNull(model);
		assertEquals(2, model.getGods().size());
		God redGod = model.getGods().get("redgod");
		assertNotNull(redGod);
		assertEquals("The Red God", redGod.getName());
		Shrine redGodShrine = redGod.getShrine();
		assertNotNull(redGodShrine);
		assertEquals(100, redGodShrine.getConstructionFavor());
	}

}
