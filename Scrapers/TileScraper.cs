using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using TerraScraper.Modules;
using TerraScraper.Utility;

namespace TerraScraper.Scrapers;

public sealed class TileScraper : ScraperBase
{
    public override string Folder { get; } = "Tiles";
    public override string Command { get; } = "tiles";
    public override string Description { get; } = "Scrapes all the game's tiles for their textures and saves them as PNGs.";
    public override int UIButtonItemID { get; } = ItemID.DirtBlock;

    private readonly int imagesToSavePerFrame = 4;

    public override IEnumerator RunScrape()
    {
        for (int i = 0; i < TileLoader.TileCount; i++)
        {
            ModTile modTile = TileLoader.GetTile(i);
            string tileName = modTile?.Name ?? GetTileDisplayName(i);
            string modName = modTile?.Mod.Name ?? "Terraria";

            try
            {
                SaveTileTexture(i, tileName, modName);
            }
            catch (Exception ex)
            {
                FailCurrentStep(ex, $"Failed to save tile texture for {tileName} ({i})");
                yield break;
            }

            // Limit how many items are processed per frame to avoid freezing
            if (i % imagesToSavePerFrame == 0)
                yield return null;
        }
    }

    public override void PostScrape()
    {
        Main.NewText($"All tile textures have been succesfully saved to '{FullSavePath}'", Color.LimeGreen);
        base.PostScrape();
    }

    private static string GetTileDisplayName(int tileId)
    {
        ModTile modTile = TileLoader.GetTile(tileId);

        if (modTile is not null)
            return modTile.Name;

        // Lookup vanilla tile name through map object
        Tile tile = new Tile();
        tile.ResetToType((ushort)tileId);

        int style = tile.TileFrameX / 18; // styles are stored in 18px columns
        int lookup = MapHelper.TileToLookup(tileId, style);
        string name = Lang.GetMapObjectName(lookup);

        if (!string.IsNullOrWhiteSpace(name))
            return name;

        // Fallback to default style if no specific style name found
        lookup = MapHelper.TileToLookup(tileId, 0);
        return Lang.GetMapObjectName(lookup);
    }


    private void SaveTileTexture(int tileId, string name, string folder)
    {
        Directory.CreateDirectory(Path.Combine(FullSavePath, folder));

        if (!TextureAssets.Tile[tileId].IsLoaded)
            Main.instance.LoadTiles(tileId);

        // Create the file path and stream
        string validated = FilenameHelper.ValidateFilename(name);
        string filename = $"{tileId}{(string.IsNullOrWhiteSpace(validated) ? "" : $"_{validated}")}";
        string path = Path.Combine(FullSavePath, folder, $"{filename}.png");

        // Create a filestream and save the texture as a PNG
        using FileStream stream = File.Create(path);
        Texture2D texture = TextureAssets.Tile[tileId].Value;
        texture.SaveAsPng(stream, texture.Width, texture.Height);

        if (ScraperLoader.Instance.LogScrapeProgress)
            Main.NewText($"[{GetPercentageString(tileId, TileLoader.TileCount - 1)}] Saved item texture: {name} ({tileId}) to {path}.", Color.LightGreen);
    }
}