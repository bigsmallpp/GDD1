using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance => _instance;

    [SerializeField] private TileMapController _tileManager;
    [SerializeField] private Fieldmanager _cropsManager;
    [SerializeField] private PlantManager _plantManager;
    
    // Start is called before the first frame update
    void Awake()
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

    public TileMapController GetTileManager()
    {
        return _tileManager;
    }
    
    public Fieldmanager GetCropsManager()
    {
        return _cropsManager;
    }

    public PlantManager GetPlantManager()
    {
        return _plantManager;
    }
}
