using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BattleScene_Ghost : Enemy_BattleScene
{
    public GameObject bulletPrefab;
    public float attackRate = .5f;

    public float desiredTime = 5; //number of seconds it takes to move between each waypoint
    private float elapsedTime;
    [SerializeField] private AnimationCurve speedCurve;
    public float distanceThreashold = .75f;

    private Vector2 targetWaypoint;
    private Vector2 prevWaypoint;
    private int targetWaypointIndex;

    private float shotTimer;

    private WaypointManager WaypointMan;

    protected override void Awake()
    {
        base.Awake();
        //Retrieve all the components
        WaypointMan = GetComponent<WaypointManager>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        shotTimer = Random.Range(0f, .4f);
        //Ways of getting another script/object's data, Method 2
        //Slowest, but works for objects that didn't exist on game start
        //waypointManager = FindObjectOfType<WaypointManager>();

        //Dynamically adding components:
        //gameObject.AddComponent(typeof(Tower));

        //Ways of getting another script/object's data, Method 3

        //Set the waypoints
        prevWaypoint = WaypointMan.waypoints[WaypointMan.waypoints.Count-1];
        targetWaypoint = WaypointMan.waypoints[0];
    }

    // Update is called once per frame
    protected override void Update()
    {
        switch (myState)
        {
            case State.Attack:
                //Execue attack logic
                Attack();
                break;
            case State.Idle:
                //Play any relevant animations or visual effects 

                //Make sure that we are facing left
                sr.flipX = false;
                break;
        }
    }

    protected override void OnEnemyTurnEnter()
    {
        Debug.Log(name + " knows that its turn has began");
        base.OnEnemyTurnEnter();
    }

    protected override void OnPlayerTurnEnter()
    {
        //Debug.Log(name + " knows that its turn has ended");
        base.OnPlayerTurnEnter();

    }

    protected override void Attack()
    {

        //Begin attack [DO NOT KEEP THIS]
        if (shotTimer > attackRate)
        {
           
            Instantiate(bulletPrefab,transform.position,Quaternion.identity);
            //bullet.GetComponent<Projectile>().targetLayer = 
            shotTimer = 0;
        } else
        {
            shotTimer += Time.deltaTime;
        }
           
        //Movement 
        Vector2 directionToMove = targetWaypoint - (Vector2)transform.position;
        float distance = directionToMove.magnitude;
        if (distance < distanceThreashold)
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
        transform.position = Vector2.Lerp(prevWaypoint, targetWaypoint, speedCurve.Evaluate(percentageComplete));

        //Flips sprite to face direction object is going in
        directionToMove.Normalize();
        if (directionToMove.x > .01)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

}
