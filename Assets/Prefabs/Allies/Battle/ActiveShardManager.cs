using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// This script acts as a middleman for the active shard behaviors common between both of them
/// and it aids in swapping
/// </summary>
public class ActiveShardManager : MonoBehaviour
{

    SpriteRenderer spr;

    /// <summary>
    /// The soul shard behavior script that is currently being used
    /// </summary>
    [SerializeField] protected ActiveShard activeShardScript;

    private void Awake()
    {
        switch (ShardManager.ActiveShard)
        {
            case SoulShards.logos:
                Debug.Log("active shard has spawned as Logos");
                activeShardScript = gameObject.AddComponent<Logos_Active>();
                break;
            case SoulShards.pathos:
                Debug.Log("active shard has spawned as Pathos");
                activeShardScript = gameObject.AddComponent<Pathos_Active>();
                break;
            case SoulShards.none:
            default:
                //there shouldn't even be an active shard
                break;
        }

    }

    //====================ATTACK LOGIC==========================

    protected Coroutine _behavior;
    public Coroutine Behavior 
    {
        get => _behavior;
        protected set
        {
            //duplicate assignments will do nothing
            if (value == _behavior) return;

            //if there is another coroutine already playing turn it off
            if (_behavior != null)
                StopCoroutine(_behavior);

            //assign the value
            _behavior = value;
        }
    }

    public bool FireButtonHeld { get; protected set; } 

    [Header("Temp inspector variables")]
    public Projectile Logos_ProjPF;
    public Projectile Pathos_ProjPF;
    private void Update()
    {

        //grab player input
        if (Input.GetAxis("Fire1") == 1 && Behavior != null)
        {
            Debug.Log("Active shard attack button is being held");
            FireButtonHeld = true;
            Behavior = activeShardScript.UseEquippedAttack();
        }
    }

    //UTILITY FUNCTIONS---------------------------

    /// <summary>
    /// Utility funciton that returns 
    /// </summary>
    public Quaternion GetMouseDirection()
    {
        //Find Mouse world position
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;   //mouse z is naturally same as camera

        //Get the direction of the mouse
        return Quaternion.FromToRotation(Vector3.right, mouseWorldPosition - transform.position);
    }

}

/// <summary>
/// Interface implemented by active shard scripts to help define interactions between manager and active shard
/// </summary>
public interface ActiveShard
{
    public abstract ActiveShardManager MyManager { get; set; }
    public abstract bool CanAttack { get; }

    public Coroutine UseEquippedAttack();

    public Color GetShardColor();

}

    

