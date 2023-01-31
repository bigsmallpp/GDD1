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
    [SerializeField] private Store _store;
    [SerializeField] private PlayerEnergy playerEnergy;
    [SerializeField] private UIMerchant uiMerchant;
    [SerializeField] private InputActionReference movementkey, interactionkey, inventorykey, hotbarkey1, hotbarkey2, hotbarkey3, hotbarkey4, hotbarkey5, hotbarkey6, hotbarCycle;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapController tileMapController;
    [SerializeField] float maxInteractDistance = 1f;
    [SerializeField] Fieldmanager fieldManager;

    //Boundaries
    public float leftBoundary = -8.621f;
    public float rightBoundary = 8.622f;
    public float topBoundary = 4.594f;
    public float bottomBoundary = -4.594f;

    private Inventory inventory;
    private Vector2 movement;
    public UIInteract uiInteract;

    private Animator anim;

    Vector3Int selectedTilePos;
    bool selectable;
    
    private void Awake()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        //uiMerchant.SetInventory(inventory);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckMovement();
        ToolSelection(); // -- Maybe Outsource to different Obj
        Marker();
        CheckAction();
        //Show Hide Inventory UI -- Maybe Outsource to different Obj
        updateAnim();

        //Show Hide Inventory UI
        if (inventorykey.action.WasPressedThisFrame())
        {
            uiInventory.SetActiveAlternativly();
        }

        //Remove one Tomato from Inventory on Key Q -- Maybe Outsource to different Obj
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
        float moveX = movementSpeed * input.x;
        float moveY = movementSpeed * input.y;
        /*Vector2 playerPos = gameObject.transform.position;
        bool leftB = leftBoundary < playerPos.x;
        bool rightB = playerPos.x < rightBoundary;
        bool topB = playerPos.y < topBoundary;
        bool botB = bottomBoundary < playerPos.y;*/
        movement = new Vector2(moveX, moveY);
    }

    void CheckAction()
    {
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
                fieldManager.Plow(selectedTilePos);
                return;
            }
        }
    }

    private void SeedGrid()
    {
        if(selectable && fieldManager.CheckStatus(selectedTilePos) == 1)
        {
            fieldManager.Seed(selectedTilePos);
            return;
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
            if (plant != null && plant.isRipe())
            {
                inventory.AddItem(plant.getItem());
                playerEnergy.EnergyChange();
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
                    _store.OpenOrClose();
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

    public Inventory GetPlayerInventory()
    {
        return inventory;
    }

    public Store GetStore()
    {
        return _store;
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

    private void Marker()
    {
        selectedTilePos = tileMapController.GetGridPosition(Input.mousePosition);
        SelectableCheck();
        Vector3Int gridPos = selectedTilePos;
        markerManager.markedCellPos = gridPos;
    }
    void updateAnim()
    {
        
        anim.SetBool("directionDown", false);
        anim.SetBool("directionUp", false);
        anim.SetBool("directionLeft", false);
        anim.SetBool("directionRight", false);
        
        if(Input.GetKey(KeyCode.S))
        {
            anim.SetBool("directionDown", true);
        }
        
        if(Input.GetKey(KeyCode.W))
        {
            anim.SetBool("directionUp", true);
        }
        
        if(Input.GetKey(KeyCode.A))
        {
            anim.SetBool("directionLeft", true);
        }
        
        if(Input.GetKey(KeyCode.D))
        {
            anim.SetBool("directionRight", true);
        }
    }

    public int getCurrentMoney()
    {
        return currentMoney;
    }

    public void decreaseCurrentMoney(int amount, int quantity)
    {
        currentMoney -= amount * quantity;
    }

    public void increaseCurrentMoney(int amount, int quantity)
    {
        currentMoney += amount * quantity;
    }
}
