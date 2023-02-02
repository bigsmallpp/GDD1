using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggMachine : MonoBehaviour
{
    public Egg eggPrefab;
    private Animator anim;
    private AnimalScript chicken;
    public bool layEgg_debug = false;

    private float secondsPerDay;
    public float pointInTime;
    public bool layedEgg = false;
    public float waitingTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponentInParent(typeof(Animator)) as Animator;
        chicken = gameObject.GetComponentInParent(typeof(AnimalScript)) as AnimalScript;
        getTimeToLayEgg();
        getRandomPointInTime();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(TimeManager.Instance._time_enabled)
        {
            waitingTimer += Time.deltaTime;
            if(waitingTimer >= pointInTime && !layedEgg) // Lay Egg after a random time
            {
                LayEgg();
                layedEgg = true;
            }
            if(waitingTimer >= secondsPerDay) // Wait remaining daytime
            {
                getRandomPointInTime();
                layedEgg = false;
                waitingTimer = 0;
            }

            if(layEgg_debug) // For debug purpose
            {
                LayEgg();
                layEgg_debug = false;
            }
        }
        
    }

    void LayEgg()
    {
        Vector2 position = transform.position;
        chicken.StopChicken();
        Instantiate(eggPrefab, position, Quaternion.identity);
    }

    void getTimeToLayEgg()
    {
        if(TimeManager.Instance != null)
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
}
