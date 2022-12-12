using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantBaseClass : MonoBehaviour
{
    [Header("Growing Related Stuff")]
    [SerializeField] private float _required_time_to_grow;
    [SerializeField] private bool _is_enhanced;
    
    [Header("Sprites For Different Stages Of Growth (insert in order from low to high)")]
    [SerializeField] private List<Sprite> _sprites_growing_stages;

    [Header("Current Stage of Growth")]
    public Utils.PlantStage _current_plant_stage;
    
    private float _current_time_spent_growing;
    
    // If field is watered -> growth rate = 1
    // Too much/little water -> growth rate = 0.7;
    private float _growth_rate;

    public abstract void Grow();
    public abstract void YieldPlant();
    public abstract void SwitchToNextSprite();
    public abstract bool CheckEnterNextStage();
    public abstract bool EnterNextStage();
}
