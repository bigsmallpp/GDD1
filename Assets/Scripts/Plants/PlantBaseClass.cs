using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantBaseClass : MonoBehaviour
{
    [Header("Growing Related Stuff")]
    [SerializeField] protected float _required_time_to_grow;
    [SerializeField] protected List<Utils.Season> _growing_seasons;
    
    [Header("Sprites For Different Stages Of Growth (insert in order from low to high)")]
    [SerializeField] protected List<Sprite> _sprites_growing_stages;
    protected Sprite _current_sprite;

    [Header("Current Stage of Growth")]
    public Utils.PlantStage _current_plant_stage;
    
    [Header("Type of plant")]
    public Utils.PlantType _plant_type;
    
    [Header("Active Scene of plant")]
    public int _plant_scene;
    
    [Header("Position For The Tilemap")]
    public Vector3Int _pos_tilemap;
    
    [Header("The Interaction Text To Display")]
    [SerializeField] protected String _interact_text;

    [Header("The Item And Its Stats")]
    public Item _item;

    protected float _current_time_spent_growing;
    protected bool _clickable = false;
    
    // If field is watered -> growth rate = 1
    // Too much/little water -> growth rate = 0.7;
    protected float _growth_rate;
    protected bool _active = true;
    public bool _loaded_from_file = false;

    public abstract void Grow();
    public abstract void YieldPlant();
    public abstract void SwitchToNextSprite();
    public abstract bool CheckEnterNextStage();
    public abstract bool isRipe();
    public abstract void EnterNextStage();
    public abstract IEnumerator lateStart();

    public Item getItem()
    {
        return _item;
    }

    public bool getClickable()
    {
        return _clickable;
    }

    public String getInteractText()
    {
        return _interact_text;
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            _clickable = false;
        }
    }
    
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            _clickable = true;
        }
    }

    protected virtual void Start()
    {
    }
    
    protected virtual void Update()
    {
    }

    public void SetScene(int scene)
    {
        _plant_scene = scene;
    }

    public void SetActive(bool val)
    {
        _active = val;
    }

    public PlantsDataStore SavePlantDataInDataStore()
    {
        PlantsDataStore data = new PlantsDataStore(_required_time_to_grow, _current_time_spent_growing, _item,
                                                   _current_plant_stage, _plant_type, _plant_scene,
                                                   gameObject.transform.position, _pos_tilemap);
        return data;
    }

    public float GetTimeToGrow()
    {
        return _required_time_to_grow;
    }

    public float GetTimeSpentGrowing()
    {
        return _current_time_spent_growing;
    }

    public void SetTileMapPos(Vector3Int pos)
    {
        _pos_tilemap = pos;
    }

    public void RestoreValues(PlantsDataStore saved_data)
    {
        gameObject.transform.position = saved_data._pos_object;
        _plant_scene = saved_data._plant_scene;
        _current_plant_stage = (Utils.PlantStage) saved_data._current_stage;
        _current_time_spent_growing = saved_data._current_time_spent_growing;
        _required_time_to_grow = saved_data._required_time_to_grow;
        _plant_type = (Utils.PlantType) saved_data._plant_type;
        _pos_tilemap = saved_data._pos_tilemap;
        _item = saved_data._item;

        _loaded_from_file = true;
        _interact_text = _current_plant_stage == Utils.PlantStage.Ripe ? Utils.Constants.PLANT_HARVEST : Utils.Constants.PLANT_NOT_RIPE_YET;
        SwitchToNextSprite();
    }
}
