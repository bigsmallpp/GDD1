using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fieldtransform
{
    public class SeedingLayer : TilemapLayer
    {
        public void Seed(Vector3 worldCoordinates, Seeds seed)
        {
            Vector3Int tileCoordinates = tilemap_.WorldToCell(worldCoordinates);
            if(seed.tile != null)
            {
                tilemap_.SetTile(tileCoordinates, seed.tile);
            }
        }
    }
}