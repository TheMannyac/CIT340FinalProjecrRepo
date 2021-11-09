using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathos : MonoBehaviour
{
    public static Pathos current;

    SoulShard_AttackManager stateHandler;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {

    }
    private void Awake()
    {
        current = this;
        stateHandler = GetComponent<SoulShard_AttackManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Logos.current.OnShardDealsDamage += OnLogosDealsDamage;
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

    //called when we are notified that Pathos has dealt damage.
    private void OnLogosDealsDamage()
    {
        Debug.Log(gameObject.name + "Has regained energy because of other shard");
        int energyRegained = 30;
        stateHandler.GainEnergy(energyRegained);
    }
}
