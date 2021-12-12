using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 * 
 * The intended order of battle is to go like this:===========
 * START, 
 * PLAYER TURN, BANTER (optiona;), FIGHT PAHSE, .... (repeat)
 * END
 * =========================================================
 */

public enum BattleState
{
    Start, PlayerTurn, FightPhase,
    Banter, End
}

public class BattleManager : MonoBehaviour
{
    //Singleton Pattern\\
    /* ================================================
     *   Create object if not already in scene:     false
     *  Remove scene dupes:                          true
     *  Global access                                 true
     *  Keep across game scene loads                  true
     * =========================================================
     */
    
    public static BattleManager instance;

    /// <summary>
    /// THe current singleton of the battlemanager 
    /// </summary>
    public static BattleManager Instance { get { return instance; } }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private BattleState _battleState = BattleState.PlayerTurn;
    /// <summary>
    /// Current battle state
    /// </summary>
    [SerializeField] public BattleState battleState {
        //anyone can check the battlestate current
        //but only the battleManager can change it
        get { return _battleState; }
        protected set
        {
            //Don't execute anything else if we're out 
            if (_battleState == value)
                return;
            //Debug.Log("State is going from " + _battleState + " to " + value);

            switch (value)
            {
                case BattleState.Start:
                    StartCoroutine(BattleStartCutscene());
                    break;
                case BattleState.PlayerTurn:
                    PlayerTurnEnter();
                    break;
                case BattleState.FightPhase:
                    EnemyTurnEnter();
                    break;
                case BattleState.Banter:
                    BanterEnter();
                    break;
                case BattleState.End:
                    GameManagment.goToMenu();
                    break;
            }

            //reset state timer
            stateTimer = 0;

            _battleState = value;
        }
    }
    //Singleton instance of BattleManager
   

    [Header("Player Settings")]
    //position where the player will spawn and return to between turns.
    [SerializeField] private Vector2 ArenaSpawn;
    [SerializeField] private GameObject arenaPF; 
    private GameObject arena;
    //

    [Header("Enemy Settings")]
    //How long the fight phase lasts
    public float fightDuration = 10;
    private float defaultFightDuration;
    //(In seconds) how long the current state has lasted 
    [HideInInspector] float stateTimer= 0;
    //The maximum number of enemies that can be in any given encounter 
    [SerializeField] public static int maxEncounterSize = 3;
    //The template for the current encounter 
    [SerializeField] private Encounter currentEncounter;
    //positions that enemies will spawn and return to between turns.
    [SerializeField] private Vector2[] enemyIdlePositions = new Vector2[maxEncounterSize];
    //where enemies will start in the arena at the begining of their turn
    [SerializeField] private Vector2[] enemyArenaPositions = new Vector2[maxEncounterSize];
    //Contains the instances of all enemies in the battle
    [HideInInspector] private Enemy_BattleScene[] currentEnemies = new Enemy_BattleScene[maxEncounterSize];
    /// <summary>
    /// The number of the current battleround 
    /// </summary>
    public int battleRound { get; protected set; }
    public object OnEnterBanter { get; internal set; }

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
                                            enemyIdlePositions[i], Quaternion.identity);
                //Make any needed additions or alterations to the enemy's datafields
                baddie.idlePosition = enemyIdlePositions[i];
                baddie.arenaPosition = enemyArenaPositions[i];

                //Stores an instance of the newly spawned enemy
                currentEnemies[i] = baddie;
            }
        }

        defaultFightDuration = fightDuration;
        battleRound = -1;
        battleState = BattleState.Start;
    }

    private void Update()
    {
        //timer increases gradually
        stateTimer += Time.deltaTime;

        if (battleState == BattleState.FightPhase)
        {
            //Debug.Log("fight timer is up");
            if (stateTimer >= fightDuration)
                battleState = BattleState.PlayerTurn;
        }
        //Debug.Log(stateTimer);

    }

    protected IEnumerator BattleStartCutscene()
    {
        //Create Dramatic black screen at the start that covers everything
        DramaticScreen bg = Instantiate(GameManagment.Instance.dramaScreePF, new Vector2(3, -23), Quaternion.identity);
        bg.onLoadCommand = DramaticScreen_OnLoadCommands.stayBlack;
        bg.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        bg.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;

        yield return new WaitForSeconds(1.1f);
        SoundManager.instance.PlaySound("cackle");

        yield return new WaitForSeconds(1.6f);
        SoundManager.instance.PlaySound("chaos");

        yield return new WaitForSeconds(1.5f);
        SoundManager.instance.PlayMusic("battleTheme");
        Destroy(bg.gameObject);
        battleState = BattleState.PlayerTurn;

    }


    //change this later; meant to be used with UI buttons
    public void endPlayerTurn()
    {
        //this function should only happen on the enemy's turn
        if (battleState != BattleState.PlayerTurn) return;
        battleState = BattleState.FightPhase;
    }

    public void endBattle()
    {
        if (battleState != BattleState.FightPhase) return;

        if (OnPlayerTurnEnter != null)
        {
            Debug.Log("player turn has begun");
            OnPlayerTurnEnter();
        }
        //destroy battle arena
        Destroy(arena);

        battleState = BattleState.Banter;

    }

    public void endTurn()
    {
        //this function should only happen on the enemy's turn
        if (battleState != BattleState.PlayerTurn) return;

        battleState = BattleState.FightPhase;
    }

    /// <summary>
    /// Event fires when teh banter step ends
    /// </summary>
    public Action OnBanterEnter;
    protected void BanterEnter()
    {
        gameObject.GetComponent<DialougeManager>().enabled = true;
        if(OnBanterEnter != null)
        {
            OnBanterEnter();
        }
    }

    /// <summary>
    /// Event that fires when the Enemy's turn begins
    /// </summary>
    public event Action OnEnemyTurnEnter;
    private void EnemyTurnEnter()
    {
        //Debug.Log("enemy turn has begun");
        arena = Instantiate(arenaPF, ArenaSpawn, Quaternion.identity);

        //disable menu
        BattleMenu.Instance.gameObject.SetActive(false);

        //fire off event
        if (OnEnemyTurnEnter != null) { OnEnemyTurnEnter();}

    }

    /// <summary>
    /// This event that fires when the player's turn begins
    /// </summary>
    public event Action OnPlayerTurnEnter;
    private void PlayerTurnEnter()
    {
        if (OnPlayerTurnEnter != null)
        {
            Debug.Log("player turn has begun");
            OnPlayerTurnEnter();
        }

        //destroy battle arena
        Destroy(arena);
        battleRound++;
        //reset timer
        fightDuration = defaultFightDuration;
        //enable menu
        BattleMenu.Instance.gameObject.SetActive(true);

    }

 
    private void OnDestroy()
    {
        //removes instance of this object from the static variable
        instance = null;
    }
    private void OnDrawGizmosSelected()
    {
        //Draw Player Battle Position
        Gizmos.DrawCube(ArenaSpawn, new Vector3 (.5f,.5f, .5f));

        //Draw Battle Positions of enemies 
        foreach (Vector2 battlePos in enemyIdlePositions)
            Gizmos.DrawSphere(battlePos, .25f);
        foreach (Vector2 battlePos in enemyArenaPositions)
            Gizmos.DrawSphere(battlePos, .35f);
    }


}
