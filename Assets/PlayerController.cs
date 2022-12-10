using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    
    public EnergySlider energySlider;

    public int currentMoney = 100;

    private float horizontal;
    private float vertical;
    public float movementSpeed = 8f;

    
    public int totalToolNumb = 6;
    public int currentToolNumb;


    public Rigidbody2D rb;
    [SerializeField] private UIInventory uiInventory;
    [SerializeField] private PlayerEnergy playerEnergy;

    private Inventory inventory;
    public UIInteract uiInteract;


    private float interactTimer = 3f;
    

    private void Awake()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);

    }

    private void Update()
    {
        horizontal =  Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        ToolSelection();

        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 1f;
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange);


            foreach (Collider2D collider in colliderArray)
            {
                if (collider.TryGetComponent(out PlantWorld plantWorld))
                {
                    inventory.AddItem(plantWorld.GetItem());
                    plantWorld.DestroySelf();
                    playerEnergy.EnergyChange();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            uiInventory.SetActiveAlternativly();
        }
        
        
        
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * movementSpeed, vertical * movementSpeed);
        
    }
    
    public PlantWorld GetInteractableObject()
    {
        List<PlantWorld> plantInteractableList = new List<PlantWorld>();
        float interactRange = 1f;
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange);

            foreach (Collider2D collider in colliderArray)
            {
                if (collider.TryGetComponent(out PlantWorld plantWorld))
                {
                plantInteractableList.Add(plantWorld);
                

                }
            }

        PlantWorld closestPlant = null;

        foreach(PlantWorld plantWorld in plantInteractableList)
        {
            if (closestPlant == null)
            {
                closestPlant = plantWorld;
            } else
            {
                if(Vector3.Distance(transform.position, plantWorld.transform.position) < 
                    Vector3.Distance (transform.position , closestPlant.transform.position))
                {
                    closestPlant = plantWorld;
                }
            }
        }
        return closestPlant;
        
    }
   

    private void ToolSelection()
    {
        
        
        
       
        if(currentToolNumb >= 0 && (int)Input.mouseScrollDelta.y > 0)
        {
            currentToolNumb += 1;
            if (currentToolNumb == 6)
            {
                currentToolNumb = 0;
            }
        }

        if(currentToolNumb <= 5 && (int)Input.mouseScrollDelta.y < 0)
        {
            currentToolNumb -= 1;
            if (currentToolNumb == -1)
            {
                currentToolNumb = 5;
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        { 
            currentToolNumb = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentToolNumb = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentToolNumb = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentToolNumb = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentToolNumb = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentToolNumb = 5;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        PlantWorld plantWorld = collision.GetComponent<PlantWorld>();
        if (plantWorld != null)
        {
            
            if (interactTimer <= 0)
            {
                inventory.AddItem(plantWorld.GetItem());
                plantWorld.DestroySelf();
                
            }
            
            //uiInteract.SetUIActive(true);
            
            
        }
    }

    





}
