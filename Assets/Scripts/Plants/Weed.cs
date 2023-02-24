using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weed : PlantBaseClass
{
    // Start is called before the first frame update
    protected override void Start()
    {
        if (!_loaded_from_file)
        {
            _plant_type = Utils.PlantType.Weed;
            _current_plant_stage = Utils.PlantStage.Seed;
            _current_time_spent_growing = 0.0f;
            _interact_text = Utils.Constants.PLANT_NOT_RIPE_YET;
            SwitchToNextSprite();
            
            if (TimeManager.Instance != null && TimeManager.Instance.PlantManagerInstance() != null)
            {
                TimeManager.Instance.PlantManagerInstance().AddPlant(this);
            }
            // PlantManager isn't ready yet, so wait a bit and try again
            else
            {
                StartCoroutine(lateStart());
            }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public override void Grow()
    {
        if (_current_plant_stage == Utils.PlantStage.Ripe || !_active)
        {
            return;
        }
        
        _current_time_spent_growing += Time.deltaTime;
        _interact_text = Utils.Constants.PLANT_NOT_RIPE_YET + getPercentOfGrowth();
        if (CheckEnterNextStage())
        {
            EnterNextStage();
        }
    }

    public override void YieldPlant()
    {
        // TODO Destroy Plant and move object to inventory (or drop on ground, if inventory full)
    }

    public override void SwitchToNextSprite()
    {
        _current_sprite = _sprites_growing_stages[(int)_current_plant_stage];
        GetComponent<SpriteRenderer>().sprite = _current_sprite;
    }

    public override bool CheckEnterNextStage()
    {
        if (_current_plant_stage == Utils.PlantStage.Ripe)
        {
            return false;
        }

        int divider = _sprites_growing_stages.Count - 1;
        
        float cycle_length = _required_time_to_grow / divider;
        return _current_time_spent_growing >= (cycle_length * ((int)_current_plant_stage + 1));
    }

    public override void EnterNextStage()
    {
        _current_plant_stage += 1;
        SwitchToNextSprite();

        if (_current_plant_stage == Utils.PlantStage.Ripe)
        {
            _interact_text = Utils.Constants.PLANT_HARVEST;
        }
    }

    public override IEnumerator lateStart()
    {
        while (TimeManager.Instance == null || TimeManager.Instance.PlantManagerInstance() == null)
        {
            yield return new WaitForSeconds(0.25f);
        }
        TimeManager.Instance.PlantManagerInstance().AddPlant(this);
    }

    public override bool isRipe()
    {
        return _current_plant_stage == Utils.PlantStage.Ripe;
    }

    private String getPercentOfGrowth()
    {
        int percent = (int)Math.Round((_current_time_spent_growing / _required_time_to_grow) * 100.0f);
        return "(" + percent.ToString() + "%)";
    }
}
