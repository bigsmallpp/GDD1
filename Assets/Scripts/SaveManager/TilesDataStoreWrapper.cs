using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TilesDataStoreWrapper
{
    public TilesDataStoreWrapper(List<TilesDataStore> tile_map, int scene)
    {
        if (scene == (int)SceneLoader.Scene.Outside)
        {
            outside_tiles_ = tile_map;
        }
        else if (scene == (int)SceneLoader.Scene.Field)
        {
            field_tiles_ = tile_map;
        }
        else
        {
            Debug.LogError("Unknown Scene " + scene + " in SaveManager");
        }
    }

    public TilesDataStoreWrapper()
    {
        outside_tiles_ = new List<TilesDataStore>();
        field_tiles_ = new List<TilesDataStore>();
    }

    public List<TilesDataStore> outside_tiles_;
    public List<TilesDataStore> field_tiles_;
}
