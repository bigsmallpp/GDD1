using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] Tilemap targetMarker;
    [SerializeField] TileBase tile;
    public Vector3Int markedCellPos;
    Vector3Int oldCellPos;
    bool active;

    private void Update()
    {
        if (!active) 
        { 
            if(oldCellPos != null)
            {
                targetMarker.SetTile(oldCellPos, null);
            }
            return; 
        }
        targetMarker.SetTile(oldCellPos, null);
        targetMarker.SetTile(markedCellPos, tile);
        oldCellPos = markedCellPos;
    }

    internal void Show(bool selectable)
    {
        active = selectable;
        targetMarker.gameObject.SetActive(active);
    }
}
