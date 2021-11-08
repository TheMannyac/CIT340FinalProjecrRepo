using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform pivotObject;
    public Vector3 pivotPoint;
    public float radius = -1;   //if radius is below 0 then it will automatically 
    public bool continuous = false;

    // Start is called before the first frame update
    void Start()
    {
        if (pivotObject != null)
        {
            pivotPoint = pivotObject.localPosition;
        }

        if (radius < 0)
            radius = (pivotPoint - transform.localPosition).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (pivotObject != null)
        {
            pivotPoint = pivotObject.localPosition;
        }

        if (continuous) { orbit(pivotPoint, radius); }
    }


   public void orbit(Vector2 pivotPoint,float rotationSpeed)
    {
        transform.RotateAround(pivotPoint, new Vector3(0,0,1), rotationSpeed * Time.deltaTime);
    }
}
