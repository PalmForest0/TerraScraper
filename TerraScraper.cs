using Terraria.ModLoader;
using TerraScraper.Modules;

namespace TerraScraper;

// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
public class TerraScraper : Mod
{
    public TerraScraper() : base()
    {
        ScraperLoader.Instance.LoadScrapers();
    }
}