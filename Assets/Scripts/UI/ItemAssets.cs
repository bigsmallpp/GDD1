using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    private static ItemAssets _instance = null;
    public static ItemAssets Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform pfPlantWorld;


    public Sprite wheat_seed;
    public Sprite carrot_seed;
    public Sprite cauliflower_seed;
    public Sprite chicken_upgrade;
    public Sprite egg;
    public Sprite carrot;
    public Sprite wheat;
    public Sprite cauliflower;
    public Sprite milk;
    public Sprite wool;
    public Sprite lamp;
    public Sprite scissors;
    public Sprite bucket;
}
