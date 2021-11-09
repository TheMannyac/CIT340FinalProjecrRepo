using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logos : MonoBehaviour
{
    public static Logos current;

    SoulShard_AttackManager stateHandler;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        
    }
    private void Awake()
    {
        stateHandler = GetComponent<SoulShard_AttackManager>();
        //This instacne of the object becomes the static reference
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Pathos.current.OnShardDealsDamage += OnPathosDealsDamage;
    }

    public event Action OnShardDealsDamage;
    public void ShardDealsDamage()
    {
        if (OnShardDealsDamage != null)
        {
            Debug.Log(gameObject.name + " has dealt damage");
            OnShardDealsDamage();
        }
    }

    //called when we are notified that Pathos has dealt damage.
    private void OnPathosDealsDamage()
    {
        //Debug.Log(gameObject.name + "Has regained energy because of other shard");
        int energyRegained = 30;
        stateHandler.GainEnergy(energyRegained);
    }
}
