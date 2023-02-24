using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int player_variant = 1;

    string start = "StartScene";
    string outside = "SampleScene";
    string stable = "Stable";
    string field = "Field";
    public enum SceneString {start, outside, stable, field};

    //I know this enum is a bit unnecessary
    public enum actualPosition { Enter_stable_pos = 0, Leave_stable_pos = 1, Enter_field_pos = 2, Leave_field_pos = 3 };

    public enum Scene {Start = 0, Outside = 1, Stable = 2, Field = 3, Shop = 4};
    public Scene currentScene;
    public actualPosition actualPos;
    public Scene previousScene = 0;
    public SceneString currentSceneString;
    public static SceneLoader Instance;
    public AnimalScript chickenPrefab;

    //pos inside stable: 0, -4.31
    //pos outside stable: 0.967, -0.609
    private Vector2 _enter_stable_pos;
    public Vector2 _leave_stable_pos;
    private Vector2 _enter_field_pos;
    public Vector2 _leave_field_pos;
    public Vector2 current_position;
    public Vector2 previous_position = Vector2.zero;
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
        current_position = new Vector2(-0.79f, -2.34f); //Startzz
        _enter_stable_pos = new Vector2(0.0f, -5.4f);
        _leave_stable_pos = new Vector2(1f, -0.55f);
        _chicken_start_pos = new Vector2(chickenPrefab.startPositionX,chickenPrefab.startPositionY);
        _chicken_pos = _chicken_start_pos;
        chicken_cage_position = new Vector2(-3.7f, 5.4f);
        chicken_door_position = new Vector2(0.0f, 5.4f);
        _enter_field_pos = new Vector2(-3f, 5.4f);
        _leave_field_pos = new Vector2(-3f, -5.5f);

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
        SaveManager.Instance.SetSceneLoader(this);
        _container_states = new Dictionary<AnimalScript.AnimalType, int>();
    }

    private void updateCurrentPosition(Vector2 pos, actualPosition actual)
    {
        actualPos = actual;
        previous_position = current_position;
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
            SceneManager.LoadScene("StartScreen");
            //Destroy all shit
            Destroy(gameObject);
            Destroy(AnimalManager.Instance.gameObject);
            break;
            
            case 1:
            //Debug.Log("Load Outside");
            // Stable -> Outside
            previousScene = currentScene;
            currentScene = Scene.Outside;
            currentSceneString = SceneString.outside;
            updateCurrentPosition(_leave_stable_pos, actualPosition.Leave_stable_pos);
            SaveManager.Instance.UpdatePlayerData();
            SaveManager.Instance.UpdateTilesData(GameManager.Instance.GetCropsManager().GetTiles(), (int) previousScene);
            SceneManager.LoadScene("SampleScene");
            AnimalManager.Instance.setChickenRespawned();
            TimeManager.Instance.UpdatePlantsPerScene(currentScene);
            break;
            
            case 2:
            //Debug.Log("Load Stable");
            // Outside -> Stable
            previousScene = currentScene;
            currentScene = Scene.Stable;
            currentSceneString = SceneString.stable;
            updateCurrentPosition(_enter_stable_pos, actualPosition.Enter_stable_pos);
            SaveManager.Instance.UpdatePlayerData();
            SaveManager.Instance.UpdateTilesData(GameManager.Instance.GetCropsManager().GetTiles(), (int) previousScene);
            SceneManager.LoadScene("Stable");
            TimeManager.Instance.UpdatePlantsPerScene(currentScene);
            //AnimalManager.Instance.setChickenRespawned();
            break;

            case 3:
            //Debug.Log("Load Outside");
            // Outside -> Field
            previousScene = currentScene;
            currentScene = Scene.Field;
            currentSceneString = SceneString.field;
            updateCurrentPosition(_enter_field_pos, actualPosition.Enter_field_pos);
            SaveManager.Instance.UpdatePlayerData();
            SaveManager.Instance.UpdateTilesData(GameManager.Instance.GetCropsManager().GetTiles(), (int) previousScene);
            SceneManager.LoadScene("Field");
            TimeManager.Instance.UpdatePlantsPerScene(currentScene);
            break;

            case 4:
            //Leave Field and load scene with different position
            // Field -> Outside
            //Debug.Log("Load Outside");
            previousScene = currentScene;
            currentScene = Scene.Outside;
            currentSceneString = SceneString.outside;
            updateCurrentPosition(_leave_field_pos, actualPosition.Leave_field_pos);
            SaveManager.Instance.UpdatePlayerData();
            SaveManager.Instance.UpdateTilesData(GameManager.Instance.GetCropsManager().GetTiles(), (int) previousScene);
            SceneManager.LoadScene("SampleScene");
            AnimalManager.Instance.setChickenRespawned();
            TimeManager.Instance.UpdatePlantsPerScene(currentScene);
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

    public void setPlayerVariant(int variant)
    {
        player_variant = variant;
    }
}
