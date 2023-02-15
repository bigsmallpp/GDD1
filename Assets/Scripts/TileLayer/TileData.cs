using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tile Data")]
[Serializable]
public class TileData : ScriptableObject
{
    public List<TileBase> tiles_;
    public bool plowable;
    public bool seedable;

}
