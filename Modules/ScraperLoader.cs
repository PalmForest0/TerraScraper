using System.Collections.Generic;
using TerraScraper.Scrapers;

namespace TerraScraper.Modules;

public static class ScraperLoader
{
    public static List<ScraperBase> Scrapers { get; private set; }

    public static void LoadScrapers()
    {
        Scrapers =
        [
            new ItemScraper(),
            new TileScraper(),
            new RecipeScraper()
        ];
    }
}