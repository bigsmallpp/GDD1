using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine.Rendering.Universal;

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
    
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private Chest _chest;
    [SerializeField] private PlayerEnergy playerEnergy;
    [SerializeField] private InputActionReference movementkey, interactionkey, inventorykey, hotbarkey1, hotbarkey2, hotbarkey3, hotbarkey4, hotbarkey5, hotbarkey6, hotbarCycle;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapController tileMapController;
    [SerializeField] float maxInteractDistance = 1f;
    [SerializeField] Fieldmanager fieldManager;
    [SerializeField] GameObject pauseMenu;
    
    public WinFunction WinningShowcaseObject;

    //Boundaries
    public float leftBoundary = -8.621f;
    public float rightBoundary = 8.622f;
    public float topBoundary = 4.594f;
    public float bottomBoundary = -4.594f;
    
    private Vector2 movement;
    public UIInteract _uiInteract;
    public SelectedToolHighlighted _toolHighlight;

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
    private bool blockMovementOnly = false;
    private bool isAutoMoving = false;
    private bool startedMoving = false;
    private float movingTimer = 0.0f;
    public float autoMoveDuration = 0.55f;
    public float autoMoveSpeedScaling = 0.35f;
    private Direction _moving_direction = 0;

    //Audio
    [SerializeField] private AudioSource scissor_Sound;
    [SerializeField] private AudioSource milk_and_egg_Sound; //milk and egg
    [SerializeField] private AudioSource dig_Sound;
    [SerializeField] private AudioSource harvest_Sound;
    [SerializeField] private AudioSource endDay_Sound;
    [SerializeField] private AudioSource fillContainer_Sound;
    [SerializeField] private AudioSource seed_Sound;
    
    private void Awake()
    {
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = SceneLoader.Instance.current_position;
        //TODO: Check if Stable entered

        //Set Player variant
        setPlayerVariant();
        SaveManager.Instance.SetPlayer(this);
        
        // TODO Make differnce between reload and scene-transition
        //      Tiles in Field disappear after save
        _playerInventory = UIHandler.Instance.GetPlayerInventory();
        SaveManager.Instance.LoadPlayerData();
        
        _uiInteract = UIHandler.Instance.GetUIInteract();
        _toolHighlight = UIHandler.Instance.GetSelectedTool();
        
        _chest = UIHandler.Instance.GetChest();
        _chest.SetPlayer(this);
        _uiInteract.UpdatePlayer(this);
        fieldManager = TimeManager.Instance.GetFieldManager();
        tileMapController = GameManager.Instance.GetTileManager();
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(GameObject.FindWithTag("UICam").GetComponent<Camera>());

        //Leaving Store Animation
        if (SceneLoader.Instance.isLeavingStore)
        {
            stopMovingAnim();
            StartCoroutine(leaveStoreAnimStart());
            SceneLoader.Instance.isLeavingStore = false;
        }
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
                _playerInventory.SetActiveAlternativly();
                if (_chest != null && _chest.isActiveAndEnabled)
                {
                    _chest.CloseChest();
                }
            }

            // //Remove one Tomato from Inventory on Key Q -- Maybe Outsource to different Obj
            // if (Input.GetKeyDown(KeyCode.Q))
            // {
            //     Debug.Log(_playerInventory.GetItems());
            //     _playerInventory.DecreaseItem(new Item( (int) Item.ItemType.tomato, 0, 0), 1);
            // }
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
        if (blockMovementOnly)
        {
            movement = new Vector2(0.0f, 0.0f);
            return;
        }
        
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
        if (interactionkey.action.WasPressedThisFrame() && TimeManager.Instance._time_enabled)
        { 
            if(Interact())
            {
                return;
            }
            if (currentToolNumb == 0 && CheckSeedOrPlowPossible())
            {
                PlowGrid();
                return;
            }
            if(currentToolNumb >= 1 && currentToolNumb <= 3 && CheckSeedOrPlowPossible() &&
               _toolHighlight.GetSeedsEnbaled(currentToolNumb))
            {
                SeedGrid();
                return;
            }
            
            if (currentToolNumb == (int) SelectedToolHighlighted.ToolbarIndices.Scissors &&
                _toolHighlight.CheckUnlocked(SelectedToolHighlighted.ToolbarIndices.Scissors) && AnimalManager.Instance.sheepHasWool)
            {
                // TODO Sheep Stuff
                //sheepScript sheep = collision.gameObject.GetComponent<sheepScript>();
                //sheep.switchWoolState();
                scissor_Sound.Play();
                AnimalManager.Instance.handleSheep();
                Item wool = new Item(Item.ItemType.wool, 1, 280);
                _playerInventory.AddItem(wool);
                Debug.Log("Shore sheep!");
                return;
            }

            if (currentToolNumb == (int) SelectedToolHighlighted.ToolbarIndices.Bucket && 
                _toolHighlight.CheckUnlocked(SelectedToolHighlighted.ToolbarIndices.Bucket) && AnimalManager.Instance.cowHasMilk)
            {
                // TODO Cow stuff
                Debug.Log("Milk cow!");
                //TODO: Get milk object
                milk_and_egg_Sound.Play();
                Item milk = new Item(Item.ItemType.milk, 1, 250);
                _playerInventory.AddItem(milk);
                AnimalManager.Instance.cowHasMilk = false;
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
                //dig_Sound.Play();
                fieldManager.Plow(selectedTilePos, dig_Sound);
                return;
            }
        }
    }

    private void SeedGrid()
    {
        if(selectable && fieldManager.CheckStatus(selectedTilePos) == 1)
        {
            //seed_Sound.Play();
            fieldManager.Seed(selectedTilePos, seed_Sound);
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
        if (CheckSeedOrPlowPossible() && fieldManager.CheckStatus(selectedTilePos) == 2)
        {
            GameObject plantObj = fieldManager.GetPlantObj(selectedTilePos);
            if (plantObj == null)
            {
                return false;
            }
            plantObj.TryGetComponent(out PlantBaseClass plant);
            if (plant != null && plant.isRipe())
            {
                harvest_Sound.Play();
                Item new_item = new Item();
                new_item.Duplicate(plant.getItem());
                _playerInventory.AddItem(new_item);
                playerEnergy.EnergyChange();
                fieldManager.deleteEntry(selectedTilePos);
                GameManager.Instance.GetPlantManager()._plants[(int)SceneLoader.Instance.currentScene].Remove(plant);
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
        int current_tool = currentToolNumb;
        
        if(currentToolNumb >= 0 && hotbarCycle.action.ReadValue<float>() > 0)
        {
            currentToolNumb -= 1;
            if (currentToolNumb == -1)
            {
                currentToolNumb = 5;
            }
        }

        if(currentToolNumb <= 5 && hotbarCycle.action.ReadValue<float>() < 0)
        {
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

        if (currentToolNumb != current_tool)
        {
            _toolHighlight.HighlightTool(currentToolNumb);
        }
    }

    private void Marker()
    {
        if (!CheckSeedOrPlowPossible())
        {
            return;
        }

        if (tileMapController.GetTileMap() == null)
        {
            return;
        }
        
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
        
        if (blockMovementOnly)
        {
            return;
        }
        
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
            FoodContainer container = collision.gameObject.GetComponent<FoodContainer>();
            if (!container._filled)
            {
                foreach (Item item in _playerInventory.GetItems())
                {
                    if (item.itemType == Item.ItemType.wheat)
                    {
                        //_playerInventory.RemoveItem(item);
                        fillContainer_Sound.Play();
                        _playerInventory.DecreaseItem(item, 1);
                        container.fillContainer();
                        break;
                    }
                }
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
            endDay_Sound.Play();
            TimeManager.Instance.skipToNextDay();
        }

        if(collision.gameObject.tag == "Cow" && currentToolNumb == 2 && AnimalManager.Instance.cowHasMilk) //ToolbarIndices.Bucket
        {
            //Debug.Log("Milk cow!");
            //TODO: Get milk object
            //Item milk = new Item(Item.ItemType.milk, 1, 250);
            //_playerInventory.AddItem(milk);
            //AnimalManager.Instance.cowHasMilk = false;
        }

        if(collision.gameObject.tag == "Sheep" && currentToolNumb == 3 && AnimalManager.Instance.sheepHasWool) //ToolbarIndices.Scissor
        {
            //sheepScript sheep = collision.gameObject.GetComponent<sheepScript>();
            //sheep.switchWoolState();
            //Item wool = new Item(Item.ItemType.wool, 1, 280);
            //_playerInventory.AddItem(wool);
            //Debug.Log("Shore sheep!");
        }

        if (collision.gameObject.tag == "StorePath")
        {
            SceneLoader.Instance.safePos(transform.position);
            stopMovingAnim();
            StartCoroutine(TransitionToNextScene(Direction.Right, 5));
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
            case 4:
            anim.SetInteger("variant", 4);
            break;
            default:
            break;
        }
    }

    public void LoadPlayerData(PlayerDataStore data, bool skip_position_and_inventory = false)
    {
        currentMoney = data._money;

        if (!skip_position_and_inventory)
        {
            transform.position = new Vector3(data._pos_x, data._pos_y);
            foreach(ItemsDataStore item_store in data._items)
            {
                _playerInventory.AddItem(new Item(item_store._type, item_store._amount, item_store._price));            
            }
        }
    }

    public Chest GetChest()
    {
        return _chest;
    }

    public void AddProfit(int profit)
    {
        currentMoney += profit;
    }

    private bool CheckSeedOrPlowPossible()
    {
        if (SceneLoader.Instance.currentScene == SceneLoader.Scene.Stable ||
            SceneLoader.Instance.currentScene == SceneLoader.Scene.Shop)
        {
            return false;    
        }

        return true;
    }

    public void BlockMovement(bool block)
    {
        blockMovementOnly = block;
    }

    IEnumerator leaveStoreAnimStart()
    {
        autoMoveDuration = 1f;
        enterAutoMovementAnim(Direction.Left);
        yield return new WaitForSeconds(1.2f);
    }
    
    public void LoseGame()
    {
        gamePaused = true;
        stopMovingAnim();
        TimeManager.Instance.PauseTimeProgression(); //????? time moving forward
        //WinningShowcaseObject = GameObject.FindGameObjectWithTag("WinObject").GetComponent<WinFunction>();
        WinningShowcaseObject.gameObject.SetActive(true);
        WinningShowcaseObject.setWinState(SceneLoader.WinningState.lost);
    }
    
    public void WinGame()
    {
        gamePaused = true;
        stopMovingAnim();
        TimeManager.Instance.PauseTimeProgression();
        //WinningShowcaseObject = GameObject.FindGameObjectWithTag("WinObject").GetComponent<WinFunction>();
        WinningShowcaseObject.gameObject.SetActive(true);
        WinningShowcaseObject.setWinState(SceneLoader.WinningState.won);
    }

    public void playEggSound()
    {
        milk_and_egg_Sound.Play();
    }
}
