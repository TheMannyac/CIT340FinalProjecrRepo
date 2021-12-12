using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBox : MonoBehaviour
{
    //Singleton Pattern\\
    /* 
     *   Create object if not already in scene:     false
     *  Remove scene dupes:                          true
     *  Global access                                 true [might restrict to battle manager]
     *  Keep across game scene loads                  true
     * 
     */
    private static BattleBox instance;

    public static BattleBox Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    //where the 
    [SerializeField] private Vector2 playerSpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        //Create player object 
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(playerSpawnPoint,.5f);
    }
}
