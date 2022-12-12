using System;
public static class Utils
{
    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    };
    
    public enum PlantStage
    {
        Seed,
        Sprout,
        Ripe
    };
    
    public static class Constants
    {
        public static String SAVEFILE_NAME = "save.json";
    }
}
