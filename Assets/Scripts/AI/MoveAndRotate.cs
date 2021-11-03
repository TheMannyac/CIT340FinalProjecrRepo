using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndRotate : MonoBehaviour
{
    
    //Hook in a full game object
    public GameObject targetObjecct;

    //Hook in a gameObject's transform component
    //can get the gameObject itself using targetTransform.gameObject
    public Transform targetTransform;

    //Just a position no reference  to a specific gameObject
    public Vector3 targetPosition;

    public float activationRange = 2;
    public float followDistance = 0;//the minimum amount of space the object will give the target
    public float speedPerSecond = 3;
    public float angleOffset = 0;

    public bool useActivationRange = true;
    public bool useFollowDistance = false;
    public bool shouldRotateTowardObject = false;
    public bool shouldFaceTarget = false;
    public bool isFacingRight = true;
    public bool targetMouse = false;
    public bool scriptEnabled = true;

    Animator animator;

    private void Awake()
    {
        Animator animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(targetTransform == null)
        {
            targetTransform = FindObjectOfType<PlayerControls>().transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (scriptEnabled)
        {
            ChaseTarget();
        }
    }

    public void ChaseTarget()
    {
        if (targetMouse)
        {
            //this gives us where the mouse is in pixels on the screem
            //NOT a position in the world
            //Input.mousePosition;

            //This converts the mouse cursor's position on the screen 
            //to where it correlates in the unity world
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mouseWorldPosition.z = 0;   //mouse z is naturally same as camera
            targetPosition = mouseWorldPosition;

        }
        else if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
        }
        else if (targetObjecct != null)
        {
            targetPosition = targetObjecct.transform.position;
        }

        //Moving toward a target object within a certain distance
        Vector3 directionVector = targetPosition - transform.position;

        float distanceToTarget = distancetoTarget();
        

        if (useActivationRange && distanceToTarget > activationRange)
        {
            return;
        }
        //Normalizing a vector = results in the same direction but length equals 1
        //length equals magnitude
        //Effectively extracts the direction from a vector without the magnitude.
        directionVector.Normalize();
        //Debug.Log(directionVector);


        //makes sure that we don't move closer than the follow distance (if any)
        if (!(useFollowDistance) || distanceToTarget >= followDistance)
        {
            transform.position += directionVector * speedPerSecond * Time.deltaTime;
        }

        //Rotating towrd a target object
        if (shouldRotateTowardObject)
        {
            //atan turns an angle from 0 to 90, and you have to add extra if the x is negative, y is negative, etc.
            //float angle = Mathf.Atan2();

            //most programming libraries fix that with a conveinient atan2 function
            //rotate toward an object with trigonometry 
            /*
            float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
            transform.rotation = 
                (Quaternion.Euler(new Vector3(0, 0, angle + angleOffset)));
            */
            //rotate toward an object
            transform.right = directionVector;
            transform.Rotate(new Vector3(0, 0, angleOffset));
        }
        else if (shouldFaceTarget)
        {
            if (distanceToTarget > 1  && ((isFacingRight && directionVector.x < 0)
               || (!isFacingRight && directionVector.x > 0)))
            {
                // Switch the way the player is labelled as facing
                isFacingRight = !isFacingRight;

                // Multiply the player's x local scale by -1
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }

            if (animator != null)
            {
                animator.SetFloat("targetX", directionVector.x);
                animator.SetFloat("targetY", directionVector.y);
            }
        }
    }

    public float distancetoTarget()
    {
         //Distance equals square sqrt((x2 - x1) + (y2-y1))
      return Vector3.Distance(transform.position, targetTransform.position);
    }
}

