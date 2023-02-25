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
    
    public Vector2Int pos_;
    public Utils.TileStage stage_;
}
