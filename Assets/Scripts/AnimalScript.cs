using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalScript : MonoBehaviour
{

    private Animator anim;

    // Start is called before the first frame update
    public float movementSpeed = 2f;
    public bool hasLayed = false;
    private float leftBoundary = -5.2f;
    private float rightBoundary = -2.1f;
    private float topBoundary = 4.6f;
    private float bottomBoundary = 1.45f;

    private bool isMoving = false;
    private bool isWaiting = false;
    private bool startedMoving = false;
    private Direction direction = 0;
    public float moveDuration = 2f;
    public float waitDuration = 2f;
    private float movingTimer = 0f;
    private float waitingTimer = 0f;

    public float startPositionX = -3.5f;
    public float startPositionY = 3f;

    private bool stopMovement = false;


    //Startposition at x=-2, y=-3

    enum Direction{
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
    

    void Start()
    {
        anim = GetComponent<Animator>();
        Vector2 getPos = SceneLoader.Instance.getChickenPos();
        //Signal chicken new spawned
        AnimalManager.Instance.setChickenRespawned();
        if (getPos != null)
        {
            transform.position = getPos;
        }
        //Restore eggs if layed before
        if(AnimalManager.Instance.egg_counter > 0)
        {
            AnimalManager.Instance.restoreEggs();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(TimeManager.Instance._time_enabled)
        { 
            if(stopMovement)
            {
                stopMovement = false;
                isMoving = false;
                movingTimer = 0;
                direction = 0;
                isWaiting = true;
                startedMoving = false;
                StopAnimation();
            }

            if(direction == 0)
            {
                direction = getDirection();
                isWaiting = true;
            }
            if(isWaiting)
            {
                waitingTimer += Time.deltaTime;
                if(waitingTimer >= waitDuration)
                {
                    isWaiting = false;
                    waitingTimer = 0;
                    isMoving = true;
                }
            }
            if(isMoving)
            {
                if(!startedMoving)
                {
                    startMovingAnim(direction);
                    startedMoving = true;
                }
                move(direction);

                movingTimer += Time.deltaTime;
                if(movingTimer >= moveDuration)
                {
                    isMoving = false;
                    movingTimer = 0;
                    direction = 0;
                    isWaiting = true;
                    startedMoving = false;

                    StopAnimation();
                }
            }
        }
        else
        {
            StopAnimation();
            //TODO: Freeze Animation
        }
    }

    Direction getDirection()
    {
        Direction dir = (Direction)Random.Range(1,5);
        Vector2 position = transform.position;
        
        if(position.x == leftBoundary)
        {
            return Direction.Right;
        }
        else if(position.x == rightBoundary)
        {
            return Direction.Left;
        }
        else if(position.y == topBoundary)
        {
            return Direction.Down;
        }
        else if(position.y == bottomBoundary)
        {
            return Direction.Up;
        }
        return dir;
    }

    void startMovingAnim(Direction randDirection)
    {
        switch (randDirection)
        {
            case Direction.Up:
            anim.SetBool("chickenLeft", true);
            break;
            case Direction.Down:
            anim.SetBool("chickenDown", true);
            break;
            case Direction.Left:
            anim.SetBool("chickenLeft", true);
            break;
            case Direction.Right:
            anim.SetBool("chickenRight", true);
            break; 
            default:
            break;
        }
    }

    void move(Direction randDirection)
    {
        Vector2 position = transform.position;
        float deltaMove = movementSpeed * Time.fixedDeltaTime;
        Vector2 move = Vector2.zero;

        switch (randDirection)
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
        //Boundary check

        if(position.x <= leftBoundary)
        {
            position.x = leftBoundary;
            anim.SetBool("chickenLeft", false);
        }
        if(position.x >= rightBoundary)
        {
            anim.SetBool("chickenRight", false);
            position.x = rightBoundary;
        }
        if(position.y >= topBoundary)
        {
            anim.SetBool("chickenLeft", false);
            position.y = topBoundary;
        }
        if(position.y <= bottomBoundary)
        {
            anim.SetBool("chickenDown", false);
            position.y = bottomBoundary;
        }

        transform.position = position;
        SceneLoader.Instance.saveChickenPos(position);
    }

    private void StopAnimation()
    {
        anim.SetBool("chickenLeft", false);
        anim.SetBool("chickenRight", false);
        anim.SetBool("chickenDown", false);
    }

    public void StopChicken()
    {
        stopMovement = true;
    }
}
