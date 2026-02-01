using System.Text;
using System.Text.Json.Serialization;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerraScraper.Data;

public class ItemData
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("tooltip")]
    public string Tooltip { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("mod")]
    public string ModName { get; set; }

    private ItemData() { }

    public static ItemData GetItemData(Item item) => new ItemData()
    {
        Name = item.Name,
        Tooltip = GetTooltipString(item.ToolTip),
        Quantity = item.stack,
        ModName = item.ModItem?.Mod.Name ?? "Terraria"
    };

    public static ItemData GetItemDataOfTile(int tileId)
    {
        Item item = new Item();

        int itemId = TileLoader.GetItemDropFromTypeAndStyle(tileId);
        item.SetDefaults(itemId);

        return GetItemData(item);
    }

    private static string GetTooltipString(ItemTooltip tooltip)
    {
        StringBuilder tooltipBuilder = new StringBuilder();
        
        for (int i = 0; i < tooltip.Lines; i++)
        {
            tooltipBuilder.AppendLine(tooltip.GetLine(i));
        }

        return tooltipBuilder.ToString().TrimEnd();
    }
}