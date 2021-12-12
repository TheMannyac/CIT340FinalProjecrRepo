using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum SoulShards
{
    none,pathos,logos
}

public interface IHasEnergy
{
    /// <summary>
    /// The max amount of energy this shard can have at any given time
    /// </summary>
    abstract float MaxEnergy { get;  set; }
    /// <summary>
    /// The amount of energy that the object currently has
    /// </summary>
    abstract float CurrentEnergy { get; set; }

    /// <summary>
    /// Cleanly sets current energy back to max
    /// </summary>
    void ResetEnergy();
    /// <summary>
    /// Adds a certain amount of energy back to the current energy value, but (probably) doesn't let it go over
    /// </summary>
    /// <param name="energy"> amount of energy being added</param>
    void GainEnergy(float energy);
    /// <summary>
    /// Removes a certain amount of energy from current energy value, but never should go below zero;
    /// </summary>
    /// <param name="energy">amount of energy being removed</param>
    void DrainEnergy(float energy);
    /// <summary>
    /// Returns a decimal value representing the percentage of current energy/max energy
    /// </summary>
    /// <returns>energy fill percent</returns>
    float GetEnergyPercent();
}
public static class ShardManager
{
    private static SoulShards _ActiveShard = SoulShards.logos;
    /// <summary>
    /// The currently active shard
    /// </summary>
    public static SoulShards ActiveShard {
        get { return _ActiveShard; }
        private set
        {
            if (value == _ActiveShard)
                return;

            switch (value)
            {
                case SoulShards.logos:
                    InactiveShard = SoulShards.pathos;
                    break;
                case SoulShards.pathos:
                    InactiveShard = SoulShards.logos;
                    break;
                case SoulShards.none:
                    InactiveShard = SoulShards.none;
                    break;
            }

            _ActiveShard = value;
        }
    }
    /// <summary>
    /// The Soul shard that is current inactive and is on standby
    /// </summary>
    public static SoulShards InactiveShard { get; private set; }

    /// <summary>
    /// The active shard and the inactive shard swap places
    /// </summary>
    public static void swapActiveShard()
    {
        //can only swap between Pathos and Logos
        if (ActiveShard == SoulShards.none) { return; }

        ActiveShard = InactiveShard;
    }

}


