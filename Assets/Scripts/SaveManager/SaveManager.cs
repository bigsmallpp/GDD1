using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ImmersiveVRTools.Runtime.Common.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance = null;
    public static SaveManager Instance => _instance;

    private TimeManager _time_manager = null;
    private AnimalManager _animal_manager = null;
    private Fieldmanager _field_manager = null;
    private SceneLoader _scene_loader_ = null;
    private PlayerController _player = null;
    
    private bool _save_file_present = false;

    private AnimalsDataStoreWrapper _animals = new AnimalsDataStoreWrapper();
    private EggDataStoreWrapper _eggs = new EggDataStoreWrapper();
    private PlayerDataStore _player_data = new PlayerDataStore();
    private SceneLoaderDataStore _loader_store = new SceneLoaderDataStore();

    private string SAVE_DIR = "saves";
    private string SAVE_PATH = "saves/GameData";
    private string ANIMALS = "Animals.json";
    private string LOADER = "Loader.json";
    private string EGGS = "Eggs.json";
    private string PLAYER = "Player.json";
    
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
        if (_animal_manager.chickenAlive)
        {
            Debug.Log("Saving chicken at " + _animal_manager.GetChickenPos());
            Vector3 chicken_pos = _animal_manager.GetChickenPos();
            AnimalsDataStore chicken = new AnimalsDataStore(AnimalScript.AnimalType.chicken, chicken_pos.x, chicken_pos.y, chicken_pos.z);
            _animals.animals_ = new List<AnimalsDataStore>();
            _animals.animals_.Add(chicken);
            Debug.Log(_animals);
        }
        
        // Save eggs
        if (_animal_manager.egg_counter > 0)
        {
            _eggs.eggs_ = new List<EggDataStore>();
            
            foreach(Vector2 egg in _animal_manager.GetEggs())
            {
                _eggs.eggs_.Add(new EggDataStore(egg.x, egg.y));    
            }
            Debug.Log("Eggs: " + _eggs.eggs_);
        }
        
        // Save Player Stuff
        if (_player != null)
        {
            foreach (Item item in _player.GetPlayerInventory().GetItemList())
            {
                Vector3 player_pos = _player.gameObject.transform.position;
                ItemsDataStore item_save = new ItemsDataStore((int) item.itemType, item.amount, item.prize);
                _player_data._items.Add(item_save);
                _player_data._money = _player.currentMoney;
                _player_data._pos_x = player_pos.x;
                _player_data._pos_y = player_pos.y;
            }
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
        _eggs.eggs_.Clear();
    }

    private void WriteToFile(string data, string file)
    {
        Debug.Log("Data: " + data);
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
                Debug.Log("Set PlayerVariant to " + _loader_store._player_variant);
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
                }
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
}
