using System;

[Serializable]
public class DataStore
{
    public DataStore()
    {
        _player_lantern = false;
    }
    
    // TODO as needed (money, stamina etc.)
    public Utils.Season _current_season;
    public int _current_day_in_season;
    public float _current_seconds;
    public bool _player_lantern;
}
