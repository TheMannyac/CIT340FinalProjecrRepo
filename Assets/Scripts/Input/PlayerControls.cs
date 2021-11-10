using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls: MonoBehaviour
{
    ///Public data fields
    public float moveSpeed = 4.5f;   //movement speed of the player
    public bool shouldFlipDirection = true;
    [SerializeField] private bool HasAnimator = true;
    
    Rigidbody2D rb;
    Animator animator;

    bool facingRight = true;
    Vector2 moveDirection;
    float magnitude = 0;

    //Executes first, even if script is disabled
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.Log("The Player object should have a ridgidBody2D component");

        if (HasAnimator)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
                Debug.Log("The Player object does not have a component");
        }
        
    }

    //Executes when object is initialized
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        //TODO compare between keyboard and controller inputs, fix any disparities

        //SPRITES AND ANIMATION

        //this is probably not the most efficent way to do it but it looks cool
        //TODO come back to this and see if we can only set the direction when needed instead of every frame
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        magnitude = moveDirection.magnitude;
        moveDirection = new Vector2(inputX, inputY);
        
        moveDirection.Normalize();

        if (HasAnimator)
        {
            animator.SetFloat("Horizontal", inputX);
            animator.SetFloat("Vertical", inputY);
            animator.SetFloat("Velocity", magnitude);
        }
           
        if(shouldFlipDirection)
        if (facingRight && inputX < 0 )
        {
            Flip();
        } else if (!facingRight && inputX > 0)
        {
            Flip();
        }

    }
  
    //Synchronized with the physics engine
    //use fixed update to avoid jitter when objects are moving into one another
    void FixedUpdate()
    {
        //Movement and physics
        move();
        
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

     void move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }



}

//Input.GetKeyDown = returns true  1st frame key is pressed
//Input.GetKey = returns true the whole time the key is pressed

//Framerate Independent Gameplay - we want a speed
//to be consistent whether the framerate is 2 or 200
//+= new Vector3(.01f... = move .01 units per frame to the right
//+= new Vector3(x * Time.deltaTime... = move x units per Second.

/*
if (Input.GetKey(KeyCode.W))
    transform.position += new Vector3(0, speed * Time.deltaTime, 0);
else if (Input.GetKey(KeyCode.S))
    transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
else if (Input.GetKey(KeyCode.A))
    transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
else if (Input.GetKey(KeyCode.D))
    transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
*/




//Movemnt Method 1:
//transform.position += new Vector3
//    (movementX * speedUPerSecond * Time.deltaTime, 0, 0);
//movementY * speedUPerSecond * Time.deltaTime, 0);

//Movement Method 2:
//DO NOT USE FOR PHSYICS; good for teleporting though
//transform.Translate(new Vector3(movementX * moveSpeed *Time.deltaTime,
// movementY * moveSpeed * Time.deltaTime, 0));



//Movement Method 3:
//Physical based movement, will have momentum and apply force to objects it hits
//also doesn't require factoring in Time.deltaTime because physicas updating does that for us
//rb.AddForce(transform.right * thrust * speedUPerSecond, 0);
//rb.AddTorque(-rotation * angleUPerSecond * Mathf.Deg2Rad);

//for finer control over physically-based movement , we can directly set
//velocity this way.
//rb.velocity = new Vector2(0, 0);
//rb.angularVeloity = new Vector2(0,0);