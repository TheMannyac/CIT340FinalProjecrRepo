using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSpin : MonoBehaviour
{
    Vector3 originalScale;
    Vector3 currentScale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Mathf.lerp give us a value between -originalscale.x and +originalScale.x
        //PingPoing goes back and forth between 1 and 0 over time.
        currentScale.x = Mathf.Lerp(-originalScale.x, originalScale.x,
            Mathf.PingPong(Time.time, 1));
        transform.localScale = currentScale;
    }
}
