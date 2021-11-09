using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    //remember to initialize your lists with = new List...
    public List<Vector2> waypoints = new List<Vector2>();
    
    //public static List<Vector2> staticWaypoints;

    //Awake runs before any Start() runs.
    private void Awake()
    {
        //staticWaypoints = waypoints;
    }

    private void Start()
    {
        
    }


    //This runs in the EDITOR, not the GAME, to show 
    private void OnDrawGizmosSelected()
    {
        foreach (Vector2 waypoint in waypoints)
            Gizmos.DrawSphere(waypoint, .25f);
    }

}
