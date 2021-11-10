using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private enum BattleState
    {
        Start, PlayerTurn, EnemyTurn,
        Banter,End
    }

    [Header("BattleManager Settings")]
    //Current State of the battle
    [SerializeField] private BattleState battleState;
    //Singleton instance of BattleManager
    [HideInInspector] public static BattleManager instance;

    [Header("Player Settings")]
    //position where the player will spawn and return to between turns.
    [SerializeField] private Vector2 playerBattleStation;

    [Header("Enemy Settings")]
    //The template for the current encounter 
    [SerializeField] private Encounter currentEncounter;
    //positions that enemies will spawn and return to between turns.
    [SerializeField] private Vector2[] enemyBattleStations = new Vector2[3];
    //Contains the instances of all enemies in the battle
    [HideInInspector] private Enemy_BattleScene[] currentEnemies = new Enemy_BattleScene[3];

    private void Awake()
    {
        //ensures there is only one battle manager at a time
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("BATTLEMANAGER DESTROYED");
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Start by instantiating all the enemies in the encounter
        for(int i = 0; i < currentEncounter.pfEnemiesInEncounter.Length; i++)
        {
            Enemy_BattleScene enem = currentEncounter.pfEnemiesInEncounter[i];
            if(enem != null)
            {
                //spawns enemy at their assigned Battle station
                Enemy_BattleScene baddie = Instantiate(enem,
                                            enemyBattleStations[i], Quaternion.identity);
                //Make any needed additions or alterations to the enemy's datafields
                baddie.battleStation = enemyBattleStations[i];

                //Stores an instance of the newly spawned enemy
                currentEnemies[i] = baddie;
            }
        }
 
        battleState = BattleState.Start;
    }

    // Update is called once per frame
    void Update()
    {
        switch (battleState)
        {
            case BattleState.Start: //Initialize battle
                //Play any necesary animations or sound effects
                //Debug.Log("Battle Start");
                //Display Opening Dialouge

                //Move on to Player's turn
                    SetBattleState(BattleState.PlayerTurn);
                    break;
            case BattleState.PlayerTurn:  //This is when the player selects actions in the menu
                Debug.Log("its now the player's turn");
                break;
            case BattleState.EnemyTurn:

                break;
            case BattleState.Banter:      //Time in between turns that characters have dialog conversations
                break;
            case BattleState.End:   //Stuff to do before we return to the overworld
                break;
        }
    }

    private IEnumerator enemyTurnDuration()
    {
        Debug.Log("Enemy Turn has begun");
        float elapsedTime = 0;
        while (elapsedTime < currentEncounter.attackDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //after attack has ended got back to player's turn
        SetBattleState(BattleState.PlayerTurn);
    }

    //Allows for the battle state to be manually changed
    //while executing any cleanup code up before the transition
    //TODO: maybe create/find "OnStateChange" event to handle this instead
    private void SetBattleState(BattleState newBattleState)
    {
        //Don't execute anything else if we're out 
        if (battleState == newBattleState)
            return;

        if (battleState == BattleState.EnemyTurn)
        {
            if(OnEnemyTurnExit != null)
            {
                Debug.Log("OnEnemyTurnExit event has fired");
                OnEnemyTurnExit();
            }
        }

        //Execute any special cleamup code before setting the new state 
        switch (newBattleState)
        {
            case BattleState.Start: 
                break;
            case BattleState.PlayerTurn: 
                //Activate 
                break;
            case BattleState.EnemyTurn:
                //Let everyone know that we're about to fight
                if (OnEnemyTurnEnter != null)
                {
                    Debug.Log("OnEnemyTurnEnter has fired");
                    OnEnemyTurnEnter();
                }
                StartCoroutine(enemyTurnDuration());
                break;
            case BattleState.Banter:    
                break;
            case BattleState.End:   
                break;
        }

        //Set battleState
        battleState = newBattleState;
    }

    //change this later
    public void endTurn()
    {
        if (battleState == BattleState.PlayerTurn)
        {
            SetBattleState(BattleState.EnemyTurn);
        }
    }

    public event Action OnEnemyTurnEnter;
    private void EnemyTurnEnter()
    {
        if (OnEnemyTurnEnter != null)
        {
            //Debug.Log(gameObject.name + " has dealt damage");
            OnEnemyTurnEnter();
        }
    }

    public event Action OnEnemyTurnExit;
    private void EnemyTurnExit()
    {
        if (OnEnemyTurnExit != null)
        {
            //Debug.Log(gameObject.name + " has dealt damage");
            OnEnemyTurnExit();
        }
    }
    
    private void OnDestroy()
    {
        //removes instance of this object from the static variable
        instance = null;
    }
    private void OnDrawGizmosSelected()
    {
        //Draw Player Battle Position
        Gizmos.DrawCube(playerBattleStation, new Vector3 (.5f,.5f, .5f));

        //Draw Battle Positions of enemies 
        foreach (Vector2 battlePos in enemyBattleStations)
            Gizmos.DrawSphere(battlePos, .25f);
    }

    /// <summary>
    /// Public version of state mutator that converts a string input before setting the state
    /// </summary>
    /// <param name="stateName"> string representing the new battle state</param>
  public void SetBattleState(string stateName)
  {
        BattleState newBattleState;

      switch (stateName.ToUpperInvariant())
      {
          case "START":
                newBattleState = BattleState.Start;
              break;
          case "PLAYER_TURN":
                newBattleState = BattleState.PlayerTurn;
                break;
          case "ENEMY_TURN":
                newBattleState = BattleState.EnemyTurn;
                break;
          case "BANTER":
                newBattleState = BattleState.Banter;
                break;
          case "END":
                newBattleState = BattleState.End;
                break;
          default:
              Debug.LogWarning(stateName + "is an invalid battleState");
                return;
      }
        //set this to the new state
        SetBattleState(newBattleState);
  }

}
