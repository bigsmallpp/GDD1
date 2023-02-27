public static class ItemDescriptions
{
private static string desc_wheat = 
@"Grows in:
Spring, Summer, Autumn

Growth Duration:
3 Days

Yields:
Wheat

Tips:
Gets bad in Winter.
Can be used as food for Chicken.
Wheat can be used as food for Cows and Sheep.";
    public static string GetDescriptionForStore(Item.ItemType type)
    {
        string desc = "";

        switch (type)
        {
            case Item.ItemType.carrot_seed:
                break;
            case Item.ItemType.cauliflower_seed:
                break;
            case Item.ItemType.wheat_seed:
                desc = desc_wheat;
                break;
            case Item.ItemType.chicken_upgrade:
                break;
            case Item.ItemType.cow_upgrade:
                break;
            case Item.ItemType.scissor:
                break;
            case Item.ItemType.sheep_upgrade:
                break;
            case Item.ItemType.bucket:
                break;
        }
        
        return desc;
    }
}
