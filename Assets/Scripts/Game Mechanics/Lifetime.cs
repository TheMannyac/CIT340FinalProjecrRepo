using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float timeToLive = 5;

    // Start is called before the first frame update
    void Start()
    {
        //calls 'killobject()' after 'timeToLive' seconds
        Invoke("KillObject", timeToLive);    
    }

    void KillObject()
    {
        Destroy(gameObject);
        //gameObject refers to the object this script is attached to
    }
}
