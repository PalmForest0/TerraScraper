using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TerraScraper.Utility;

namespace TerraScraper.Scrapers;

public sealed class ItemScraper : ScraperBase
{
    public override string Folder { get; } = "Items";
    public override string Command { get; } = "items";
    public override string Description { get; } = "Scrapes all the game's items for their textures and saves them as PNGs.";
    public override int UIButtonItemID { get; } = ItemID.Magiluminescence;

    public override void RunScrape()
    {
        for (int i = 0; i < ItemLoader.ItemCount; i++)
        {
            string itemName = Lang.GetItemNameValue(i);

            if (string.IsNullOrWhiteSpace(itemName))
                continue;

            // If the item ID is beyond the base game's items, it is a mod item
            if (i >= ItemID.Count)
            {
                // Load the mod item to get access to its mod name
                ModItem modItem = ItemLoader.GetItem(i);

                if (modItem is not null)
                    SaveItemTexture(i, itemName, modItem.Mod.Name);

                continue;
            }

            SaveItemTexture(i, itemName, "Terraria");
        }
    }

    public override void PostScrape()
    {
        Main.NewText($"All items have been succesfully saved to '{FullSavePath}'", Color.LimeGreen);
        base.PostScrape();
    }

    private void SaveItemTexture(int id, string name, string folder)
    {
        Directory.CreateDirectory(Path.Combine(FullSavePath, folder));

        // Create the file path and stream
        string path = Path.Combine(FullSavePath, folder, $"{FilenameHelper.ValidateFilename(name)}.png");
        using FileStream stream = File.Create(path);

        // Write the item texture to the stream
        Texture2D texture = GetItemFirstFrame(id);
        texture.SaveAsPng(stream, texture.Width, texture.Height);
    }

    private static Texture2D GetItemFirstFrame(int itemId)
    {
        if (!TextureAssets.Item[itemId].IsLoaded)
            Main.instance.LoadItem(itemId);

        Texture2D texture = TextureAssets.Item[itemId].Value;
        var animation = Main.itemAnimations[itemId];

        if (animation is not null)
        {
            // Determine the rectangle of the first animation frame
            int frameHeight = texture.Height / animation.FrameCount;
            Rectangle frameRect = new Rectangle(0, 0, texture.Width, frameHeight);

            // Extract the data of the first frame
            Color[] data = new Color[frameRect.Width * frameRect.Height];
            texture.GetData(0, frameRect, data, 0, data.Length);

            // Create a new texture for the first frame
            texture = new Texture2D(Main.graphics.GraphicsDevice, frameRect.Width, frameRect.Height);
            texture.SetData(data);
        }

        return texture;
    }
}