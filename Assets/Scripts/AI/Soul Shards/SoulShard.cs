using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoulShard : MonoBehaviour
{
    [SerializeField] protected float maxEnergy;
    void Start()
    {
        
    }

    public event Action OnShardDealsDamage;
    public void ShardDealsDamage()
    {
        if (OnShardDealsDamage != null)
        {
            //Debug.Log(gameObject.name + " has dealt damage");
            OnShardDealsDamage();
        }
    }

}
