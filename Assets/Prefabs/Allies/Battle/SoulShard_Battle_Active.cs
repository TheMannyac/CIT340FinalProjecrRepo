using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulShard_Battle_Active : MonoBehaviour
{
    [SerializeField] private bool isPathos = true;//determines which shard we are right now

    //bool isPathos = true; 
    //public static SoulShard_Battle_Active current; 

    SoulShard_AttackManager attackMan;
    SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {

        //Grab refernces to components
        sr = GetComponent<SpriteRenderer>();
        attackMan = GetComponent<SoulShard_AttackManager>();

        //Subscribe to the Shard Manager's shardSwap event
        ShardManager.OnSwapActiveShard += OnSwapActiveShard;

        setShardSettings();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnSwapActiveShard()
    {
        Debug.Log(name + " will now swap active shards");
        //swaps which shard is the active one
        isPathos = !(isPathos);
        setShardSettings();
    }

    private void setShardSettings()
    {

        //change settings depending on which shard we are
        if (isPathos)
        {
            sr.color = Color.blue;
            attackMan.SwapAttack();
        }
        else
        {
            sr.color = Color.yellow;
            attackMan.SwapAttack();
        }
    }

    private void OnDestroy()
    {
        ShardManager.OnSwapActiveShard -= OnSwapActiveShard;
    }
}
