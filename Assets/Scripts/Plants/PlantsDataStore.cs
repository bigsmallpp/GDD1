using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlantsDataStore
{
    public PlantsDataStore(float time_to_grow, float time_spent_growing, Item item, Utils.PlantStage stage, Utils.PlantType type,
                           int scene, Vector3 position, Vector3Int position_tile)
    {
        _required_time_to_grow = time_to_grow;
        _current_time_spent_growing = time_spent_growing;
        _item = item;
        _plant_type = (int) type;
        _current_stage = (int) stage;
        _plant_scene = scene;
        _pos_object = position;
        _pos_tilemap = position_tile;
    }
    
    public float _required_time_to_grow;
    public int _plant_type;
    public int _current_stage;
    public float _current_time_spent_growing;
    public Item _item;
    public int _plant_scene;
    public Vector3Int _pos_tilemap;
    public Vector3 _pos_object;
}
