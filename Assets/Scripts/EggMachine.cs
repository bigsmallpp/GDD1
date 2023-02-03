using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggMachine : MonoBehaviour
{
    public Egg eggPrefab;
    private Animator anim;
    private AnimalScript chicken;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponentInParent(typeof(Animator)) as Animator;
        chicken = gameObject.GetComponentInParent(typeof(AnimalScript)) as AnimalScript;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
    }

    public void LayEgg()
    {
        Vector2 position = transform.position;
        chicken.StopChicken();
        Instantiate(eggPrefab, position, Quaternion.identity);
        int counter = AnimalManager.Instance.egg_counter++;
        AnimalManager.Instance.safeEggPosition(counter, position);
    }

    
}
