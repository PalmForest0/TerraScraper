using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TerraScraper.UI;

namespace TerraScraper.Modules;

public class UILoader : ModSystem
{
    internal UserInterface UserInterface;
    internal ScraperUI ScraperUI;

    private GameTime _lastUpdateUiGameTime;

    public override void Load()
    {
        if (Main.dedServ) return;

        UserInterface = new UserInterface();
        ScraperUI = new ScraperUI();
        ScraperUI.Activate();
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _lastUpdateUiGameTime = gameTime;
        if (UserInterface?.CurrentState != null)
        {
            UserInterface.Update(gameTime);
        }

        if (Main.playerInventory)
            ShowUI();
        else
            HideUI();
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "TerraScraper: ScraperInterface",
                delegate
                {
                    if (_lastUpdateUiGameTime != null && UserInterface?.CurrentState != null)
                    {
                        UserInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                    }
                    return true;
                },
                InterfaceScaleType.UI));
        }
    }

    internal void ShowUI()
    {
        UserInterface?.SetState(ScraperUI);
    }

    internal void HideUI()
    {
        UserInterface?.SetState(null);
    }
}
