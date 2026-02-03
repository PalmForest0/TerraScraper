using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TerraScraper.Modules;

namespace TerraScraper.Scrapers;

public abstract class ScraperBase
{
    public abstract string Folder { get; }
    public abstract string Command { get; }
    public abstract string Description { get; }
    public abstract int UIButtonItemID { get; }

    public string FullSavePath => Path.Combine(Path.Combine(ScraperLoader.Instance.SavePath, Folder));

    private bool success = true;

    /// <summary>Runs the setup method before the main scrape process. To run full scrape please use <see cref="ScrapeAll(CommandCaller)"/></summary>
    public virtual void PreScrape()
    {
        if (string.IsNullOrEmpty(Folder) || string.IsNullOrEmpty(Command))
            throw new ArgumentNullException($"{nameof(this.PreScrape)} couldn't execute because a Scraper property was null.");

        Directory.CreateDirectory(Folder);
        SoundEngine.PlaySound(SoundID.Duck);
    }

    /// <summary>Runs main scrape process. To run full scrape please use <see cref="ScrapeAll(CommandCaller)"/>.</summary>
    public abstract IEnumerator RunScrape();

    /// <summary>Runs final cleanup after main scrape step. To run full scrape please use <see cref="ScrapeAll(CommandCaller)"/></summary>
    public virtual void PostScrape()
    {
        SoundEngine.PlaySound(SoundID.Unlock);
    }

    public void ScrapeAll()
    {
        success = true;
        PreScrape();

        // If PreScrape failed, don't continue
        if (!success)
            return;

        CoroutineHandler.StartNew(RunScrape(), () =>
        {
            // If RunScrape failed, don't continue
            if (success)
                PostScrape();
        });
    }

    protected void FailCurrentStep(Exception ex, string context)
    {
        success = false;
        Main.NewText($"{context}:\n{ex.Message}", Color.OrangeRed);
    }

    protected string GetPercentageString(int current, int total) => $"{(float)current / total * 100:0.00}%";
}