using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathos : SoulShard
{
    private static readonly SoulShards _shardName = SoulShards.pathos;
    public override SoulShards ShardName => _shardName;

    protected static float _maxEnergy;
    public override float MaxEnergy { 
        get { return _maxEnergy; } 
        set => _maxEnergy = value; 
    }

    protected static float _currentEnergy;
    public override float CurrentEnergy {
        get { return _currentEnergy; }
        set => _currentEnergy = value;
    }

}
