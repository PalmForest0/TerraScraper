using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Terraria;
using Terraria.ID;
using TerraScraper.Data;

namespace TerraScraper.Scrapers;

public sealed class RecipeScraper : ScraperBase
{
    public override string Folder { get; } = "Recipes";
    public override string Command { get; } = "recipes";
    public override string Description { get; } = "Scrapes all recips and saves them to a json file.";
    public override int UIButtonItemID { get; } = ItemID.WorkBench;

    public override IEnumerator RunScrape()
    {
        List<RecipeData> recipes = new();

        // Get recipe data for each recipe
        foreach (Recipe recipe in Main.recipe)
        {
            RecipeData data = RecipeData.GetRecipeData(recipe);

            if(data is not null)
                recipes.Add(data);
        }

        // Write the recipes of each mod to a seperate file
        foreach (var modRecipes in recipes.GroupBy(r => r.ModName))
        {
            Directory.CreateDirectory(FullSavePath);

            string json = JsonSerializer.Serialize(modRecipes.ToArray());
            File.WriteAllText(Path.Combine(FullSavePath, $"recipes_{modRecipes.Key.Replace(" ", "")}.json"), json);
        }

        yield break;
    }

    public override void PostScrape()
    {
        Main.NewText($"All recipes have been succesfully saved to '{FullSavePath}'", Color.LimeGreen);
        base.PostScrape();
    }
}