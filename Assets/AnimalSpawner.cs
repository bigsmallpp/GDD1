using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public AnimalScript chickenPrefab;
    public cowScript cowPrefab;
    public sheepScript sheepPrefab;
    public bool isStable = false;

    // Start is called before the first frame update
    void Start()
    {
        //Restore Chicken
        if(isStable && SceneLoader.Instance.getChickenState())
        {
            Vector2 chicknePos = SceneLoader.Instance.getChickenPos();
            Debug.Log("Init Chicken");
            Instantiate(chickenPrefab.gameObject, chicknePos, Quaternion.identity);
        }

        if(isStable && SceneLoader.Instance.cowAlive)
        {
            Vector2 cowPos = SceneLoader.Instance.getCowPos();
            Debug.Log("Init Cow");
            Instantiate(cowPrefab.gameObject, cowPos, Quaternion.identity);
        }

        if(isStable && SceneLoader.Instance.sheepAlive)
        {
            Vector2 sheepPos = SceneLoader.Instance.getSheepPos();
            Debug.Log("Init Sheep");
            Instantiate(sheepPrefab.gameObject, sheepPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
