using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_BattleScene : MonoBehaviour
{
    protected enum State
    {
        Idle, Attack,
    }

    [SerializeField] protected State myState;
    

    [Header("Stats")]
    [SerializeField] protected int maxHP; 
    [HideInInspector] protected int currentHP;
    [SerializeField] protected int attackStat;
    [SerializeField] protected int defenseStat;

    [HideInInspector] public Vector2 idlePosition; //where this enemy returns to between turns; is assigned by the battle Manager
    [HideInInspector] public Vector2 arenaPosition; //where this enemy starts during enemy turn

    protected bool isFacingLeft = true;   //default direction during battle

    //Required Components
    protected Rigidbody2D rb;
    protected Collider2D myCollider;
    protected SpriteRenderer sr;
    protected Animator anim;
    public Slider healthBar;
    
    protected virtual void Awake()
    {
        //Grab references to relevant componenets before anything else
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if(healthBar == null)
        {
            Debug.LogWarning(name + "does not have a health bar slider object attached");
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHP = maxHP;
        //All Enemies with this script must have the Enemies Layer mask
        gameObject.layer = LayerMask.NameToLayer("Enemies");
        
        //Makes sure that we always know who's turn it is
        BattleManager.instance.OnEnemyTurnEnter += OnEnemyTurnEnter;
        BattleManager.instance.OnEnemyTurnExit += OnEnemyTurnExit;
    }

    //Determines how this enemy will attack next 
    protected virtual void Attack()
    {
        Debug.LogWarning(name + "hasn't overridden this method yet");
    }
    protected virtual void  OnEnemyTurnEnter()
    {
        //Debug.Log(name + " knows that its turn has begun");
        //travel to Arena
        transform.position = arenaPosition;

        //enable colliders
        myCollider.enabled = true;

        //change the state
        myState = State.Attack;

    }
    protected virtual void Update()
    {
        switch (myState)
        {
            case State.Attack:
                //Execue attack logic
                Attack();
                break;
            case State.Idle:
                //Play any relevant animations or visual effects 

                //Make sure that we are facing left
                sr.flipX = false;
                break;
        }
    }

    //called when the battle manager lets us know when we end our turn.
    protected virtual void OnEnemyTurnExit()
    {
        //Go back to idling
        myState = State.Idle;

        //disable colliders
        myCollider.enabled = false;

        //travel back to idle position
        transform.position = idlePosition;
    }

    //Restores the health of a character 
    public void HealDamage(int healing)
    {
        //Play visual/audio effects

        //Update health
        currentHP = Mathf.Max(currentHP + healing, maxHP);

        //Update HealthBar
        healthBar.GetComponent<SliderScript>().setFillPercent((float)currentHP / maxHP);
    }

    public void TakeDamage(int damage)
    {
        //Play visual/Audio Effects

        //Update Health
        //can't go below 0 (Might change this later for "shenanigans")
        currentHP = Mathf.Min(currentHP - damage, 0);

        // Update HealthBar
        healthBar.GetComponent<SliderScript>().setFillPercent((float)currentHP / maxHP);

        //Die if we go below 0
        if (currentHP >= 0)
        {
            Die();
        }
    }

    //called when this enemy's HP reaches zero
    protected virtual void Die()
    {
        //Play any relevant effects

        //Destroy the game Object
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(arenaPosition, .5f);
    }
}
