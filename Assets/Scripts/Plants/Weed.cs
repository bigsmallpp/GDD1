using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weed : PlantBaseClass
{
    // Start is called before the first frame update
    protected override void Start()
    {
        _current_plant_stage = Utils.PlantStage.Seed;
        _current_time_spent_growing = 0.0f;
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

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public override void Grow()
    {
        if (_current_plant_stage == Utils.PlantStage.Ripe)
        {
            return;
        }
        
        _current_time_spent_growing += Time.deltaTime;
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
            
        float cycle_length = _required_time_to_grow / 3.0f;
        return _current_time_spent_growing >= (cycle_length * ((int)_current_plant_stage + 1));
    }

    public override void EnterNextStage()
    {
        _current_plant_stage += 1;
        SwitchToNextSprite();
    }

    public override IEnumerator lateStart()
    {
        while (TimeManager.Instance == null || TimeManager.Instance.PlantManagerInstance() == null)
        {
            yield return new WaitForSeconds(0.25f);
        }
        TimeManager.Instance.PlantManagerInstance().AddPlant(this);
    }
}
