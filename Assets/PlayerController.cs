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
    public Rigidbody2D rb;
    Vector2 movement;
    public float movementSpeed = 10f;

    
    public int totalToolNumb = 6;
    public int currentToolNumb;



    [SerializeField] private UIInventory uiInventory;
    [SerializeField] private PlayerEnergy playerEnergy;
    [SerializeField] private UIMerchant uiMerchant;

    private Inventory inventory;
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
        if (Input.GetKeyDown(KeyCode.I))
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
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        movement = new Vector2(movementSpeed * inputX, movementSpeed * inputY);
    }

    void CheckAction()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Face the Direction clicked
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            transform.LookAt(worldPosition, Vector3.up);
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

    void SelectedPlant(GameObject plant)
    {
        //ToDo: Implement Harvestable Tag
        //if(gameObject.tag == "harvestable")
        //{
        //float interactRange = 1f;
        //Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange);
        //foreach (Collider2D collider in colliderArray)
        //{
            if (plant.TryGetComponent(out PlantWorld plantWorld))
            {
                if(plantWorld.getClickableState())
            {
                inventory.AddItem(plantWorld.GetItem());
                plantWorld.DestroySelf();
                playerEnergy.EnergyChange();
            }

            }
        //}
        //}
    }

    public PlantWorld GetInteractableObject()
    {
        List<PlantWorld> plantInteractableList = new List<PlantWorld>();
        float interactRange = 0.37f;
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
