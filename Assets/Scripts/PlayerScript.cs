using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D body;
    public float movementSpeed = 1f;
    Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check for Movement
        CheckMovement();
        //Check for Action
        CheckAction();
    }

    private void FixedUpdate()
    {
        if(body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }
        body.velocity = movement;
    }

    void CheckMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        movement = new Vector2(movementSpeed * inputX, movementSpeed * inputY);
    }

    void CheckAction()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            //Face the Direction clicked
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            transform.LookAt(worldPosition, Vector3.up);
            //Play Action based on current Inventory Item (Tile Based Action - Action performed on current tile and Tile + 1 in Look direction)
        }
        return;
    }
}
