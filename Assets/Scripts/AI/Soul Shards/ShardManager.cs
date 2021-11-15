using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShardManager : MonoBehaviour
{

    [Header("Logos Settings")]
    [SerializeField] private int maxEnergy_Logos = 100;
    public int currenEnergy_Logos;
    public Slider LogosEnergyBar;

    [Header("Pathos Settings")]
    [SerializeField]private int maxEnergy_Pathos = 100;
    private int currenEnergy_Pathos;
    public Slider PathosEnergyBar;

    public static ShardManager current;
    private void Awake()
    {
         if(PathosEnergyBar == null)
        {
            Debug.LogError("Pathos energy bar not given to shard manager");
        }
        if (LogosEnergyBar == null)
        {
            Debug.LogError("Logos energy bar not given to shard manager");
        }
    }

    private void Start()
    {
        if(current == null)
        {
            current = this;
        } else
        {
            Debug.LogWarning(name + "'s shard Manager component has been destroyed!!");
            Destroy(this);
            return;
        }

        currenEnergy_Logos = maxEnergy_Logos;
        currenEnergy_Pathos  = maxEnergy_Logos;

        LogosEnergyBar.GetComponent<SliderScript>().setFillPercent((float)currenEnergy_Pathos / maxEnergy_Pathos);
        LogosEnergyBar.GetComponent<SliderScript>().setFillPercent((float)currenEnergy_Logos / maxEnergy_Logos);
    }
    //public static bool activeShardIsPathos = true;

    //Reference to the current shard that is active
    //public static SoulShard_Battle_Active activeShard;
    //Reference to the shard that is 
    //public static SoulShard_Battle_Assist assistShard;

    public static event Action OnSwapActiveShard;
    public static void SwapActiveShard()
    {
        if(OnSwapActiveShard != null)
        {
            Debug.Log("Active Shard is now swapped");
            OnSwapActiveShard();
        }
    }
   

    //THIS IS SLOPPY, WE'LL CHANGE THIS LATER
    public void GainEnergy(int energyGain, bool isPathos)
    {
        if (isPathos)
        {
            currenEnergy_Logos = Mathf.Min(currenEnergy_Pathos + energyGain, maxEnergy_Pathos);

            LogosEnergyBar.GetComponent<SliderScript>().setFillPercent((float) currenEnergy_Pathos/maxEnergy_Pathos);
        } 
        else
        {
            currenEnergy_Logos = Mathf.Min(currenEnergy_Pathos + energyGain, maxEnergy_Pathos);

            LogosEnergyBar.GetComponent<SliderScript>().setFillPercent((float)currenEnergy_Logos / maxEnergy_Logos);
        }
    }

    public void LoseEnergy(int energyCost, bool isPathos)
    {
        if (isPathos)
        {
            currenEnergy_Logos = Mathf.Max(currenEnergy_Pathos - energyCost, maxEnergy_Pathos);

            LogosEnergyBar.GetComponent<SliderScript>().setFillPercent((float)currenEnergy_Pathos / maxEnergy_Pathos);
        }
        else
        {
            currenEnergy_Logos = Mathf.Min(currenEnergy_Pathos + energyCost, maxEnergy_Pathos);

            LogosEnergyBar.GetComponent<SliderScript>().setFillPercent((float)currenEnergy_Logos / maxEnergy_Logos);
        }
    }

    public int GetCurrentEnergy_Logos()
    {
        return currenEnergy_Logos;
    }

    public int GetCurrentEnergy_Pathos()
    {
        return currenEnergy_Pathos;
    }
}
