using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Terraria;

namespace TerraScraper.Data;

public class RecipeData
{
    [JsonPropertyName("result")]
    public ItemData Result { get; set; }

    [JsonPropertyName("ingredients")]
    public ItemData[] Ingredients { get; set; }

    [JsonPropertyName("workstations")]
    public ItemData[] Workstations { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("mod")]
    public string ModName { get; set; }

    private RecipeData() { }

    public static RecipeData GetRecipeData(Recipe recipe)
    {
        if(recipe is null)
            return null;

        Item result = recipe.createItem;
        List<Item> ingredients = recipe.requiredItem;
        List<int> workstations = recipe.requiredTile;

        if (result is null || string.IsNullOrWhiteSpace(result.Name))
            return null;
        if (ingredients is null || ingredients.Count == 0)
            return null;

        return new RecipeData()
        {
            Result = ItemData.GetItemData(result),
            Ingredients = ingredients.Select(ItemData.GetItemData).ToArray(),
            Workstations = workstations.Select(ItemData.GetItemDataOfTile).ToArray(),
            Id = recipe.RecipeIndex,
            ModName = recipe.Mod?.Name ?? "Terraria"
        };
    }
}
