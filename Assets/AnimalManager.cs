using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;
    private AnimalScript chicken;
    private EggMachine eggMachine;
    public Egg egg_prefab;

    //Chicken boundaries
    private float leftBoundary = -5.2f;
    private float rightBoundary = -2.1f;
    private float topBoundary = 4.6f;
    private float bottomBoundary = 1.45f;

    public bool layEgg_debug = false;
    public bool chickenAlive = false;
    public bool chickenInit = false;
    public bool layedEgg = false;
    public int egg_counter = 0;
    //private bool hasFood_chicken = false;

    public bool cowHasMilk = true;
    public bool sheepHasWool = true;

    public float secondsPerDay = 42;
    public float pointInTime;
    
    public float waitingTimer = 0f;

    public bool checkForFood_Debug = false;

    //Save egg position
    private Dictionary<int, Vector2> _egg_positions;
    public 
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        _egg_positions = new Dictionary<int, Vector2>();
        //Get chicken
        initChicken();
        //getTimeToLayEgg();
        getRandomPointInTime();
        SaveManager.Instance.SetAnimalManager(this);
        SaveManager.Instance.LoadEggs();
    }

    private bool initChicken()
    {
        GameObject chickenObject = GameObject.FindGameObjectWithTag("Chicken");
        if (chickenObject != null)
        {
            chicken = chickenObject.GetComponent<AnimalScript>();
            eggMachine = chicken.GetComponent<EggMachine>();
            chickenInit = false;
            return true;
        }
        chickenInit = true;
        return false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(secondsPerDay == 42)
        {
            getTimeToLayEgg();
        }
        
        if(checkForFood_Debug)
        {
            checkAnimalsHaveFood();
            checkForFood_Debug = false;
        }
        chickenAlive = SceneLoader.Instance.getChickenState();
        if(chickenInit)
        {
            if(!initChicken())
            {
                //Debug.Log("Chicken not found!");
            }
        }
        if (TimeManager.Instance._time_enabled && chickenAlive)
        {
            waitingTimer += Time.deltaTime;
            if (waitingTimer >= pointInTime && !layedEgg) // Lay Egg after a random time
            {
                if (!chickenInit)
                {
                    eggMachine.LayEgg(); //Lay Egg at chicken
                    layedEgg = true;
                }
                else
                {
                    layEggOutOfSight();
                    Debug.Log("Save egg in store and calc position");
                    layedEgg = true;
                }
                
            }
            if (waitingTimer >= secondsPerDay) // Wait remaining daytime
            {
                getRandomPointInTime();
                layedEgg = false;
                waitingTimer = 0;
            }

            if (layEgg_debug) // For debug purpose
            {
                if (!chickenInit)
                {
                    eggMachine.LayEgg(); //Lay Egg at chicken
                    layedEgg = true;
                }
                else
                {
                    layEggOutOfSight();
                    //Debug.Log("Save egg in store and calc position");
                    layedEgg = true;
                }
            }
        }
    }
    void getTimeToLayEgg()
    {
        Debug.Log("Get Time to Lay Egg");
        if (TimeManager.Instance != null)
        {
            Debug.Log("Seconds per day");
            secondsPerDay = TimeManager.Instance.GetSecondsPerDay();
            Debug.Log(secondsPerDay);
        }
        /*Debug.Log("Seconds per day");
        secondsPerDay = TimeManager.Instance.GetSecondsPerDay();
        Debug.Log(secondsPerDay);*/
    }
    void getRandomPointInTime()
    {
        float offset = secondsPerDay / 4;
        pointInTime = Random.Range(offset, secondsPerDay - offset); //Maybe not instant after starting the new day
    }

    public void setChickenRespawned()
    {
        chickenInit = true;
    }

    public void safeEggPosition(int index, Vector2 position)
    {
        Vector2 existingPos = Vector2.zero;
        bool hasValue = _egg_positions.TryGetValue(index, out existingPos);
        if (!hasValue)
        {
            _egg_positions.Add(index, position);
        }
    }

    public void restoreEggs()
    {
        foreach (var egg in _egg_positions)
        {
            Instantiate(egg_prefab, egg.Value, Quaternion.identity);
        }
    }

    private void layEggOutOfSight()
    {
        Vector2 newPos = Vector2.zero;
        //calc random position
        float new_x = Random.Range(leftBoundary, rightBoundary);
        float new_y = Random.Range(topBoundary, bottomBoundary);
        newPos.x = new_x;
        newPos.y = new_y;
        egg_counter++;
        _egg_positions.Add(egg_counter, newPos);
    }

    /*public void setFoodState(bool state, AnimalScript.AnimalType type)
    { 
        switch(type)
        {
            case AnimalScript.AnimalType.chicken:
                hasFood_chicken = state;
                break;
            case AnimalScript.AnimalType.cow:
                break;
            case AnimalScript.AnimalType.sheep:
                break;
        }
    }*/

    public void checkAnimalsHaveFood()
    {
        //TODO: For all animals

        //check if chicken alive
        if(chickenAlive)
        {
            int chicken_food_state = SceneLoader.Instance.getContainerStateByType(AnimalScript.AnimalType.chicken);
            if (chicken_food_state != 1) //check if animal has food to eat at night
            {
                //Animal dies
                SceneLoader.Instance._chicken_state = false;
                //make death sprite :(
                if (SceneLoader.Instance.currentScene == SceneLoader.Scene.Stable && chicken != null)
                {
                    chicken.gameObject.SetActive(false);
                }
            }
            else
            {
                if (!SceneLoader.Instance.tryToEmptyContainer(AnimalScript.AnimalType.chicken))
                {
                    Debug.Log("Try to empty container: [FAILED]");
                }
            }
        }

        if(SceneLoader.Instance.cowAlive)
        {
            int cow_food_state = SceneLoader.Instance.getContainerStateByType(AnimalScript.AnimalType.cow);
            if (cow_food_state != 1) //check if animal has food to eat at night
            {
                //Animal dies
                SceneLoader.Instance.cowAlive = false;
                //make death sprite :(
                /*if (SceneLoader.Instance.currentScene == SceneLoader.Scene.Stable && cow != null)
                {
                    chicken.gameObject.SetActive(false);
                }*/
            }
            else
            {
                if (!SceneLoader.Instance.tryToEmptyContainer(AnimalScript.AnimalType.cow))
                {
                    Debug.Log("Try to empty container: [FAILED]");
                }
            }
        }

        if(SceneLoader.Instance.sheepAlive)
        {
            int cow_food_state = SceneLoader.Instance.getContainerStateByType(AnimalScript.AnimalType.sheep);
            if (cow_food_state != 1) //check if animal has food to eat at night
            {
                SceneLoader.Instance.sheepAlive = false;
            }
            else
            {
                if (!SceneLoader.Instance.tryToEmptyContainer(AnimalScript.AnimalType.sheep))
                {
                    Debug.Log("Try to empty container: [FAILED]");
                }
            }
        }
    }

    public Vector3 GetChickenPos()
    {
        return SceneLoader.Instance.getChickenPos();
    }

    public List<Vector2> GetEggs()
    {
        List<Vector2> eggs = new List<Vector2>();
        foreach (Vector2 pos in _egg_positions.Values)
        {
            eggs.Add(pos);
        }

        return eggs;
    }

    public void LoadEggPositions(List<EggDataStore> saved_eggs)
    {
        int index = 0;
        foreach(EggDataStore egg in saved_eggs)
        {
            _egg_positions.Add(index, new Vector2(egg._pos_x, egg._pos_y));
            index++;
        }
        egg_counter = index;

        // if (SceneLoader.Instance.currentScene == SceneLoader.Scene.Stable)
        // {
        //     restoreEggs();
        // }
    }

    public void RemoveEgg(Vector2 pos)
    {
        double x = Math.Round(pos.x, 2);
        double y = Math.Round(pos.y, 2);
        KeyValuePair<int, Vector2> egg_to_remove;
        bool found = false;

        foreach (KeyValuePair<int, Vector2> egg in _egg_positions)
        {
            Debug.Log(Math.Round(egg.Value.x, 2) + " " + Math.Round(egg.Value.y, 2) + "\t" + x + " " + y);
            if (Math.Round(egg.Value.x, 2) == x && Math.Round(egg.Value.y, 2) == y)
            {
                Debug.Log("Egg removed");
                found = true;
                egg_to_remove = egg;
                break;
            }
        }

        if (found)
        {
            _egg_positions.Remove(egg_to_remove.Key);
        }
    }

    public void handleSheep()
    {
        sheepScript sheep = GameObject.FindGameObjectWithTag("Sheep").GetComponent<sheepScript>();
        if (sheep != null)
        {
            sheep.switchWoolState();
        }
    }
}
