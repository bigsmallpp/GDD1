using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance = null;
    public static TimeManager Instance => _instance;

    [Header("Adjustable Values For Time/Season")]
    [SerializeField] private float _seconds_per_day;
    [SerializeField] private int _days_per_season;

    [Header("Manager For Plant Growing Progress etc.")]
    // Manages stuff for plants
    [SerializeField] private PlantManager _plant_manager;
    [SerializeField] private Fieldmanager _field_manager;

    [Header("Manager For All Light Sources In The Scene")]
    [SerializeField] private LightManager _light_manager;
    
    
    [Header("Timer In Our HUD")]
    // Timer in HUD
    [SerializeField] private GameObject _timer;
    [SerializeField] private GameObject _day;
    [SerializeField] private SeasonSpriteHolder _season;
    
    [Header("Enable/Disable Time Progression")]
    public bool _time_enabled = false;

    // TODO Save/Load Data
    private DataStore _game_data = null;

    private Color _currentLightColor;
    private bool _dayEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        _game_data = new DataStore();
        TryLoadSaveFile();
        
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            SaveManager.Instance.SetTimeManager(this);
            StartDay();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_time_enabled)
        {
            IncreaseTime();
            _plant_manager.GrowPlants();
        }
    }

    private void IncreaseTime()
    {
        _game_data._current_seconds += Time.deltaTime;
        updateHUD(true);
        _light_manager.UpdateLighting(Utils.GetTransition(_game_data._current_seconds, _seconds_per_day));
        _currentLightColor = _light_manager.GetGlobalColor();
        if (!_game_data._player_lantern && _game_data._current_seconds >= _seconds_per_day)
        {
            EndDay();
            _game_data._current_seconds = 0.0f;
        }
        // Extend day by 2 hours
        else if(_game_data._current_seconds >= _seconds_per_day + (2 * _seconds_per_day / 12))
        {
            EndDay();
            _game_data._current_seconds = 0.0f;
        }
    }

    public void EndDay()
    {
        _time_enabled = false;
        updateHUD(false);
        _field_manager.UpdateSeeds();
        _dayEnded = true;
        //AnimalManager.Instance.checkAnimalsHaveFood(); //Check if animals have food to eat
        //AnimalManager.Instance.cowHasMilk = true;
        //AnimalManager.Instance.sheepHasWool = true;
        //StartDay(); //Start Day when collision with House Door
        //TODO: Reload scene and update season to change map
    }

    private void increaseDay()
    {
        ++_game_data._current_day_in_season;
        Debug.Log("Increase Day");
        AdjustSeason();
    }

    private void AdjustSeason()
    {
        if (_game_data._current_day_in_season > _days_per_season)
        {
            _game_data._current_day_in_season = 1;
            ++_game_data._current_season;
            _game_data._current_season = (Utils.Season)((int)_game_data._current_season % 4);
            SceneLoader.Instance.increaseSeason();
        }
    }

    public void StartDay()
    {
        _light_manager.TurnOffLanterns();
        _light_manager.SetLightToDaytime();
        _time_enabled = true;
        _dayEnded = false;
    }

    public void PauseTimeProgression()
    {
        _time_enabled = false;
    }

    public void UnpauseTimeProgression()
    {
        _time_enabled = true;
    }

    private void TryLoadSaveFile()
    {
        try
        {
            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }
            
            string json_text = File.ReadAllText("saves/" + Utils.Constants.SAVEFILE_NAME);
            DataStore loaded_data = JsonUtility.FromJson<DataStore>(json_text);
            _game_data = loaded_data;
        }
        catch (Exception a)
        {
            Debug.Log(a);
            CreateNewData();
        }
        
        updateHUD(false);
        // StartDay();
    }

    private void CreateNewData()
    {
        DataStore new_data = new DataStore();
        new_data._current_day_in_season = 1;
        new_data._current_season = Utils.Season.Spring;

        _game_data = new_data;
    }

    private void SaveDataToFile(bool reset_seconds = false)
    {
        SaveManager.Instance.Save();
        if (reset_seconds)
        {
            _game_data._current_seconds = 0.0f;
        }
        
        // Debug.Log("Trying to save data");

        try
        {
            string data = JsonUtility.ToJson(_game_data);
            File.WriteAllText("saves/" + Utils.Constants.SAVEFILE_NAME, data);
        }
        catch (Exception e)
        {
            Debug.Log("Couldn't save data to file\n" + e);
            throw;
        }

    }

    public PlantManager PlantManagerInstance()
    {
        return _plant_manager;
    }

    private void adjustDay()
    {
        String day = _game_data._current_day_in_season.ToString();
        _day.GetComponent<TextMeshProUGUI>().text = day;
    }

    private void adjustSeason()
    {
        _season.SetSeasonSprite(_game_data._current_season);
    }

    private void adjustTime()
    {
        _timer.GetComponent<TextMeshProUGUI>().text = Utils.ConvertSecondsToDaytime(_game_data._current_seconds, _seconds_per_day);
    }

    private void updateHUD(bool adjust_only_time)
    {
        adjustTime();
        
        if (!adjust_only_time)
        {
            adjustDay();
            adjustSeason();
        }
    }

    public float GetSecondsPerDay()
    {
        return _seconds_per_day;
    }

    public void pauseTime(bool pausing)
    {
        _time_enabled = pausing;
    }

    public void skipToNextDay()
    {
        if (!_dayEnded)
        {
            return;
        }

        increaseDay();
        AdjustSeason();
        updateHUD(false);
        _field_manager.UpdateSeeds();
        AnimalManager.Instance.checkAnimalsHaveFood(); //Check if animals have food to eat
        if(_game_data._current_day_in_season % 2 == 0)
        {
            AnimalManager.Instance.cowHasMilk = true;
            AnimalManager.Instance.sheepHasWool = true;
        }
        Debug.Log("You went to sleep. Starting a new Day!");
        StartDay(); //Start new Day

        // Only occurs when Game is reloaded (Continued)
        if (Chest.Instance != null)
        {
            Chest.Instance.SellItemsInChest();
        }
        
        SaveDataToFile(true);
        _game_data._current_seconds = 0.0f;
    }

    public void UpdateLightManager(LightManager light)
    {
        if (_light_manager == null)
        {
            _light_manager = light;
            _light_manager.SetGlobalColor(_currentLightColor);
            _light_manager.UpdateLighting(Utils.GetTransition(_game_data._current_seconds, _seconds_per_day));
            if (_dayEnded)
            {
                _light_manager.TurnOnLanterns();
            }
        }
    }

    public void UpdatePlantsPerScene(SceneLoader.Scene new_scene)
    {
        _plant_manager.UpdatePlantVisibility(new_scene);
    }

    public Fieldmanager GetFieldManager()
    {
        return _field_manager;
    }

    public bool CheckPlayerhasLantern()
    {
        return _game_data._player_lantern;
    }

    public void UnlockLantern()
    {
        _game_data._player_lantern = true;
    }
}
