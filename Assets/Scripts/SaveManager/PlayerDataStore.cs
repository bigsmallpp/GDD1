using System;
using System.Collections.Generic;

[Serializable]
public class PlayerDataStore
{
    public PlayerDataStore()
    {
        _items = new List<ItemsDataStore>();
    }
    
    public PlayerDataStore(List<ItemsDataStore> items, int curr_energy, int max_energy, int money, float x, float y)
    {
        _items = items;
        _current_energy = curr_energy;
        _max_energy = max_energy;
        _money = money;
        _pos_x = x;
        _pos_y = y;
    }
    
    public List<ItemsDataStore> _items;
    public int _current_energy;
    public int _max_energy;
    public int _money;
    public float _pos_x;
    public float _pos_y;
}
