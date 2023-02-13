using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapEnter : MonoBehaviour
{
    //public PlayerController player;
    public PlayerController.Direction direction;
    public float duration = 1f;
    public GameObject[] trigger;

    private PlayerController player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        if(SceneLoader.Instance.actualPos == SceneLoader.actualPosition.Leave_stable_pos)
        {
            duration = 0.5f;
            direction = PlayerController.Direction.Down;
        }
        else
        {
            //Don't deactivate when leaving stable
            foreach (GameObject entry in trigger)
            {
                entry.SetActive(false);
            }
        }

        if (SceneLoader.Instance.previousScene != 0)
        {
            player.TransitionWhenEnterScene(direction, duration);
        }
        StartCoroutine(ActivateTriggerAgain());
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }
    }

    IEnumerator ActivateTriggerAgain()
    {
        yield return new WaitForSeconds(1.2f);
        foreach (GameObject entry in trigger)
        {
            entry.SetActive(true);
        }
    }
}
