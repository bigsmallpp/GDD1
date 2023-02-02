using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodContainer : MonoBehaviour
{
    [SerializeField] protected List<Sprite> _sprites_fill_stages;

    protected Sprite _current_sprite;
    protected bool _filled;
    protected int _stage = 0;
    public int index;

    //TODO: needs to save state
    void Start()
    {
        //Get safed state
        int try_get_state = SceneLoader.Instance.getContainerStateByIndex(index);
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
        SceneLoader.Instance.safeContainerState(index, _stage);
    }

    public void fillContainer()
    {
        if(_stage == 0)
        {
            //Debug.Log("Container filled");
            switchState();
        }
    }

    public void emptyContainer()
    {
        if(_stage == 1)
        {
            //Debug.Log("Empty Container");
            switchState();
        }
    }
}
