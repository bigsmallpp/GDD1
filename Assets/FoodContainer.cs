using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodContainer : MonoBehaviour
{
    [SerializeField] protected List<Sprite> _sprites_fill_stages;

    protected Sprite _current_sprite;
    public bool _filled;
    protected int _stage = 0;
    //public int index;
    public AnimalScript.AnimalType type;

    //TODO: needs to save state
    void Start()
    {
        //Get safed state
        int try_get_state = SceneLoader.Instance.getContainerStateByType(type);
        if (try_get_state == 1)
        {
            switchState();
        }

        _current_sprite = _sprites_fill_stages[_stage];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void switchState()
    {
        if (_filled)
        {
            _stage = 0;
        }
        else
        {
            _stage = 1;
        }
        _current_sprite = _sprites_fill_stages[_stage];
        GetComponent<SpriteRenderer>().sprite = _current_sprite;
        _filled = !_filled;
        SceneLoader.Instance.safeContainerState(type, _stage);
    }

    public void fillContainer()
    {
        if(_stage == 0)
        {
            //Debug.Log("Container filled");
            switchState();
            //AnimalManager.Instance.setFoodState(true, type);
        }
    }

    public bool emptyContainer()
    {
        if(_stage == 1)
        {
            //Debug.Log("Empty Container");
            switchState();
            //AnimalManager.Instance.setFoodState(false, type);
            return true;
        }
        return false;
    }
}
