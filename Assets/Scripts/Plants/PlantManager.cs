using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    [SerializeField] public Dictionary<int, List<PlantBaseClass>> _plants;
    private Queue<Utils.Request> _plant_requests;

    // Start is called before the first frame update
    void Start()
    {
        _plants = SaveManager.Instance.LoadPlantsData();
        UpdatePlantVisibility(SceneLoader.Instance.currentScene);
        _plant_requests = new Queue<Utils.Request>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrowPlants()
    {
        ResolveRequests();
        
        foreach (List<PlantBaseClass> plants_in_scene in _plants.Values)
        {
            foreach(PlantBaseClass p in plants_in_scene)
            {
                p.Grow();
            }
        }
    }

    public void AddPlant(PlantBaseClass plant)
    {
        Utils.Request add_plant_request;
        add_plant_request._scene = (int) SceneLoader.Instance.currentScene;
        add_plant_request._plant = plant;
        add_plant_request._type = Utils.RequestType.Add;
        _plant_requests.Enqueue(add_plant_request);
    }
    
    public void AddPlant(PlantBaseClass plant, int scene)
    {
        Utils.Request add_plant_request;
        add_plant_request._scene = scene;
        add_plant_request._plant = plant;
        add_plant_request._type = Utils.RequestType.Add;
        _plant_requests.Enqueue(add_plant_request);
    }

    public void RemovePlant(PlantBaseClass plant)
    {
        Utils.Request remove_plant_request;
        remove_plant_request._scene = (int) SceneLoader.Instance.currentScene;
        remove_plant_request._plant = plant;
        remove_plant_request._type = Utils.RequestType.Add;
        
        _plant_requests.Enqueue(remove_plant_request);
    }

    private void ResolveRequests()
    {
        while (_plant_requests.Count > 0)
        {
            Debug.Log("Resolving Request");
            Utils.Request current_request = _plant_requests.Dequeue();

            if (current_request._plant != null && current_request._type == Utils.RequestType.Add)
            {
                _plants[current_request._scene].Add(current_request._plant);
                
                // Carry plants in-between scenes
                current_request._plant.gameObject.transform.parent = gameObject.transform;
            }
            else if (current_request._plant != null && current_request._type == Utils.RequestType.Remove)
            {
                // _plants[current_request._scene].Remove(current_request._plant);
                DestroyPlant(current_request._plant);
            }
            
            // Else Plant expired and is invalid
        }
    }

    public void UpdatePlantVisibility(SceneLoader.Scene new_scene)
    {
        
        foreach (int scene in _plants.Keys)
        {
            bool visible = (int) new_scene == scene;
            foreach (PlantBaseClass p in _plants[scene])
            {
                p.gameObject.SetActive(visible);
            }
        }
    }

    public GameObject FindPlantWithPositionInCurrentScene(Vector2 pos)
    {
        List<PlantBaseClass> plants_in_scene = _plants[(int)SceneLoader.Instance.currentScene];

        foreach(PlantBaseClass p in plants_in_scene)
        {
            if ((Vector2) p.transform.position == pos)
            {
                return p.gameObject;
            }
        }

        return null;
    }

    public List<PlantBaseClass> GetActivePlantsInScene()
    {
        return _plants[(int)SceneLoader.Instance.currentScene];
    }

    public void DestroyPlant(PlantBaseClass plant)
    {
        GameManager.Instance.GetCropsManager().deleteEntry(plant._pos_tilemap);
        _plants[plant._plant_scene].Remove(plant);
        Destroy(plant.gameObject);
    }
}
