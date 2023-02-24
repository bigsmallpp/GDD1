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
    [SerializeField] GameObject pauseMenu;

    //Boundaries
    public float leftBoundary = -8.621f;
    public float rightBoundary = 8.622f;
    public float topBoundary = 4.594f;
    public float bottomBoundary = -4.594f;

    private Inventory inventory;
    private Vector2 movement;
    public UIInteract uiInteract;

    private Animator anim;
    private Sprite _current_sprite;

    public enum Direction{
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    Vector3Int selectedTilePos;
    bool selectable;
    private bool gamePaused = false;
    private bool blockControlling = false;
    private bool isAutoMoving = false;
    private bool startedMoving = false;
    private float movingTimer = 0.0f;
    public float autoMoveDuration = 0.55f;
    public float autoMoveSpeedScaling = 0.35f;
    private Direction _moving_direction = 0;


    private void Awake()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        //uiMerchant.SetInventory(inventory);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = SceneLoader.Instance.current_position;
        //TODO: Check if Stable entered

        //Set Player variant
        setPlayerVariant();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            pauseMenu.SetActive(gamePaused);
            //Freeze Game
            TimeManager.Instance.pauseTime(!gamePaused);
        }

        if(!gamePaused && !blockControlling)
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
    }
    private void FixedUpdate()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.velocity = movement;

        if(isAutoMoving)
            {
                if(!startedMoving)
                {
                    startMovingAnim();
                    startedMoving = true;
                }
                AutoMovePlayer();

                movingTimer += Time.deltaTime;
                if(movingTimer >= autoMoveDuration)
                {
                    isAutoMoving = false;
                    movingTimer = 0.0f;
                    startedMoving = false;
                    stopMovingAnim();
                    blockControlling = false;
                    _moving_direction = 0;
                }
            }

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
            /*currentToolNumb += 1;
            if (currentToolNumb == 6)
            {
                currentToolNumb = 0;
            }*/

            currentToolNumb -= 1;
            if (currentToolNumb == -1)
            {
                currentToolNumb = 5;
            }
        }

        if(currentToolNumb <= 5 && hotbarCycle.action.ReadValue<float>() < 0)
        {
            /*currentToolNumb -= 1;
            if (currentToolNumb == -1)
            {
                currentToolNumb = 5;
            }*/

            currentToolNumb += 1;
            if (currentToolNumb == 6)
            {
                currentToolNumb = 0;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "StableDoorInside" && !blockControlling)
        {
            SceneLoader.Instance.loadScene(1);
        }
        if(collision.gameObject.tag == "StableDoorOutside" && !blockControlling)
        {
            SceneLoader.Instance.loadScene(2);
            SceneLoader.Instance.enterStable = true;
        }
        if (collision.gameObject.tag == "StableEnterChicken" && !blockControlling)
        {
            //SceneLoader.Instance.loadScene(2);
            transform.position = SceneLoader.Instance.chicken_cage_position;
            //Make Enter animation
            stopMovingAnim();
            enterAutoMovementAnim(Direction.Down);
        }
        if (collision.gameObject.tag == "StableLeaveChicken" && !blockControlling)
        {
            //SceneLoader.Instance.loadScene(2);
            transform.position = SceneLoader.Instance.chicken_door_position;
            //Make Enter animation
            stopMovingAnim();
            enterAutoMovementAnim(Direction.Down);
        }
        if (collision.gameObject.tag == "Food Container")
        {
            if (currentToolNumb == 1)
            {
                FoodContainer container = collision.gameObject.GetComponent<FoodContainer>();
                container.fillContainer();
            }
        }
        //transform.position = SceneLoader.Instance.current_position;

        if (collision.gameObject.tag == "GotToField" && !blockControlling)
        {
            stopMovingAnim();
            StartCoroutine(TransitionToNextScene(Direction.Down, 3));
        }

        if (collision.gameObject.tag == "GoHome" && !blockControlling)
        {
            stopMovingAnim();
            StartCoroutine(TransitionToNextScene(Direction.Up, 4));
        }

        if(collision.gameObject.tag == "HouseDoor")
        {
            TimeManager.Instance.skipToNextDay();
        }

        if(collision.gameObject.tag == "Cow" && currentToolNumb == 2 && AnimalManager.Instance.cowHasMilk)
        {
            Debug.Log("Milk cow!");
            //TODO: Get milk object
            AnimalManager.Instance.cowHasMilk = false;
        }

        if(collision.gameObject.tag == "Sheep" && currentToolNumb == 3 && AnimalManager.Instance.sheepHasWool)
        {
            sheepScript sheep = collision.gameObject.GetComponent<sheepScript>();
            sheep.switchWoolState();
            Debug.Log("Shore sheep!");
        }
    }

    IEnumerator TransitionToNextScene(Direction direction, int scene)
    {
        //Set duration longer
        autoMoveDuration = 1f;
        enterAutoMovementAnim(direction);
        yield return new WaitForSeconds(1.2f);
        //Load Scene
        //transform.position = Vector2.zero;
        SceneLoader.Instance.loadScene(scene);
    }

    public void TransitionWhenEnterScene(Direction direction, float duration)
    {
        StartCoroutine(AnimationOnEnterScene(direction, duration, 1f));
    }

    IEnumerator AnimationOnEnterScene(Direction direction, float duration, float waitingTime)
    {
        autoMoveDuration = duration;
        enterAutoMovementAnim(direction);
        yield return new WaitForSeconds(waitingTime);
    }

    public void enterAutoMovementAnim(Direction direction)
    {
        _moving_direction = direction;
        blockControlling = true;
        SceneLoader.Instance.enterStable = false;
        movement = Vector2.zero;

        isAutoMoving = true;
    }

    private void AutoMovePlayer()
    {
        Vector2 position = transform.position;
        float deltaMove = (movementSpeed * autoMoveSpeedScaling) * Time.fixedDeltaTime;
        Vector2 move = Vector2.zero;

        switch (_moving_direction)
        {
            case Direction.Up:
            //Move up
            move.y += deltaMove;
            break;
            case Direction.Down:
            //Move down
            move.y -= deltaMove;
            break;
            case Direction.Left:
            //Move left
            move.x -= deltaMove;
            break;
            case Direction.Right:
            //Move right
            move.x += deltaMove;
            break; 
            default:
            break;
        }

        position += move;
        transform.position = position;
    }

    private void startMovingAnim()
    {
        switch (_moving_direction)
        {
            case Direction.Up:
            anim.SetBool("directionUp", true);
            break;
            case Direction.Down:
            anim.SetBool("directionDown", true);
            break;
            case Direction.Left:
            anim.SetBool("directionLeft", true);
            break;
            case Direction.Right:
            anim.SetBool("directionRight", true);
            break; 
            default:
            break;
        }
    }

    public void stopMovingAnim()
    {
        anim.SetBool("directionDown", false);
        anim.SetBool("directionUp", false);
        anim.SetBool("directionLeft", false);
        anim.SetBool("directionRight", false);
    }

    private void setPlayerVariant()
    {
        int variant = SceneLoader.Instance.player_variant;
        switch (variant)
        {
            case 1:
            anim.SetInteger("variant", 1);
            break;
            case 2:
            anim.SetInteger("variant", 2);
            break;
            case 3:
            anim.SetInteger("variant", 3);
            break;
            default:
            break;
        }
    }
}
