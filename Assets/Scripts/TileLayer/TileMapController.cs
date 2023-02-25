using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour
{
    [SerializeField] Tilemap tilemap_;
    [SerializeField] List<TileData> tileData_;
    Dictionary<TileBase, TileData> dataOfTiles_;

    private void Start()
    {
        dataOfTiles_ = new Dictionary<TileBase, TileData>();
        foreach(TileData data in tileData_)
        {
            foreach(TileBase tile in data.tiles_)
            {
                if(tile != null)
                {
                    dataOfTiles_.Add(tile, data);
                }
            }
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && tilemap_ != null)
        {
            GetTileBase(Input.mousePosition);
        }
    }

    public Vector3Int GetGridPosition(Vector2 position)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        Vector3Int gridPos = tilemap_.WorldToCell(worldPosition);
        return gridPos;
    }

    public TileBase GetTileBase(Vector2 mousePosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int cellPos = tilemap_.WorldToCell(worldPosition);
        TileBase tile = tilemap_.GetTile(cellPos);
        return tile;
    }

    public TileData GetTileData(TileBase tile)
    {
        return dataOfTiles_[tile];
    }

    public void SetMap(Tilemap map)
    {
        tilemap_ = map;
    }

    public Tilemap GetTileMap()
    {
        return tilemap_;
    }
}
