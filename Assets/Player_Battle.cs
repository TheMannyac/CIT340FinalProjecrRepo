using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Battle : MonoBehaviour
{

    Rigidbody2D rb;

    //Singleton Pattern\\
    /* ================================================
     *  Create object if not already in scene:     false
     *  Remove scene dupes:                          false
     *  Global access                                 true
     *  Keep across game scene loads                  false
     * =========================================================
     */

    public static Player_Battle instance;

    /// <summary>
    /// THe current singleton instance of this component 
    /// </summary>
    public static Player_Battle Instance { get { return instance; } }


    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning(name + "'s" + GetType() + " was removed since "
                             + instance.gameObject.name + "'s already exists");
            //destroys the attached component NOT the game object
            Destroy(this);
        }
        else
        {
            instance = this;
        }

    }

    private void OnDisable()
    {
        if(instance == this)
        {
            instance = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Player has entered trigger");
        if (collision.tag == "Projectile")
        {
           // Debug.Log("player hit by projectile");
            Projectile proj = collision.GetComponent<Projectile>();

            if (proj.Alignment == ProjectileAllignment.Enemy)
            {
                Debug.Log("player hit by enemy projectile");
                Stylo.TakeDamage(proj.damage);
            }
        }
    }
}
