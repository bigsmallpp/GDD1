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
    [SerializeField] private UIMerchant uiMerchant;

    private Inventory inventory;
    public UIInteract uiInteract;
    


    

    
    private void Awake()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);

        uiMerchant.SetInventory(inventory);
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

            //Besides I the Inventory can also be opened and closed with TAB
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                _playerInventory.SetActiveAlternativly();
            }
        }

        //Show Hide Inventory UI
        if (Input.GetKeyDown(KeyCode.I))
        {
            uiInventory.SetActiveAlternativly();
        }
        
        //Remove one Tomato from Inventory on Key Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(inventory.GetItemList());
            inventory.RemoveItem(new Item { itemType = Item.ItemType.tomato, amount = 1 , price = 10});
        }
        
        
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * movementSpeed, vertical * movementSpeed);
        
    }
    
    public PlantWorld GetInteractableObject()
    {
<<<<<<< Updated upstream
        List<PlantWorld> plantInteractableList = new List<PlantWorld>();
        float interactRange = 1f;
=======
        if (interactionkey.action.WasPressedThisFrame())
        { 
            if(Interact())
            {
                return;
            }
            if (currentToolNumb == 0)
            {
                PlowGrid();
                return;
            }
            if(currentToolNumb == 1)
            {
                SeedGrid();
                return;
            }
        }
        return;
    }

    private void PlowGrid()
    {
        if(selectable)
        {
            if(fieldManager.CheckStatus(selectedTilePos) == 0)
            {
                if (playerEnergy.currentEnergy >= playerEnergy.EnergyCost(0))
                {
                    fieldManager.Plow(selectedTilePos);

                    playerEnergy.EnergyChange(0);
                    return;
                }
                
            }
        }
    }


    private void SeedGrid()
    {
        if(selectable && fieldManager.CheckStatus(selectedTilePos) == 1)
        {
            if (playerEnergy.currentEnergy >= playerEnergy.EnergyCost(1))
            {
                fieldManager.Seed(selectedTilePos);
                playerEnergy.EnergyChange(1);
                return;

            }
                
        }
    }

    void SelectableCheck()
    {
        selectable = DistanceToObject() < maxInteractDistance;
        markerManager.Show(selectable);
    }

    bool Interact()
    {
        //ToDo: Implement Harvestable Tag
        //if(gameObject.tag == "harvestable")
        if (fieldManager.CheckStatus(selectedTilePos) == 2)
        {
            GameObject plantObj = fieldManager.GetPlantObj(selectedTilePos);
            if (plantObj == null)
            {
                return false;
            }
            plantObj.TryGetComponent(out PlantBaseClass plant);
            if (plant != null && plant.isRipe() && playerEnergy.currentEnergy >= playerEnergy.EnergyCost(2))
            {
                _playerInventory.AddItem(plant.getItem());
                playerEnergy.EnergyChange(2);
                fieldManager.deleteEntry(selectedTilePos);
                Destroy(plant.gameObject);
                return true;
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D raycastHit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if (raycastHit.collider != null && DistanceToObject() <= maxInteractDistance)
            {
                GameObject obj = raycastHit.collider.gameObject;
                if (obj.tag == "Chest")
                {
                    _chest.OpenOrCloseChestUI();

                    if (_chest.gameObject.activeSelf)
                    {
                        _playerInventory.SetActive(true);
                        blockMovementOnly = true;
                    }
                    else
                    {
                        _playerInventory.SetActive(false);
                        blockMovementOnly = false;
                    }
                    
                    return true;
                }
            }
        }
        return false;
    }

    private float DistanceToObject()
    {
        Vector2 playerPos = transform.position;
        Vector2 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Vector2.Distance(playerPos, camPos);
    }

    public PlayerInventory GetPlayerInventory()
    {
        return _playerInventory;
    }

    public PlantBaseClass GetInteractableObject()
    {
        List<PlantBaseClass> plantInteractableList = new List<PlantBaseClass>();
        // TODO Move to Utils
        float interactRange = 0.37f;
>>>>>>> Stashed changes
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

    







}
