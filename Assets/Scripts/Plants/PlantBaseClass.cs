using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantBaseClass : MonoBehaviour
{
    [Header("Growing Related Stuff")]
    [SerializeField] protected float _required_time_to_grow;
    [SerializeField] protected bool _is_enhanced;
    
    [Header("Sprites For Different Stages Of Growth (insert in order from low to high)")]
    [SerializeField] protected List<Sprite> _sprites_growing_stages;
    protected Sprite _current_sprite;

    [Header("Current Stage of Growth")]
    public Utils.PlantStage _current_plant_stage;
    
    [Header("The Interaction Text To Display")]
    [SerializeField] private string _interact_text;

    [Header("The Item And Its Stats")]
    public Item _item;
    
    protected float _current_time_spent_growing;
    protected bool _clickable = false;
    
    // If field is watered -> growth rate = 1
    // Too much/little water -> growth rate = 0.7;
    protected float _growth_rate;

    public abstract void Grow();
    public abstract void YieldPlant();
    public abstract void SwitchToNextSprite();
    public abstract bool CheckEnterNextStage();
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
}
