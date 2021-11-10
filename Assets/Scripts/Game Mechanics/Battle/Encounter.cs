using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Encounter
{
    //Hook in prefabs with that contain the "Enemy" script to create an encounter
    //Can only have up to 3 enemies per encounter
    public Enemy_BattleScene[] pfEnemiesInEncounter = new Enemy_BattleScene[3];

    //CHANGE LATER
    public float attackDuration = 6f;

    //Indicates if this fight should be "serious"
    public bool serious = false; 


    //this is where we will define any special encounter rules and conditions
}
