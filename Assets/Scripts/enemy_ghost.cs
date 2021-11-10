using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_ghost : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float attackRate = .5f;

    public float desiredTime = 5; //number of seconds it takes to move between each waypoint
    private float elapsedTime;
    [SerializeField] private AnimationCurve speedCurve;
    public float distanceThreshold = .75f;

    private Vector2 targetWaypoint;
    private Vector2 prevWaypoint;
    private int targetWaypointIndex;

    private SpriteRenderer sr;
    private WaypointManager WaypointMan;

    private void Awake()
    {
        //Retrieve all the components
        WaypointMan = GetComponent<WaypointManager>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Ways of getting another script/object's data, Method 2
        //Slowest, but works for objects that didn't exist on game start
        //waypointManager = FindObjectOfType<WaypointManager>();

        //Dynamically adding components:
        //gameObject.AddComponent(typeof(Tower));

        //Ways of getting another script/object's data, Method 3

        //Set the waypoints
        prevWaypoint = transform.position;
        targetWaypoint = WaypointMan.waypoints[0];

    }

    private void OnEnable()
    {
        //Debug.Log("object is enabled");
        //Begin attacking
        StartCoroutine(shootBullet());
    }


    // Update is called once per frame
    void Update()
    {
        //Movement 
        Vector2 directionToMove = targetWaypoint - (Vector2)transform.position;
        float distance = directionToMove.magnitude;
        if (distance < distanceThreshold)
        {

            //Loops back to first waypoint if it finishes the list
            if (targetWaypointIndex == WaypointMan.waypoints.Count - 1)
                targetWaypointIndex = 0;
            else
                targetWaypointIndex++;

            prevWaypoint = targetWaypoint;
            targetWaypoint = WaypointMan.waypoints[targetWaypointIndex];

            //reset timer
            elapsedTime = 0;
            //directionToMove = targetWaypoint - (Vector2)transform.position;
        }

        //Figure out where we should be in the commute
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredTime;
        transform.position = Vector2.Lerp(prevWaypoint, targetWaypoint ,speedCurve.Evaluate(percentageComplete));

        //Flips sprite to face direction object is going in
        directionToMove.Normalize();
        if (directionToMove.x > .01)
            sr.flipX = true;
        else
            sr.flipX = false;

    }

    public IEnumerator shootBullet()
    {
        while (true)
        {
            Instantiate(bulletPrefab,transform.position,Quaternion.identity);
            yield return new WaitForSeconds(attackRate);
        }
    }
}
