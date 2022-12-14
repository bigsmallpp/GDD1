using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    
    public EnergySlider energySlider;

    public int currentMoney = 100;

    private float horizontal;
    private float vertical;
    public Rigidbody2D rb;
    public float movementSpeed = 10f;

    
    public int totalToolNumb = 6;
    public int currentToolNumb;



    [SerializeField] private UIInventory uiInventory;
    [SerializeField] private PlayerEnergy playerEnergy;
    [SerializeField] private UIMerchant uiMerchant;
    [SerializeField] private InputActionReference movementkey, interactionkey, inventorykey, hotbarkey1, hotbarkey2, hotbarkey3, hotbarkey4, hotbarkey5, hotbarkey6, hotbarCycle;

    private Inventory inventory;
    private Vector2 movement;
    public UIInteract uiInteract;


    

    
    private void Awake()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        //uiMerchant.SetInventory(inventory);
    }

    private void Update()
    {
        CheckMovement();
        ToolSelection();
        CheckAction();

        //Show Hide Inventory UI
        if (inventorykey.action.WasPressedThisFrame())
        {
            uiInventory.SetActiveAlternativly();
        }
        
        //Remove one Tomato from Inventory on Key Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(inventory.GetItemList());
            inventory.RemoveItem(new Item { itemType = Item.ItemType.tomato, amount = 1 , prize = 10});
        }
        
        
    }
    private void FixedUpdate()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.velocity = movement;

    }

    void CheckMovement()
    {
        Vector2 input = movementkey.action.ReadValue<Vector2>();
        movement = new Vector2(movementSpeed * input.x, movementSpeed * input.y);
    }

    void CheckAction()
    {
        if (interactionkey.action.WasPressedThisFrame())
        {
            //Face the Direction clicked
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            
            //ToDo: Check which Item is active and change Behavior according to it (No Item requirement for harvesting)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D raycastHit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if (raycastHit.collider != null)
            {
                //Our custom method. 
                SelectedPlant(raycastHit.collider.gameObject);
            }


        }
        return;
    }

    void SelectedPlant(GameObject obj)
    {
        //ToDo: Implement Harvestable Tag
        //if(gameObject.tag == "harvestable")
        if (obj.TryGetComponent(out PlantBaseClass plant))
        { 
            if(plant.getClickable() && plant.isRipe())
            {
                inventory.AddItem(plant.getItem());
                playerEnergy.EnergyChange();
                Destroy(plant.gameObject);
            }
        }

    }

    public PlantBaseClass GetInteractableObject()
    {
        List<PlantBaseClass> plantInteractableList = new List<PlantBaseClass>();
        // TODO Move to Utils
        float interactRange = 0.37f;
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange);

        foreach (Collider2D collider in colliderArray)
        {
            if (collider.TryGetComponent(out PlantBaseClass plant))
            {
                plantInteractableList.Add(plant);
            }
        }

        PlantBaseClass closestPlant = null;
        foreach(PlantBaseClass current_plant in plantInteractableList)
        {
            if (closestPlant == null)
            {
                closestPlant = current_plant;
            } else
            {
                if(Vector3.Distance(transform.position, current_plant.transform.position) < 
                    Vector3.Distance (transform.position , closestPlant.transform.position))
                {
                    closestPlant = current_plant;
                }
            }
        }
        
        return closestPlant;
    }
   

    private void ToolSelection()
    {
        
        if(currentToolNumb >= 0 && hotbarCycle.action.ReadValue<float>() > 0)
        {
            currentToolNumb += 1;
            if (currentToolNumb == 6)
            {
                currentToolNumb = 0;
            }
        }

        if(currentToolNumb <= 5 && hotbarCycle.action.ReadValue<float>() < 0)
        {
            currentToolNumb -= 1;
            if (currentToolNumb == -1)
            {
                currentToolNumb = 5;
            }
        }

        if(hotbarkey1.action.WasPressedThisFrame())
        { 
            currentToolNumb = 0;
        }
        if (hotbarkey2.action.WasPressedThisFrame())
        {
            currentToolNumb = 1;
        }
        if (hotbarkey3.action.WasPressedThisFrame())
        {
            currentToolNumb = 2;
        }
        if (hotbarkey4.action.WasPressedThisFrame())
        {
            currentToolNumb = 3;
        }
        if (hotbarkey5.action.WasPressedThisFrame())
        {
            currentToolNumb = 4;
        }
        if (hotbarkey6.action.WasPressedThisFrame())
        {
            currentToolNumb = 5;
        }

        
    }

    







}
