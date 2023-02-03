using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;
    private AnimalScript chicken;
    private EggMachine eggMachine;
    public Egg egg_prefab;

    private float leftBoundary = -5.2f;
    private float rightBoundary = -2.1f;
    private float topBoundary = 4.6f;
    private float bottomBoundary = 1.45f;

    public bool layEgg_debug = false;
    public bool chickenAlive = false;
    public bool chickenInit = false;

    private float secondsPerDay;
    public float pointInTime;
    public bool layedEgg = false;
    public float waitingTimer = 0f;

    public int egg_counter = 0;

    //Save egg position
    private Dictionary<int, Vector2> _egg_positions;
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
        getTimeToLayEgg();
        getRandomPointInTime();
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
                    Debug.Log("Save egg in store and calc position");
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
        //TODO: calc random position
        float new_x = Random.Range(leftBoundary, rightBoundary);
        float new_y = Random.Range(topBoundary, bottomBoundary);
        newPos.x = new_x;
        newPos.y = new_y;
        egg_counter++;
        _egg_positions.Add(egg_counter, newPos);
    }
}
