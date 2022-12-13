using System;
public static class Utils
{
    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }
    
    public enum PlantStage
    {
        Seed,
        Sprout,
        Ripe
    }
    
    public enum RequestType
    {
        Add,
        Remove
    }

    public struct Request
    {
        public PlantBaseClass _plant;
        public RequestType _type;
    }
    
    public static class Constants
    {
        public static String SAVEFILE_NAME = "save.json";
        public static String PLANT_NOT_RIPE_YET = "Plant is not ripe yet...";
        public static String PLANT_HARVEST = "Press E to harvest";
    }
}
