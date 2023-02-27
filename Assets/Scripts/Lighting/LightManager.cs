using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    [Header("All Light Sources In The Scene")]
    [SerializeField] private Light2D _global_light;
    [SerializeField] private List<Light2D> _lanterns;
    [SerializeField] private Light2D _player_light;

    [Header("The Light Colors")]
    [SerializeField] private Color _default_lighting;
    [SerializeField] private Color _night_lighting;
    [SerializeField] private Color _dawn_lighting;

    private float _seconds_per_transition = 0.0f;
    private float _seconds_left_current_transition = 0.0f;
    private bool _lanterns_turned_on;

    private void Start()
    {
        // TODO Enable flashlight as gadget for nighttime
        _player_light.enabled = false;
        
        // Might be null on first scene load
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.UpdateLightManager(this);
        }
    }

    public void UpdateLighting(Utils.LightingTransition target)
    {
        if (target == Utils.LightingTransition.Day)
        {
            return;
        }

        if (CheckTransitionNight(target) && !_lanterns_turned_on)
        {
            TurnOnLanterns();
        }

        if (_seconds_per_transition == 0.0f)
        {
            _seconds_per_transition = Utils.CalculateSecondsForLightTransition(TimeManager.Instance.GetSecondsPerDay());
        }

        if (_seconds_left_current_transition == 0.0f)
        {
            _seconds_left_current_transition = _seconds_per_transition;
        }
        
        Color color_target = target == Utils.LightingTransition.Dawn ? _dawn_lighting : _night_lighting;
        _global_light.color = Color.Lerp(_global_light.color, color_target, Time.deltaTime / _seconds_left_current_transition);

        _seconds_left_current_transition -= Time.deltaTime;
        if (_seconds_left_current_transition < 0.1f)
        {
            _seconds_left_current_transition = 0.0f;
        }

        // TODO Enable Lanterns and Playerlights
    }

    public void SetLightToDaytime()
    {
        _global_light.color = _default_lighting;
    }

    public void TurnOffLanterns()
    {

        if (TimeManager.Instance.CheckPlayerhasLantern())
        {
            _player_light.enabled = false;
        }
        
        foreach (Light2D light in _lanterns)
        {
            if(light != null)
            {
                light.enabled = false;
            }
        }

        _lanterns_turned_on = false;
    }
    
    public void TurnOnLanterns()
    {
        if (TimeManager.Instance.CheckPlayerhasLantern())
        {
            _player_light.enabled = true;
        }
        
        foreach (Light2D light in _lanterns)
        {
            if(light != null)
            {
                light.enabled = true;
            }
        }

        _lanterns_turned_on = true;
    }

    private bool CheckTransitionNight(Utils.LightingTransition transition)
    {
        return transition == Utils.LightingTransition.Night;
    }

    public Color GetGlobalColor()
    {
        return _global_light.color;
    }

    public void SetGlobalColor(Color color)
    {
        _global_light.color = color;
    }
}
