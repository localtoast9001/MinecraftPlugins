package test;

import static org.junit.Assert.*;

import java.io.File;

import de.shittyco.TemplesAndGods.*;

import org.junit.Test;
import org.bukkit.Bukkit;
import org.bukkit.configuration.file.*;

public class DefaultConfigTest {

	@Test
	public void testDefaultConfig() {
		MockServer server = new MockServer();
		Bukkit.setServer(server);
		
		File configFile = new File("config.yml");
		GodsSection.registerClass();
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
		assertEquals(4, redGodShrine.getBlessings().size());
		for (String key : redGodShrine.getBlessings().keySet()) {
			Trade value = redGodShrine.getBlessings().get(key);
			assertNotNull(value);
			assertTrue(value.getFavor() > 0);
			assertNotNull(value.getItem());
		}
	}

}
