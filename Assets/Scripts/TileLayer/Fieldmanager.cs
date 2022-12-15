using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fieldmanager : MonoBehaviour
{
    [SerializeField] TileBase plowed;
    [SerializeField] TileBase seeded;
    [SerializeField] Tilemap tilemap_;
    [SerializeField] PlantBaseClass plant;
    [SerializeField] GameObject plantPrefab;
    Dictionary<Vector2Int, GameObject> plants_;
    Dictionary<Vector2Int, TileBase> tiles_;

    private void Start()
    {
        tiles_ = new Dictionary<Vector2Int, TileBase>();
        plants_ = new Dictionary<Vector2Int, GameObject>();
    }

    public void Seed(Vector3Int pos)
    {
        if(!tiles_.ContainsKey((Vector2Int)pos) || plants_.ContainsKey((Vector2Int)pos))
        {
            return;
        }
        tilemap_.SetTile(pos, seeded);
        tiles_.Remove((Vector2Int)pos);
        plants_.Add((Vector2Int)pos, null);
    }
    public void Plow(Vector3Int pos)
    {
        if (plants_.ContainsKey((Vector2Int)pos))
        {
            return;
        }
        if (tiles_.ContainsKey((Vector2Int)pos))
        {
            return;
        }
        tilemap_.SetTile(pos, plowed);
        tiles_.Add((Vector2Int)pos, plowed);
    }

    public int CheckStatus(Vector3Int pos)
    {
        if(tiles_.ContainsKey((Vector2Int)pos))
        {
            return 1; //Return 1 if Plowed
        }
        if(plants_.ContainsKey((Vector2Int)pos))
        {
            return 2; //Return 2 if Seeded / Plants
        }
        return 0;
    }

    public GameObject GetPlantObj(Vector3Int pos)
    {
        return plants_[(Vector2Int)pos];
    }

    public void UpdateSeeds()
    {
        Dictionary<Vector2Int, GameObject> plants_current = plants_;
        foreach (var tile in plants_.ToArray())
        {
            if(tile.Value == null)
            {
                Vector3Int pos = new Vector3Int(tile.Key.x, tile.Key.y, 0);
                Vector3 poscenter = tilemap_.GetCellCenterWorld(pos);
                //Vector3 posWorld = tilemap_.CellToWorld(pos);
                
                Debug.Log("PosWorld x: " + poscenter.x + " y: " + poscenter.y + "\n");
                tilemap_.SetTile(pos, null);
                GameObject val = Instantiate(plantPrefab, poscenter, Quaternion.identity);
                Debug.Log("GameObject x: " + val.transform.position.x + " y: " + val.transform.position.y + "\n");
                plants_current[tile.Key] = val;
            }
        }
        plants_ = plants_current;
    }

    public void deleteEntry(Vector3Int key)
    {
        plants_.Remove((Vector2Int)key);
    }
}
