using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerShow : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    public float movementSpeed = 2f;
    //TODO put boundaries only one time anywhere
    private float leftBoundary = -4.86f;
    private float rightBoundary = 7.5f;
    private float topBoundary = -0.87f;
    private float bottomBoundary = -4.2f;

    private bool isMoving = false;
    private bool isWaiting = false;
    private bool startedMoving = false;
    private Direction direction = 0;
    public float moveDuration = 2f;
    public float waitDuration = 2f;
    private float movingTimer = 0f;
    private float waitingTimer = 0f;

    private bool stopMovement = false;

    //Startposition at x=-2, y=-3

    enum Direction
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }


    void Start()
    {
        anim = GetComponent<Animator>();
        //Restore eggs if layed before
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stopMovement)
        {
            stopMovement = false;
            isMoving = false;
            movingTimer = 0;
            direction = 0;
            isWaiting = true;
            startedMoving = false;
            StopAnimation();
        }

        if (direction == 0)
        {
            direction = getDirection();
            isWaiting = true;
        }
        if (isWaiting)
        {
            waitingTimer += Time.deltaTime;
            if (waitingTimer >= waitDuration)
            {
                isWaiting = false;
                waitingTimer = 0;
                isMoving = true;
            }
        }
        if (isMoving)
        {
            if (!startedMoving)
            {
                startMovingAnim(direction);
                startedMoving = true;
            }
            move(direction);

            movingTimer += Time.deltaTime;
            if (movingTimer >= moveDuration)
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

    Direction getDirection()
    {
        Direction dir = (Direction)Random.Range(1, 5);
        Vector2 position = transform.position;

        if (position.x == leftBoundary)
        {
            return Direction.Right;
        }
        else if (position.x == rightBoundary)
        {
            return Direction.Left;
        }
        else if (position.y == topBoundary)
        {
            return Direction.Down;
        }
        else if (position.y == bottomBoundary)
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

        if (position.x <= leftBoundary)
        {
            position.x = leftBoundary;
            anim.SetBool("directionLeft", false);
        }
        if (position.x >= rightBoundary)
        {
            anim.SetBool("directionRight", false);
            position.x = rightBoundary;
        }
        if (position.y >= topBoundary)
        {
            anim.SetBool("directionUp", false);
            position.y = topBoundary;
        }
        if (position.y <= bottomBoundary)
        {
            anim.SetBool("directionDown", false);
            position.y = bottomBoundary;
        }

        transform.position = position;
    }

    private void StopAnimation()
    {
        anim.SetBool("directionUp", false);
        anim.SetBool("directionLeft", false);
        anim.SetBool("directionRight", false);
        anim.SetBool("directionDown", false);
    }
}
