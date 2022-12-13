using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    [SerializeField] private List<PlantBaseClass> _plants;
    private Queue<Utils.Request> _plant_requests;

    // Start is called before the first frame update
    void Start()
    {
        _plants = new List<PlantBaseClass>();
        _plant_requests = new Queue<Utils.Request>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrowPlants()
    {
        ResolveRequests();
        
        foreach(PlantBaseClass p in _plants)
        {
            p.Grow();
        }
    }

    public void AddPlant(PlantBaseClass plant)
    {
        Utils.Request add_plant_request;
        add_plant_request._plant = plant;
        add_plant_request._type = Utils.RequestType.Add;
        
        _plant_requests.Enqueue(add_plant_request);
    }

    public void RemovePlant(PlantBaseClass plant)
    {
        Utils.Request remove_plant_request;
        remove_plant_request._plant = plant;
        remove_plant_request._type = Utils.RequestType.Add;
        
        _plant_requests.Enqueue(remove_plant_request);
    }

    private void ResolveRequests()
    {
        while (_plant_requests.Count > 0)
        {
            Utils.Request current_request = _plant_requests.Dequeue();

            if (current_request._plant != null && current_request._type == Utils.RequestType.Add)
            {
                _plants.Add(current_request._plant);
            }
            else if (current_request._plant != null && current_request._type == Utils.RequestType.Remove)
            {
                _plants.Remove(current_request._plant);
            }
            
            // Else Plant expired and is invalid
        }
    }
}
