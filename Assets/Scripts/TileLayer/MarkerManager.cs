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

    private void Update()
    {
        targetMarker.SetTile(oldCellPos, null);
        targetMarker.SetTile(markedCellPos, tile);
        oldCellPos = markedCellPos;
    }
}
