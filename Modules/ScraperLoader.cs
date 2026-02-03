using System;
using System.Collections.Generic;
using System.IO;
using TerraScraper.Scrapers;

namespace TerraScraper.Modules;

public class ScraperLoader
{
    public static ScraperLoader Instance { get; } = new ScraperLoader();

    public string SavePath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TerraScraper");
    public List<ScraperBase> Scrapers { get; private set; }

    public bool LogScrapeProgress { get; set; } = true;
    
    private ScraperLoader() { }

    public void LoadScrapers()
    {
        Scrapers =
        [
            new ItemScraper(),
            new TileScraper(),
            new RecipeScraper()
        ];
    }
}