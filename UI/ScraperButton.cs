using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using TerraScraper.Scrapers;

namespace TerraScraper.UI;

public class ScraperButton : UIPanel
{
    private readonly ScraperBase scraper;
    private readonly Texture2D iconTexture;

    public ScraperButton(ScraperBase scraper)
    {
        this.scraper = scraper;
        iconTexture = Main.Assets.Request<Texture2D>($"Images/Item_{Math.Clamp(scraper.UIButtonItemID, 0, ItemLoader.ItemCount - 1)}", AssetRequestMode.ImmediateLoad).Value;

        CreateButton();
        OnLeftClick += (_, _) => scraper.ScrapeAll();
    }

    private void CreateButton()
    {
        Width.Set(50, 0);
        Height.Set(50, 0);
        HAlign = 0.5f;
        SetPadding(9);

        UIImage icon = new UIImage(iconTexture);
        icon.HAlign = 0.5f;
        icon.VAlign = 0.5f;
        icon.Width.Set(0, 1);
        icon.Height.Set(0, 1);
        Append(icon);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        // If this code is in the panel or container element, check it directly
        if (ContainsPoint(Main.MouseScreen))
            Main.LocalPlayer.mouseInterface = true;

        // Otherwise, we can check a child element instead
        if (ContainsPoint(Main.MouseScreen))
            Main.LocalPlayer.mouseInterface = true;

        // Hover text tooltip
        if (IsMouseHovering)
            Main.hoverItemName = $"Scrape {scraper.Command}";
    }
}