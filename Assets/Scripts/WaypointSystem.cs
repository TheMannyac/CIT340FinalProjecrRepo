using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    public List<Transform> waypoints;
    public float distanceThreshold = .2f;
    public float speed = 2;
    public bool circular = false;
    public bool automaticallyExecute = false;

    public int TargetWaypoint { get; protected set; }
    protected Rigidbody2D body;
    protected SpriteRenderer spr;
    protected bool reverse = false;


    private void Awake()
    {
        //gets a special set of waypoints if we are dabox
        DaBozz db = GetComponent<DaBozz>();
        if (db)
        {   
            waypoints.Clear();
            waypoints = new List<Transform>(4);
            foreach(Vector3 pos in db.knives_waypointList)
            {
                Transform wp = Instantiate(new GameObject("Dabozz_mudaAttack_waypoint").transform, pos, Quaternion.identity);
                waypoints.Add(wp);
            }
        }
    }


    // Use this for initialization
    void Start ()
    {

        if(body == null)
            body = GetComponent<Rigidbody2D>();
        
        //Pick closest waypoint to start going to it.
        if (waypoints.Count == 0)
            return;

        float dist = float.MaxValue;
        for(int i =0; i < waypoints.Count; ++i)
        {
            float distToWaypoint = Vector3.Distance(transform.position, waypoints[i].position);
            if (distToWaypoint < dist)
            {
                TargetWaypoint = i;
                dist = distToWaypoint;
            }
        }
    }
	
    public bool MoveToNextWaypoint(Rigidbody2D rb, float speed)
    {
        if (waypoints.Count == 0)
            return false;

        bool reached;

        //Pick new waypoint if reached the current one
        if (Vector3.Distance(transform.position, waypoints[TargetWaypoint].position) < distanceThreshold)
        {
            
            reached = true;
            if (circular)
            {
                if (TargetWaypoint == waypoints.Count - 1)
                    TargetWaypoint = 0;
                else
                    TargetWaypoint++;
            }
            else
            {
                if (TargetWaypoint == waypoints.Count - 1)
                    reverse = true;
                else if (TargetWaypoint == 0)
                    reverse = false;

                if (reverse)
                    TargetWaypoint--;
                else
                    TargetWaypoint++;
            }
        } else { reached = false; }

        //Get direction to current waypoint
        Vector3 direction = waypoints[TargetWaypoint].position - transform.position;
        direction = direction.normalized;

        //Flip the sprite based on direction
        try
        {
            if (direction.x < 0)
                GetComponent<SpriteRenderer>().flipX = true;
            else
                GetComponent<SpriteRenderer>().flipX = false;
        } catch(MissingComponentException e)
        {
            Debug.LogWarning(e.StackTrace);
        }
        

        //Move towards the waypoint
        if (rb)
            body.velocity = direction * speed;
        else
            transform.Translate(direction * speed * Time.deltaTime);

        return reached;
    }

    //public void setWaypointList

    private void OnDisable()
    {
        
    }

    /*
     *     /// <summary>
    /// Psudo constructor for waypoint system
    /// </summary>
    public static WaypointSystem CreateWaypointSystem(Rigidbody2D rb, List<Transform> waypointList, float speed, float distThreashold, bool circular)
    {
        GameObject go = new GameObject("wpInstance");
        go.SetActive(false);

        WaypointSystem wpSystem = go.AddComponent<WaypointSystem>();

        //set all of the fields
        wpSystem.body = rb;
        wpSystem.waypoints = waypointList;
        wpSystem.speed = speed;
        wpSystem.distanceThreshold = distThreashold;

        //return the waypoint system component
        return wpSystem;
    }
    */
}
