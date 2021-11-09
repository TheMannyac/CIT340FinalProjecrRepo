using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
    }

    public event Action OnShardDealsDamage_Pathos;
    public void ShardDealsDamage_Pathos()
    {
        if(OnShardDealsDamage_Pathos != null)
        {
            OnShardDealsDamage_Pathos();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
