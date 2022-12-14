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

    private void Start()
    {
        // TODO Enable flashlight as gadget for nighttime
        _player_light.enabled = false;
    }

    public void UpdateLighting(Utils.LightingTransition target)
    {
        if (target == Utils.LightingTransition.Day)
        {
            return;
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
}
