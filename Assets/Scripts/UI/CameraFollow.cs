using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject objectToFollow;
    public float lerpPerFrame = .01f;
    public bool lockY = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 objPos = objectToFollow.transform.position;
        //objPos = Vector3.Lerp(transform.position, objPos, .01f);
        //transform.position = new Vector3(objPos.x, objPos.y, transform.position.z);
        objPos.z = transform.position.z;
        if (lockY)
            objPos.y = transform.position.y;

        transform.position = Vector3.Lerp(transform.position, objPos, lerpPerFrame);
    }
}
