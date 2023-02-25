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
        SaveManager.Instance.SetFieldManager(this);

        tiles_ = new Dictionary<Vector2Int, TileBase>();
        plants_ = new Dictionary<Vector2Int, GameObject>();
        SaveManager.Instance.LoadTilesFromFile();
        StartCoroutine(lateStart());

    }
    
    public IEnumerator lateStart()
    {
        while (GameManager.Instance == null || GameManager.Instance.GetPlantManager() == null ||
               GameManager.Instance.GetPlantManager()._plants == null)
        {
            yield return new WaitForSeconds(0.25f);
        }
        
        RestorePlantMap();
        RestoreTileMap();
    }

    public void Seed(Vector3Int pos)
    {
        if(!tiles_.ContainsKey((Vector2Int)pos) || plants_.ContainsKey((Vector2Int)pos))
        {
            return;
        }
        tilemap_.SetTile(pos, seeded);
        // tiles_.Remove((Vector2Int)pos);
        tiles_[(Vector2Int)pos] = seeded;
        // plants_.Add((Vector2Int)pos, null);
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
        if(tiles_.ContainsKey((Vector2Int)pos) && tiles_[(Vector2Int)pos] == plowed)
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
        List<Vector2Int> keys_to_remove = new List<Vector2Int>();
        foreach (var tile in tiles_)
        {
            if(tile.Value == seeded)
            {
                keys_to_remove.Add(tile.Key);
                Vector3Int pos = new Vector3Int(tile.Key.x, tile.Key.y, 0);
                Vector3 poscenter = tilemap_.GetCellCenterWorld(pos);
                //Vector3 posWorld = tilemap_.CellToWorld(pos);
                
                Debug.Log("PosWorld x: " + poscenter.x + " y: " + poscenter.y + "\n");
                tilemap_.SetTile(pos, null);
                GameObject val = Instantiate(plantPrefab, poscenter, Quaternion.identity);
                val.GetComponent<PlantBaseClass>().SetScene((int) SceneLoader.Instance.currentScene);
                val.GetComponent<PlantBaseClass>().SetTileMapPos(pos);
                GameManager.Instance.GetPlantManager().AddPlant(val.GetComponent<PlantBaseClass>());
                
                Debug.Log("GameObject x: " + val.transform.position.x + " y: " + val.transform.position.y + "\n");
                plants_.Add(tile.Key, val);
            }
        }

        foreach(Vector2Int tile in keys_to_remove)
        {
            tiles_.Remove(tile);
            SaveManager.Instance.RemoveTile(tile, (int) SceneLoader.Instance.currentScene);
        }

        UpdateSeedsField();
    }

    private void UpdateSeedsField()
    {
        List<Vector2Int> keys_to_remove = new List<Vector2Int>();
        foreach (var tile in SaveManager.Instance.GetTileStore().field_tiles_)
        {
            if(tile.stage_ == Utils.TileStage.Seeded)
            {
                keys_to_remove.Add(tile.pos_);
                Vector3Int pos = new Vector3Int(tile.pos_.x, tile.pos_.y, 0);
                Vector3 poscenter = tilemap_.GetCellCenterWorld(pos);
                
                // tilemap_.SetTile(pos, null);
                GameObject val = Instantiate(plantPrefab, poscenter, Quaternion.identity);
                val.GetComponent<PlantBaseClass>()._plant_type = Utils.PlantType.Weed;
                val.GetComponent<PlantBaseClass>().SetScene((int) SceneLoader.Scene.Field);
                val.GetComponent<PlantBaseClass>().SetTileMapPos(pos);
                val.GetComponent<PlantBaseClass>()._loaded_from_file = true;
                val.GetComponent<PlantBaseClass>()._current_plant_stage = Utils.PlantStage.Seed;
                val.GetComponent<PlantBaseClass>().SwitchToNextSprite();
                val.SetActive(false);
                GameManager.Instance.GetPlantManager().AddPlant(val.GetComponent<PlantBaseClass>(), (int) SceneLoader.Scene.Field);
                // plants_.Add(tile.pos_, val);
            }
        }

        foreach(Vector2Int tile in keys_to_remove)
        {
            SaveManager.Instance.RemoveTile(tile, (int) SceneLoader.Scene.Field);
        }
    }

    public void deleteEntry(Vector3Int key)
    {
        plants_.Remove((Vector2Int)key);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void SetMap(Tilemap map)
    {
        tilemap_ = map;
    }

    public Dictionary<Vector2Int, TileBase> GetTiles()
    {
        return tiles_;
    }
    
    public Dictionary<Vector2Int, GameObject> GetPlantTiles()
    {
        return plants_;
    }

    public void RestorePlantMap()
    {
        plants_ = new Dictionary<Vector2Int, GameObject>();
        List<PlantBaseClass> plants = GameManager.Instance.GetPlantManager().GetActivePlantsInScene();
        foreach(PlantBaseClass p in plants)
        {
            Vector2Int pos = (Vector2Int) p._pos_tilemap;
            plants_.Add(pos, p.gameObject);
            SaveManager.Instance.RemoveTile(pos, (int)SceneLoader.Instance.currentScene);
        }
    }
    
    public void RestoreTileMap()
    {
        tiles_ = new Dictionary<Vector2Int, TileBase>();
        List<TilesDataStore> saved_tile = SceneLoader.Instance.currentScene == SceneLoader.Scene.Outside ? 
                                          SaveManager.Instance.GetTileStore().outside_tiles_ : SaveManager.Instance.GetTileStore().field_tiles_;
        
        foreach(TilesDataStore t in saved_tile)
        {
            TileBase tile_base = t.stage_ == Utils.TileStage.Plowed ? plowed : seeded;
            tiles_.Add(t.pos_, tile_base);

            Vector3Int pos = new Vector3Int(t.pos_.x, t.pos_.y, 0);
            tilemap_.SetTile(pos, tile_base);
        }
    }
}
