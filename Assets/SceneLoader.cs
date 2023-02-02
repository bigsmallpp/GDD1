using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    //pos inside stable: 0, -4.31
    //pos outside stable: 0.967, -0.609
    private Vector2 _enter_stable_pos;
    private Vector2 _leave_stable_pos;
    public Vector2 current_position;

    private Dictionary<int, int> _container_states;

    void Awake()
    {
        current_position = new Vector2(-0.79f, -2.34f); //Start
        _enter_stable_pos = new Vector2(0.0f, -4.31f);
        _leave_stable_pos = new Vector2(0.967f, -0.609f);

        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _container_states = new Dictionary<int, int>();
    }

    private void updateCurrentPosition(Vector2 pos)
    {
        current_position = pos;
    }

    public void loadScene(int scene)
    {
        //TODO: Make instances of TimeManager etc and save stuff of player, states of plants
        switch(scene)
        {
            case 0:
            SceneManager.LoadScene("StartScene");
            break;
            
            case 1:
            //Debug.Log("Load Outside");
            updateCurrentPosition(_leave_stable_pos);
            SceneManager.LoadScene("SampleScene");
            break;
            
            case 2:
            //Debug.Log("Load Stable");
            updateCurrentPosition(_enter_stable_pos);
            SceneManager.LoadScene("Stable");
            break;
        }
    }

    public void safeContainerState(int index, int state)
    {
        int value = 0;
        bool hasValue = _container_states.TryGetValue(index, out value);
        if (hasValue)
        {
            _container_states[index] = state;
        }
        else
        {
            _container_states.Add(index, state);
        }
    }

    public int getContainerStateByIndex(int index)
    {
        int state;
        if(_container_states != null && _container_states.TryGetValue(index, out state))
        {
            return state;
        }
        return 2; //return invalid state
        /*else
        {
            //Debug.Log("No value to index found");
            return 2;
        }*/
    }
}
