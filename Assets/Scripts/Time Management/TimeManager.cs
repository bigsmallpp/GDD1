using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance = null;

    [Header("Adjustable Values For Time/Season")]
    [SerializeField] private float _seconds_per_day;
    [SerializeField] private int _days_per_season;

    // [Header("Manager For Plant Growing Progress etc.")]
    // Manages stuff for plants
    // [SerializeField] private StateManager _state_manager;
    
    [Header("Timer In Our HUD")]
    // Timer in HUD
    [SerializeField] private GameObject _timer;
    
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
        }
    }

    private void IncreaseTime()
    {
        _game_data._current_seconds += Time.deltaTime;
        if (_game_data._current_seconds >= _seconds_per_day)
        {
            EndDay();
            _game_data._current_seconds = 0.0f;
        }
    }

    private void EndDay()
    {
        _time_enabled = false;
        ++_game_data._current_day_in_season;
        AdjustSeason();
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

    public TimeManager Instance()
    {
        return _instance;
    }
}
