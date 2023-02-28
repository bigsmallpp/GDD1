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

private static string desc_carrot = 
@"Grows in:
Spring, Summer

Growth Duration:
4 Days

Yields:
Carrot

Tips:
Gets bad in Winter.";

private static string desc_cauliflower = 
@"Grows in:
Summer, Autumn

Growth Duration:
4 Days

Yields:
Cauliflower

Tips:
Gets bad in Winter.";

private static string desc_chicken = 
@"Every Farmers best friend.

Food:
Wheat Seeds

Produces:
Egg

Tips:
Needs to be fed once a day.";

private static string desc_cow = 
@"Medium maintenance, medium reward farm animal.

Food:
Wheat

Produces:
Milk

Tips:
Needs to be fed once a day.
Can be milked once a day (requires a bucket).";

private static string desc_sheep = 
@"Medium maintenance, high reward farm animal.

Food:
Wheat

Produces:
Wool

Tips:
Needs to be fed once a day.
Can be sheared once a day (requires scissors).";

private static string desc_scissors = 
@"Very sharp, be careful.
Always carry with the tip facing downwards. Or was it upwards?

Allows you to shear sheep.
";

private static string desc_bucket = 
@"The swiss-knife amongst farming tools.

Can be used to carry ordinary things such as stashes of money or cocaine. Or some more unique stuff like milk.

Allows you to milk cows.
";

private static string desc_lamp = 
@"Maybe the dark after 20:00 isn't that scary anymore with proper illumination?

Light it up!
";

    public static string GetDescriptionForStore(Item.ItemType type)
    {
        string desc = "";

        switch (type)
        {
            case Item.ItemType.carrot_seed:
                desc = desc_carrot;
                break;
            case Item.ItemType.cauliflower_seed:
                desc = desc_cauliflower;
                break;
            case Item.ItemType.wheat_seed:
                desc = desc_wheat;
                break;
            case Item.ItemType.chicken_upgrade:
                desc = desc_chicken;
                break;
            case Item.ItemType.cow_upgrade:
                desc = desc_cow;
                break;
            case Item.ItemType.scissor:
                desc = desc_scissors;
                break;
            case Item.ItemType.sheep_upgrade:
                desc = desc_sheep;
                break;
            case Item.ItemType.bucket:
                desc = desc_bucket;
                break;
            case Item.ItemType.lamp:
                desc = desc_lamp;
                break;
        }
        
        return desc;
    }
}
