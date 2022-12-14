using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalScript : MonoBehaviour
{

    private Animator anim;

    // Start is called before the first frame update
    public float movementSpeed = 2f;
    public bool bought = false;
    public bool hasLayed = false;
    public float leftBoundary = -4f;
    public float rightBoundary = 4f;
    public float topBoundary = -1.5f;
    public float bottomBoundary = -4f;

    //public TimeManager tManager;
    private bool isMoving = false;
    private bool isWaiting = false;
    private bool startedMoving = false;
    private Direction direction = 0;
    public float moveDuration = 2f;
    public float waitDuration = 2f;
    private float movingTimer = 0f;
    private float waitingTimer = 0f;

    enum Direction{
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
    

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
                Debug.Log("Start Anim");
                Debug.Log(direction);
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

                anim.SetBool("chickenLeft", false);
                anim.SetBool("chickenRight", false);
                anim.SetBool("chickenDown", false);
            }
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
            Debug.Log("Up State Start Anim");
            break;
            case Direction.Down:
            anim.SetBool("chickenDown", true);
            Debug.Log("Down State Start Anim");
            break;
            case Direction.Left:
            anim.SetBool("chickenLeft", true);
            Debug.Log("Left State Start Anim");
            break;
            case Direction.Right:
            anim.SetBool("chickenRight", true);
            Debug.Log("Right State Start Anim");
            break; 
            default:
            Debug.Log("Default State Start Anim");
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
    }
}
