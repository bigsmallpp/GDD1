using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfPlantWorld;



    public Sprite tomato;
    public Sprite potato;
    public Sprite tomato_seed;
    public Sprite tomato_plant;
    public Sprite chicken_upgrade;
    public Sprite egg;
    public Sprite carrot;
    public Sprite wheat;
    public Sprite cauliflower;

}
