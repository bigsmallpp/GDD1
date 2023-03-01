using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ImmersiveVRTools.Runtime.Common.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance = null;
    public static SaveManager Instance => _instance;

    [SerializeField] private GameObject _pfWeed;
    [SerializeField] private TileBase _plowed;
    [SerializeField] private TileBase _seeded;

    private TimeManager _time_manager = null;
    private AnimalManager _animal_manager = null;
    private Fieldmanager _field_manager = null;
    private SceneLoader _scene_loader_ = null;
    private PlayerController _player = null;
    
    private bool _save_file_present = false;
    private bool _playerDataLocalSavePresent = false;
    private bool _firstLoadFromDisk = true;
    private bool _tilesDataLocalSavePresent = false;

    private AnimalsDataStoreWrapper _animals = new AnimalsDataStoreWrapper();
    private EggDataStoreWrapper _eggs = new EggDataStoreWrapper();
    private PlayerDataStore _player_data = new PlayerDataStore();
    private SceneLoaderDataStore _loader_store = new SceneLoaderDataStore();
    private PlantsDataStoreWrapper _plants = new PlantsDataStoreWrapper();
    private StoreDataStore _store_data = new StoreDataStore();
    [SerializeField] private TilesDataStoreWrapper _tiles = new TilesDataStoreWrapper();

    private string SAVE_DIR = "saves";
    private string SAVE_PATH = "saves/GameData";
    private string ANIMALS = "Animals.json";
    private string LOADER = "Loader.json";
    private string EGGS = "Eggs.json";
    private string PLAYER = "Player.json";
    private string PLANTS = "Plants.json";
    private string TILES = "Tiles.json";
    private string STORE = "Store.json";
    
    // Start is called before the first frame update
    void Start()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTimeManager(TimeManager manager)
    {
        _time_manager = manager;
    }
    
    public void SetFieldManager(Fieldmanager manager)
    {
        _field_manager = manager;
    }
    
    public void SetAnimalManager(AnimalManager manager)
    {
        _animal_manager = manager;
    }
    
    public void SetSceneLoader(SceneLoader loader)
    {
        _scene_loader_ = loader;
    }

    public void SetPlayer(PlayerController player)
    {
        _player = player;
    }
    
    public void Save()
    {
        // Save Animals and their positions
        if (_animal_manager.chickenAlive || SceneLoader.Instance.cowAlive || SceneLoader.Instance.sheepAlive)
        {
            SaveAnimalData();
        }
        
        // Save eggs
        if (_animal_manager.egg_counter > 0)
        {
            _eggs.eggs_ = new List<EggDataStore>();
            
            foreach(Vector2 egg in _animal_manager.GetEggs())
            {
                _eggs.eggs_.Add(new EggDataStore(egg.x, egg.y));    
            }
            Debug.Log("Eggs: " + _eggs.eggs_.ToArray());
        }
        
        // Save Player Stuff
        if (_player != null)
        {
            UpdatePlayerData();
        }
        
        // Save Plants
        if (GameManager.Instance.GetPlantManager()._plants.Count > 0)
        {
            SavePlants(GameManager.Instance.GetPlantManager()._plants);
        }
        
        // Save Tiles
        if (GameManager.Instance.GetCropsManager().GetTiles().Count > 0)
        {
            UpdateTilesData(GameManager.Instance.GetCropsManager().GetTiles(), (int) SceneLoader.Instance.currentScene);
        }
        
        try
        {   
            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }
            
            string data = JsonUtility.ToJson(_animals);
            WriteToFile(data, SAVE_PATH + ANIMALS);

            _loader_store._player_variant = _scene_loader_.player_variant;
            data = JsonUtility.ToJson(_loader_store);
            WriteToFile(data, SAVE_PATH + LOADER);

            data = JsonUtility.ToJson(_eggs);
            WriteToFile(data, SAVE_PATH + EGGS);
            
            data = JsonUtility.ToJson(_player_data);
            WriteToFile(data, SAVE_PATH + PLAYER);
            
            data = JsonUtility.ToJson(_plants);
            WriteToFile(data, SAVE_PATH + PLANTS);
            
            data = JsonUtility.ToJson(_tiles);
            WriteToFile(data, SAVE_PATH + TILES);
            
            data = JsonUtility.ToJson(_store_data);
            WriteToFile(data, SAVE_PATH + STORE);
        }
        catch (Exception e)
        {
            Debug.Log("Couldn't save data to file\n" + e);
            ResetBuffers();
            throw;
        }
        
        ResetBuffers();
    }

    private void ResetBuffers()
    {
        _animals.animals_.Clear();
        _player_data._items.Clear();
        _eggs.eggs_.Clear();
        _plants._plants.Clear();
    }

    private void WriteToFile(string data, string file)
    {
        // Debug.Log("Data: " + data);
        File.WriteAllText(file, data);
    }

    private void LoadData()
    {
        // Exists before SampleScene is loaded
        try
        {
            string json_text = "";
            
            // Load PlayerModel
            if (File.Exists(SAVE_PATH + LOADER))
            {
                json_text = File.ReadAllText(SAVE_PATH + LOADER);
                SceneLoaderDataStore loader_data = JsonUtility.FromJson<SceneLoaderDataStore>(json_text);
                _loader_store = loader_data;
                _scene_loader_.setPlayerVariant(_loader_store._player_variant);
            } 

            // Load Animals
            if (File.Exists(SAVE_PATH + ANIMALS))
            {
                json_text = File.ReadAllText(SAVE_PATH + ANIMALS);
                AnimalsDataStoreWrapper loaded_data = JsonUtility.FromJson<AnimalsDataStoreWrapper>(json_text);
                _animals = loaded_data;
                foreach(AnimalsDataStore anim in loaded_data.animals_)
                {
                    if (anim._type == (int)AnimalScript.AnimalType.chicken)
                    {
                        SceneLoader.Instance.setChickenState(true);
                        SceneLoader.Instance.saveChickenPos(new Vector2(anim._pos_x, anim._pos_y));
                    }
                    else if (anim._type == (int)AnimalScript.AnimalType.cow)
                    {
                        SceneLoader.Instance.cowAlive = true;
                        SceneLoader.Instance.cow_pos = new Vector2(anim._pos_x, anim._pos_y);
                    }
                    else if (anim._type == (int)AnimalScript.AnimalType.sheep)
                    {
                        SceneLoader.Instance.sheepAlive = true;
                        SceneLoader.Instance.sheep_pos = new Vector2(anim._pos_x, anim._pos_y);
                    }
                    else
                    {
                        Debug.LogError("Unknown Animal: " + nameof(anim._type));
                    }
                }
            }
            
            // Load Store Contents
            if (File.Exists(SAVE_PATH + STORE))
            {
                json_text = File.ReadAllText(SAVE_PATH + STORE);
                StoreDataStore loaded_data = JsonUtility.FromJson<StoreDataStore>(json_text);
                _store_data = loaded_data;
            }
        }
        catch (Exception a)
        {
            Debug.Log(a);
        }
    }

    public void LoadDataAndStartGame()
    {
        SceneManager.LoadScene("SampleScene");
        SceneLoader.Instance.currentScene = SceneLoader.Scene.Outside;
        LoadData();
    }

    public void LoadEggs()
    {
        // Exists after SampleScene was loaded
        try
        {
            // Load Eggs
            if (File.Exists(SAVE_PATH + EGGS))
            {
                string json_text = File.ReadAllText(SAVE_PATH + EGGS);
                EggDataStoreWrapper loaded_data = JsonUtility.FromJson<EggDataStoreWrapper>(json_text);
                _eggs = loaded_data;
                _animal_manager.LoadEggPositions(_eggs.eggs_);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public void LoadPlayerData()
    {
        // Used between Scene Transitions
        if (_playerDataLocalSavePresent)
        {
            _player.LoadPlayerData(_player_data, true);
            return;
        }

        if (_firstLoadFromDisk)
        {
            // Exists after SampleScene was loaded
            try
            {
                // Load Player
                if (File.Exists(SAVE_PATH + PLAYER))
                {
                    string json_text = File.ReadAllText(SAVE_PATH + PLAYER);
                    PlayerDataStore loaded_data = JsonUtility.FromJson<PlayerDataStore>(json_text);
                    _player_data = loaded_data;
                    _player.LoadPlayerData(_player_data);
                    _firstLoadFromDisk = false;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
    
    public void LoadPlantsFromFile()
    {
        try
        {
            if (File.Exists(SAVE_PATH + PLANTS))
            {
                string json_text = File.ReadAllText(SAVE_PATH + PLANTS);
                PlantsDataStoreWrapper loaded_data = JsonUtility.FromJson<PlantsDataStoreWrapper>(json_text);
                _plants = loaded_data;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }
    
    public void LoadTilesFromFile()
    {
        try
        {
            if (File.Exists(SAVE_PATH + TILES))
            {
                string json_text = File.ReadAllText(SAVE_PATH + TILES);
                TilesDataStoreWrapper loaded_data = JsonUtility.FromJson<TilesDataStoreWrapper>(json_text);
                _tiles = loaded_data;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public void ResetSaves()
    {
        try
        {
            if (Directory.Exists(SAVE_DIR))
            {
                Directory.Delete(SAVE_DIR, true);
                Directory.CreateDirectory(SAVE_DIR);
                Debug.Log("Deleted Directory");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public void UpdatePlayerData()
    {
        _player_data._items.Clear();
        foreach (Item item in _player.GetPlayerInventory().GetItems())
        {
            ItemsDataStore item_save = new ItemsDataStore((int) item.itemType, item.amount, item.prize);
            _player_data._items.Add(item_save);
        }
        
        Vector3 player_pos = _player.gameObject.transform.position;
        _player_data._money = _player.currentMoney;
        _player_data._pos_x = player_pos.x;
        _player_data._pos_y = player_pos.y;
        
        _playerDataLocalSavePresent = true;
    }

    public void UpdatePlayerMoneyAndInventory(int new_money_amount)
    {
        _player_data._items.Clear();
        foreach (Item item in _player.GetPlayerInventory().GetItems())
        {
            ItemsDataStore item_save = new ItemsDataStore((int) item.itemType, item.amount, item.prize);
            _player_data._items.Add(item_save);
        }

        _player_data._money = new_money_amount;
    }

    public int GetPlayerMoney()
    {
        return _player_data._money;
    }

    public Dictionary<int, List<PlantBaseClass>> LoadPlantsData()
    {
        LoadPlantsFromFile();
        PlantManager plant_manager = GameManager.Instance.GetPlantManager();
        
        Dictionary<int, List<PlantBaseClass>> plants = new Dictionary<int, List<PlantBaseClass>>();
        foreach(SceneLoader.Scene scene in Enum.GetValues(typeof(SceneLoader.Scene)))
        {
            plants.Add((int) scene, new List<PlantBaseClass>());
        }
        
        foreach (PlantsDataStore p in _plants._plants)
        {
            GameObject plant;
            switch(p._plant_type)
            {
                case (int) Utils.PlantType.Weed:
                    plant = Instantiate(_pfWeed, p._pos_object, Quaternion.identity, plant_manager.transform);
                    plant.GetComponent<PlantBaseClass>().RestoreValues(p);
                    plants[p._plant_scene].Add(plant.GetComponent<PlantBaseClass>());
                    break;
                
                case (int) Utils.PlantType.None:
                default:
                    Debug.LogError("No PlantType set");
                    break;
            }
        }

        return plants;
    }

    public void SavePlants(Dictionary<int, List<PlantBaseClass>> plants)
    {
        List<PlantsDataStore> saved_plants = new List<PlantsDataStore>();
        foreach(SceneLoader.Scene scene in Enum.GetValues(typeof(SceneLoader.Scene)))
        {
            foreach(PlantBaseClass p in plants[(int) scene])
            {
                PlantsDataStore saved_data = p.SavePlantDataInDataStore();
                saved_plants.Add(saved_data);
            }
        }

        _plants = new PlantsDataStoreWrapper(saved_plants);
    }

    public void SaveAnimalData()
    {
        _animals.animals_ = new List<AnimalsDataStore>();
        if (_animal_manager.chickenAlive)
        {
            Vector3 chicken_pos = _animal_manager.GetChickenPos();
            AnimalsDataStore chicken = new AnimalsDataStore(AnimalScript.AnimalType.chicken, chicken_pos.x, chicken_pos.y, chicken_pos.z);
            _animals.animals_.Add(chicken);
        }

        if (SceneLoader.Instance.cowAlive)
        {
            Vector3 cow_pos = SceneLoader.Instance.getCowPos();
            AnimalsDataStore cow = new AnimalsDataStore(AnimalScript.AnimalType.cow, cow_pos.x, cow_pos.y, cow_pos.z);
            _animals.animals_.Add(cow);
        }
            
        if (SceneLoader.Instance.sheepAlive)
        {
            Vector3 sheep_pos = SceneLoader.Instance.getSheepPos();
            AnimalsDataStore sheep = new AnimalsDataStore(AnimalScript.AnimalType.sheep, sheep_pos.x, sheep_pos.y, sheep_pos.z);
            _animals.animals_.Add(sheep);
        }
    }

    public void UpdateTilesData(Dictionary<Vector2Int, TileBase> tiles_, int scene)
    {
        _tilesDataLocalSavePresent = true;
        foreach(KeyValuePair<Vector2Int, TileBase> t in tiles_)
        {
            Utils.TileStage stage = t.Value == _plowed ? Utils.TileStage.Plowed : Utils.TileStage.Seeded;
            TilesDataStore saved_data = new TilesDataStore(t.Key, stage);

            if (scene == (int)SceneLoader.Scene.Outside)
            {
                if (!CheckDuplicate(saved_data, scene))
                {
                    _tiles.outside_tiles_.Add(saved_data);
                }
            }
            else if (scene == (int)SceneLoader.Scene.Field)
            {
                if (!CheckDuplicate(saved_data, scene))
                {
                    _tiles.field_tiles_.Add(saved_data);
                }
            }
            else
            {
                Debug.LogError("Unknown Scene");
            }
        }
    }

    public TilesDataStoreWrapper GetTileStore()
    {
        return _tiles;
    }

    private bool CheckDuplicate(TilesDataStore check, int scene)
    {
        if (scene == (int)SceneLoader.Scene.Outside)
        {
            foreach(TilesDataStore t in _tiles.outside_tiles_)
            {
                if (t.pos_ == check.pos_)
                {
                    if (t.stage_ == check.stage_)
                    {
                        return true;
                    }

                    t.stage_ = check.stage_;
                }
            }
        }
        else if (scene == (int)SceneLoader.Scene.Field)
        {
            foreach(TilesDataStore t in _tiles.field_tiles_)
            {
                if (t.pos_ == check.pos_)
                {
                    if (t.stage_ == check.stage_)
                    {
                        return true;
                    }

                    t.stage_ = check.stage_;
                }
            }
        }

        return false;
    }

    public void RemoveTile(Vector2Int pos, int scene)
    {
        List<TilesDataStore> tiles_temp = scene == (int)SceneLoader.Scene.Field ? _tiles.field_tiles_ : _tiles.outside_tiles_;
        List<TilesDataStore> tiles_to_remove = new List<TilesDataStore>();

        foreach (TilesDataStore t in tiles_temp)
        {
            if (t.pos_ == pos)
            {
                tiles_to_remove.Add(t);
            }
        }

        foreach (TilesDataStore t in tiles_to_remove)
        {
            tiles_temp.Remove(t);
        }

        _tiles.field_tiles_ = scene == (int)SceneLoader.Scene.Field ? tiles_temp : _tiles.field_tiles_;
        _tiles.outside_tiles_ = scene == (int)SceneLoader.Scene.Outside ? tiles_temp : _tiles.outside_tiles_;
    }

    public void Reset()
    {
        _time_manager = null;
        _animal_manager = null;
        _field_manager = null;
        _player = null;
        
        _save_file_present = false;
        _playerDataLocalSavePresent = false;
        _firstLoadFromDisk = true;
        _tilesDataLocalSavePresent = false;

        _animals = new AnimalsDataStoreWrapper();
        _eggs = new EggDataStoreWrapper();
        _player_data = new PlayerDataStore();
        _loader_store = new SceneLoaderDataStore();
        _plants = new PlantsDataStoreWrapper();
        _tiles = new TilesDataStoreWrapper();

        Destroy(GameManager.Instance.gameObject);
        Destroy(UIHandler.Instance.gameObject);
    }

    public void RemoveEgg(Vector2 pos)
    {
        EggDataStore egg_to_remove = null;
        foreach (EggDataStore egg in _eggs.eggs_)
        {
            Debug.Log(Math.Round(pos.x, 2).ToString() + " " + Math.Round(pos.x, 2).ToString() + "\t" + Math.Round(egg._pos_x, 2).ToString() + " " + Math.Round(egg._pos_y, 2).ToString());
            if (Math.Round(pos.x, 2) == Math.Round(egg._pos_x, 2) &&
                Math.Round(pos.y, 2) == Math.Round(egg._pos_y, 2))
            {
                egg_to_remove = egg;
                break;
            }
        }

        if (egg_to_remove == null)
        {
            Debug.LogError("No egg found");
            return;
        }

        _eggs.eggs_.Remove(egg_to_remove);
    }

    public StoreDataStore LoadStoreData()
    {
        return _store_data;
    }

    public void UpdateStoreData(StoreDataStore store)
    {
        _store_data = store;
    }

    public int getCurrentPlayerMoney()
    {
        return _player.currentMoney;
    }
    
    public void updateCurrentPlayerMoney(int money)
    {
        _player.currentMoney = money;
    }

    public void playerLose()
    {
        _player.LoseGame();
    }
    
    public void playerWin()
    {
        _player.WinGame();
    }
}
