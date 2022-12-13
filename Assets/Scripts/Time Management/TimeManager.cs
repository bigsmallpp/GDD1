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
    
    [Header("Timer In Our HUD")]
    // Timer in HUD
    [SerializeField] private GameObject _timer;
    [SerializeField] private GameObject _day;
    [SerializeField] private GameObject _season;
    
    [Header("Enable/Disable Time Progression")]
    public bool _time_enabled = false;
    
    // TODO Save/Load Data
    private DataStore _game_data = null;

    // Start is called before the first frame update
    void Start()
    {
        TryLoadSaveFile();

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
        if (_game_data._current_seconds >= _seconds_per_day)
        {
            EndDay();
            _game_data._current_seconds = 0.0f;
            
            // TODO Create Day-toDay transition here and then re-enable time again
        }
    }

    private void EndDay()
    {
        _time_enabled = false;
        ++_game_data._current_day_in_season;
        AdjustSeason();
        updateHUD(false);
    }

    private void AdjustSeason()
    {
        if (_game_data._current_day_in_season == _days_per_season)
        {
            _game_data._current_day_in_season = 0;
            ++_game_data._current_season;
            _game_data._current_season = (Utils.Season)((int)_game_data._current_season % 4);
        }
        
    }

    public void StartDay()
    {
        _time_enabled = true;
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
            DataStore loaded_data = JsonUtility.FromJson<DataStore>(Utils.Constants.SAVEFILE_NAME);
            _game_data = loaded_data;
        }
        catch (ArgumentException a)
        {
            Debug.Log(a);
            CreateNewData();
        }
        
        updateHUD(false);
        StartDay();
    }

    private void CreateNewData()
    {
        DataStore new_data = new DataStore();
        new_data._current_day_in_season = 0;
        new_data._current_season = Utils.Season.Spring;

        _game_data = new_data;
    }

    private void SaveDataToFile()
    {
        // TODO
    }

    public PlantManager PlantManagerInstance()
    {
        return _plant_manager;
    }

    private void adjustDay()
    {
        String day = "Day " + _game_data._current_day_in_season.ToString() + "/" + _days_per_season;
        _day.GetComponent<TextMeshProUGUI>().text = day;
    }

    private void adjustSeason()
    {
        String season = Utils.Constants.SEASONS[(int)_game_data._current_season];
        _season.GetComponent<TextMeshProUGUI>().text = season;
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
}
