using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using TerraScraper.Utility;

namespace TerraScraper.Scrapers;

public sealed class TileScraper : ScraperBase
{
    public override string Folder { get; } = "Tiles";
    public override string Command { get; } = "tiles";
    public override string Description { get; } = "Scrapes all the game's tiles for their textures and saves them as PNGs.";
    public override int UIButtonItemID { get; } = ItemID.DirtBlock;

    public override void RunScrape()
    {
        for (int i = 0; i < TileLoader.TileCount; i++)
        {
            ModTile modTile = TileLoader.GetTile(i);

            // Save vanilla tile in a different way
            if (modTile is null)
            {
                string name = GetTileDisplayName(i);
                SaveTileTexture(i, name, "Terraria");
                continue;
            }
            
            SaveTileTexture(i, modTile.Name, modTile.Mod.Name);
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
        string path = Path.Combine(FullSavePath, folder, $"{FilenameHelper.ValidateFilename(name)}.png");
        using FileStream stream = File.Create(path);

        Texture2D texture = TextureAssets.Tile[tileId].Value;
        texture.SaveAsPng(stream, texture.Width, texture.Height);
    }
}