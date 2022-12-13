using System;
using UnityEngine;

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
        public static String PLANT_NOT_RIPE_YET = "Plant is not ripe yet ";
        public static String PLANT_HARVEST = "Click to harvest";
        public static String[] SEASONS = new string[]{ "Spring", "Summer", "Autumn", "Winter" };
    }

    public static String ConvertSecondsToDaytime(float current_seconds, float max_seconds)
    {
        // Start 0800
        // End   2000
        // 360 seconds per day
        // 7,5 seconds = 15 min
        
        // Hours per day * quarter of an hour
        float seconds_per_quarter_of_an_hour = max_seconds / (12 * 4);
        
        int minutes_step = 15;
        int amount_steps = (int)(current_seconds / seconds_per_quarter_of_an_hour);

        int hour_increases = (int)(amount_steps / 4);

        int hour = 8 + hour_increases;
        int minutes = minutes_step * (amount_steps % 4);

        String prefix_hours = hour >= 10 ? "" : "0";
        String postfix_minutes = minutes > 0 ? "" : "0";
        
        String time = prefix_hours + hour.ToString() + ":" + minutes.ToString() + postfix_minutes;
        
        return time;
    }
}
