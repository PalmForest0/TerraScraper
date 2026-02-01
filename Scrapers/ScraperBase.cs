using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraScraper.Scrapers;

public abstract class ScraperBase
{
    public abstract string Folder { get; }
    public abstract string Command { get; }
    public abstract string Description { get; }
    public abstract int UIButtonItemID { get; }

    public string FullSavePath => Path.Combine(Path.Combine(TerraScraper.SavePath, Folder));

    /// <summary>Runs the setup method before the main scrape process. To run full scrape please use <see cref="ScrapeAll(CommandCaller)"/></summary>
    public virtual void PreScrape()
    {
        if (string.IsNullOrEmpty(Folder) || string.IsNullOrEmpty(Command))
            throw new ArgumentNullException($"{nameof(this.PreScrape)} couldn't execute because a Scraper property was null.");

        Directory.CreateDirectory(Folder);
        SoundEngine.PlaySound(SoundID.Duck);
    }

    /// <summary>Runs main scrape process. To run full scrape please use <see cref="ScrapeAll(CommandCaller)"/>.</summary>
    public virtual void RunScrape()
    {
    }

    /// <summary>Runs final cleanup after main scrape step. To run full scrape please use <see cref="ScrapeAll(CommandCaller)"/></summary>
    public virtual void PostScrape()
    {
        SoundEngine.PlaySound(SoundID.Unlock);
    }

    public void ScrapeAll()
    {
        // Try to run each step, if one fails, terminate
        if (!TryRun(PreScrape, $"Encountered an error while running the preparation step of {GetType().Name}. The scrape step will not run."))
            return;
        if(!TryRun(RunScrape, $"Encountered an error while running the scrape step of {GetType().Name}. The post-scrape step will not run."))
            return;
        TryRun(PostScrape, $"Encountered an error while running the post-scrape step of {GetType().Name}.");
    }

    private static bool TryRun(Action action, string errorMessage)
    {
        try
        {
            action();
            return true;
        }
        catch (Exception ex)
        {
            Main.NewText(errorMessage, Color.OrangeRed);
            return false;
        }
    }
}