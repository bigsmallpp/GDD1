using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    string start = "StartScene";
    string outside = "SampleScene";
    string stable = "Stable";
    public enum SceneString {start, outside, stable};
    public enum Scene {Start = 0, Outside = 1, Stable = 2};
    public Scene currentScene;
    public SceneString currentSceneString;
    public static SceneLoader Instance;
    public AnimalScript chickenPrefab;

    //pos inside stable: 0, -4.31
    //pos outside stable: 0.967, -0.609
    private Vector2 _enter_stable_pos;
    private Vector2 _leave_stable_pos;
    public Vector2 current_position;
    private Vector2 _chicken_start_pos;
    public Vector2 chicken_door_position;
    public Vector2 chicken_cage_position;
    public bool enterStable = false;

    //Saving states
    private Dictionary<AnimalScript.AnimalType, int> _container_states;
    public bool _chicken_state = false;
    private Vector2 _chicken_pos;

    void Awake()
    {
        current_position = new Vector2(-0.79f, -2.34f); //Start
        _enter_stable_pos = new Vector2(0.0f, -4.31f);
        _leave_stable_pos = new Vector2(0.967f, -0.609f);
        _chicken_start_pos = new Vector2(chickenPrefab.startPositionX,chickenPrefab.startPositionY);
        _chicken_pos = _chicken_start_pos;
        chicken_cage_position = new Vector2(-3.7f, 5.4f);
        chicken_door_position = new Vector2(0.0f, 5.4f);

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
        _container_states = new Dictionary<AnimalScript.AnimalType, int>();
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
            currentScene = Scene.Start;
            currentSceneString = SceneString.start;
            SceneManager.LoadScene("StartScene");
            break;
            
            case 1:
            //Debug.Log("Load Outside");
            currentScene = Scene.Outside;
            currentSceneString = SceneString.outside;
            updateCurrentPosition(_leave_stable_pos);
            SceneManager.LoadScene("SampleScene");
            AnimalManager.Instance.setChickenRespawned();
            
            break;
            
            case 2:
            //Debug.Log("Load Stable");
            currentScene = Scene.Stable;
            currentSceneString = SceneString.stable;
            updateCurrentPosition(_enter_stable_pos);
            SceneManager.LoadScene("Stable");
            
            //AnimalManager.Instance.setChickenRespawned();
            break;
        }
    }

    public void safeContainerState(AnimalScript.AnimalType type, int state)
    {
        int value = 0;
        bool hasValue = _container_states.TryGetValue(type, out value);
        if (hasValue)
        {
            _container_states[type] = state;
        }
        else
        {
            _container_states.Add(type, state);
        }
    }

    public bool updateContainerState(AnimalScript.AnimalType type, int state)
    {
        int value = 0;
        bool hasValue = _container_states.TryGetValue(type, out value);
        if (hasValue)
        {
            _container_states[type] = state;
            return true;
        }
        return false;
    }

    public int getContainerStateByType(AnimalScript.AnimalType type)
    {
        int state;
        if(_container_states != null && _container_states.TryGetValue(type, out state))
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

    //Chicken bought/alive
    public void setChickenState(bool state)
    {
        _chicken_state = state;
    }

    public bool getChickenState()
    {
        return _chicken_state;
    }

    public void saveChickenPos(Vector2 position)
    {
        _chicken_pos = position;
    }

    public Vector2 getChickenPos()
    {
        return _chicken_pos;
    }

    public bool tryToEmptyContainer(AnimalScript.AnimalType type)
    {
        bool returnState = false;
        GameObject[] container = GameObject.FindGameObjectsWithTag("Food Container");
        foreach (GameObject item in container)
        {
            FoodContainer foodContainer = item.GetComponent<FoodContainer>();
            if (foodContainer.type == AnimalScript.AnimalType.chicken)
            {
                foodContainer.emptyContainer();
                return true;
            }
        }
        if(getContainerStateByType(type) == 1)
        {
            returnState = updateContainerState(type, 0);
        }
        return returnState;
    }
}
