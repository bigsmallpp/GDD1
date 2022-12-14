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
    
    public enum LightingTransition
    {
        Day,
        Dawn,
        Night
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

        public static int PLAYABLE_HOURS_PER_DAY_DEFAULT = 12;
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

    public static float CalculateSecondsForLightTransition(float seconds_per_day)
    {
        float seconds_per_hour = seconds_per_day / Constants.PLAYABLE_HOURS_PER_DAY_DEFAULT;
        
        // Dawn transitions from 1600 -> 1800
        // Night transitions from 1800 -> 2000
        int duration_of_transition = 2;

        return seconds_per_hour * duration_of_transition;
    }

    public static LightingTransition GetTransition(float passed_seconds_current_day, float seconds_per_day)
    {
        LightingTransition transition = LightingTransition.Day;
        float seconds_per_hour = seconds_per_day / Constants.PLAYABLE_HOURS_PER_DAY_DEFAULT;
        int hours_passed_until_night_start = 10;
        int hours_passed_until_dawn_start = 8;

        if (passed_seconds_current_day >= seconds_per_hour * hours_passed_until_night_start)
        {
            transition = LightingTransition.Night;
        }
        else if (passed_seconds_current_day >= seconds_per_hour * hours_passed_until_dawn_start)
        {
            transition = LightingTransition.Dawn;
        }

        return transition;
    }
}
