using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sheepScript : MonoBehaviour
{

    public bool hasWool = true;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if(!AnimalManager.Instance.sheepHasWool)
        {
            hasWool = false;
            anim.SetBool("hasWool", hasWool);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchWoolState()
    {
        hasWool = !hasWool;
        anim.SetBool("hasWool", hasWool);
        AnimalManager.Instance.sheepHasWool = hasWool;
    }
}
