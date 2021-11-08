using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToward : MonoBehaviour
{
    public float angleOffset = 0;
    //public bool targetMouse; = true;
    [SerializeField] private AnimationCurve rotationFeel;
    private Vector3 directionVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        Vector3 difference  = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
       // mouseWorldPosition.z = 0;   //mouse z is naturally same as camera
        //directionVector = mouseWorldPosition.normalized;

        //rotate toward an object
        //transform.right = directionVector;
        //transform.Rotate(Vector3.Lerp());// (new Vector3(0, 0, angleOffset));
    }
}
