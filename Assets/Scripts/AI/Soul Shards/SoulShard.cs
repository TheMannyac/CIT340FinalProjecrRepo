using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class SoulShard : MonoBehaviour, IHasEnergy
{
    //Soul Shard Fields
    [Header("General Soul Shard Fields")]
    public Color shardColor;   //The color used to tint all of this shard's effects
    [SerializeField] protected Slider energyBar;


    //Soul Shard Properties
    /// <summary>
    /// enum idenifier that labels which shard this is for quick comparisons
    /// </summary>    
    public abstract SoulShards ShardName { get; }

    protected virtual void Start()
    {
        ResetEnergy();
    }

    //SoulShard Private Methods
    /// <summary>
    /// Utility function that updates the energy bar to match the value
    /// </summary>
    protected virtual void UpdateEnergyBar()
    {
        SliderScript slid = energyBar.GetComponent<SliderScript>();

        if (slid)
            slid.setFillPercent(GetEnergyPercent());
        else
            //assumes that all slider are percentage based
            energyBar.value = GetEnergyPercent();
    }

    //======================================================

    //Implemented Fields
    public abstract float MaxEnergy { get; set; }
    public abstract float CurrentEnergy { get; set; }
    public Color ShardColor { get => shardColor; set => shardColor = value; }

    //Implemented Methods
    public void DrainEnergy(float energy)
    {
        //apply change to current energy
        CurrentEnergy = Math.Max(CurrentEnergy - energy, 0);

        //Update Energy Bar
        UpdateEnergyBar();
    }

    public void GainEnergy(float energy)
    {
        throw new NotImplementedException();
    }

    public void ResetEnergy() {CurrentEnergy = MaxEnergy; }

    public float GetEnergyPercent() { return CurrentEnergy / MaxEnergy; }
}
