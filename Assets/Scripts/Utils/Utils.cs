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
    }
}
