using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemy : MonoBehaviour
{
 
    public int damage = 20;   //base damage that enemy will inflict on hit.

    public GameObject target;  //thing that the 
 
    private Rigidbody2D rb;
   // private TargetJoint2D;

    // Start is called before the first frame update
    void Start()
    {
      
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bool killedTarget = collision.gameObject.GetComponent<Health>().changeHP(damage);
            
        }
     
    }

}
