using System;
using UnityEngine;

[Serializable]
public class TilesDataStore
{
    public TilesDataStore(Vector2Int tile_pos, Utils.TileStage stage)
    {
        pos_ = tile_pos;
        stage_ = stage;
    }
    
    public TilesDataStore(Vector2Int tile_pos, Utils.TileStage stage, Utils.PlantType type)
    {
        pos_ = tile_pos;
        stage_ = stage;
        seed_type_ = type;
    }
    
    public Vector2Int pos_;
    public Utils.TileStage stage_;
    public Utils.PlantType seed_type_;
}
