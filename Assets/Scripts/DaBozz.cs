using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DabozzAttacks
{
    Meteor,TeleShoot1,TeleShoot2,StarKnives1,CheckmateDa
}
public class DaBozz : Enemy_BattleScene
{
    //FIELDS
    [SerializeField] private List<DabozzAttacks> attackOrder;
    public float moveSpeed = 4.0f;
    public float angleOffset = 0;
    Transform target;
    /// <summary>
    /// normalized Vector from this enemy to the target
    /// </summary>
    Vector3 targDir;
    /// <summary>
    /// world distance from target
    /// </summary>
    float targDist;

    public Dialouge postBattleText;

    private bool usingWaypoints = false;
    private bool banter = false;
    

    private Coroutine _behavior;
    /// <summary>
    /// The current behavior of this enemy
    /// </summary>
    protected Coroutine behavior
    {
        get { return _behavior; }
        set
        {
            //Stop duplicate calls from executing 
            if (_behavior == value) return;
            //Stop the old coroutine if its still playing
            if (_behavior != null)  {StopCoroutine(_behavior);}
            //set value
            _behavior = value;
        }
    }

    protected override void Start()
    {
        base.Start();
        knives_waypointer = GetComponent<WaypointSystem>();

        BattleManager.instance.OnBanterEnter += OnBanterEnter;
        //entrance
        //SoundManager.instance.PlaySound("chaos");
    }

    protected override void Update()
    {
        try
        {
            target = Player_Battle.instance.transform;
        } catch(NullReferenceException e)
        {
            target = null;
        }

        //figure out direction of target
      if(target != null)
        {
            targDir = (Player_Battle.instance.transform.position - transform.position).normalized;
            targDist = targDir.magnitude;
            targDir.Normalize();

            //if not using waypoints
            if(usingWaypoints == false && anim.GetBool("banter") == false)
            {
                //flip the sprite
                if (targDir.x < 0 )
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }
        }
    }

    protected override void OnEnemyTurnEnter()
    {
        base.OnEnemyTurnEnter();
        
        //determine whicch attack to use
        Attack();
    }

    protected override void OnPlayerTurnEnter()
    {
        //unlink from arena
        //transform.parent = null;

        //if we still have the waypoint system remove it
        //if (muda_waypointer != null)
        // Destroy(muda_waypointer);
        rb.velocity = Vector2.zero;
        anim.SetBool("TeleShoot", false); 


        //Stop current attack coroutine
        if (behavior != null)
            StopCoroutine(behavior);
        

        base.OnPlayerTurnEnter();
    }

    protected void OnBanterEnter()
    {
        anim.SetBool("banter", true);
        sr.flipX = false;

        SoundManager.instance.PlayMusic("circus");
        BattleManager.instance.GetComponent<DialougeManager>().StartDialouge(postBattleText);
        GameObject styloSprite = GameObject.Find("Stylo");
        styloSprite.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
        styloSprite.GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    public override void TakeDamage(int damage)
    {
        SoundManager.instance.PlaySound("jevilHurt");
        base.TakeDamage(damage);
    }

    //=======================================
    /// <summary>
    /// sets the attack based on the battle round
    /// </summary>
    protected override void Attack()
    {
        int attackIndex = BattleManager.instance.battleRound % attackOrder.Count;
        DabozzAttacks currentAttack = attackOrder[attackIndex];

        //Attack based on battle round
        switch (currentAttack)
        {
            default:
            case DabozzAttacks.TeleShoot1:
                Debug.Log("ROUND 1");
                behavior = StartCoroutine(TeleShoot_Shotgun(teleShoot1_bulletPF, teleShoot1_bulletNum, teleShoot1_bulletSpreadAngle,teleShoot1_shootLag,teleShoot1_teleportLag));
                break;
            case DabozzAttacks.TeleShoot2:
                Debug.Log("ROUND 2");
                behavior = StartCoroutine(TeleShoot_Burstfire(teleShoot2_bulletPF, teleShoot2_bulletNum, teleShoot2_refireTime, teleShoot2_shootLag, teleShoot2_teleportLag));
                break;
            case DabozzAttacks.StarKnives1:
                Debug.Log("ROUND 3");
                behavior = StartCoroutine(StarKniveAttack(starKnivePF, moveSpeed, knives_KniveInterval, knives_knifeTriggerDelay, knives_resetDealy));
                break;
            case DabozzAttacks.Meteor:
                Debug.Log("ROUND 4");
                behavior = StartCoroutine(DragonStorm(meteor_interval));
                break;
            case DabozzAttacks.CheckmateDa:
                Debug.Log("ROUND 5");
                behavior = StartCoroutine(CheckmateDa(teleShoot3_kniveNum, teleShoot3_shootLag, teleShoot3_targetShootLag));
                break;
        }
    }

    [Header("TeleShoot Shotgun")]
    public Projectile teleShoot1_bulletPF;
    [Range(0, 10)] public float teleShoot_teleRadius = 6.0f;
    [Range(0, 7)] public float teleShoot_minTeleRadius = 3.5f;
    [Range(1, 20)] public int teleShoot1_bulletNum = 4;
    [Range(0, 359)] public float teleShoot1_bulletSpreadAngle = 80;
    [Range(0, 5)] public float teleShoot1_shootLag = 1.5f;
    [Range(0, 5)] public float teleShoot1_teleportLag = .5f;
 
    /// <summary>
    /// Dabozz Teleports somewhere random in the area before firing a 3 bullet burst in a shotgun spread
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="bulletNum"></param>
    /// <param name="bulletSpread"></param>
    /// <param name="attackInterval"></param>
    /// <param name="attackDelay"></param>
    /// <returns></returns>
    protected IEnumerator TeleShoot_Shotgun(Projectile prefab, int bulletNum, float bulletSpread, float attackInterval, float attackDelay)
    {
        anim.SetBool("TeleShoot", true);

        Vector3 centerpoint = BattleBox.Instance.transform.position;
        //wait a little bit before 
        yield return new WaitForSeconds(.75f);
        while (true)
        {
            //Fire shotgun attack
            SoundManager.instance.PlaySound("OH");
            anim.SetTrigger("shoot");
            Projectile.ScatterBurstAttack(teleShoot1_bulletPF,transform.position, bulletNum, targDir, bulletSpread);
            yield return new WaitForSeconds(attackInterval);

            //Teleport to a random place in the Battle Arena
            transform.position = RandomTeleportDestination(centerpoint, teleShoot_teleRadius, teleShoot_minTeleRadius);
           
            //Delay between teleport and next attack
            yield return new WaitForSeconds(attackDelay);
        }
    }
//========================================================================================
    [Header("Dragon Storm")]
    public Meteor meteorPF;
    [Range(0, 10)] public float meteor_interval;
    [Range(0, 10)] public float meteor_xOffset;
    [Range(0, 10)] public float meteor_yOffset;

    protected IEnumerator DragonStorm(float interval)
    {
        SoundManager.instance.PlaySound("chaos");
        BattleManager.instance.fightDuration *= 1.5f;

        yield return new WaitForSeconds(.5f);
        while (true)
        {
            //Define spawnpoint of the meteor
            Vector3 metorSpawnpoint  = BattleBox.Instance.transform.position;
            metorSpawnpoint.x +=  UnityEngine.Random.Range(-(meteor_xOffset + 3),(meteor_xOffset + 3));
            metorSpawnpoint.y = 13 + UnityEngine.Random.Range(0, meteor_yOffset);

            //Instansiate meteor
            Instantiate(meteorPF, metorSpawnpoint,Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }

//==============================================================================================
    [Header("Star Knives Attack")]
    public StarKnive starKnivePF;
    public float knives_KniveInterval = .5f;
    public float knives_knifeTriggerDelay = .6f;
    public float knives_resetDealy = 1.5f;
    public float knives_distThreashold = .2f;
    public List<Vector3> knives_waypointList;
    private WaypointSystem knives_waypointer;

    protected IEnumerator StarKniveAttack(StarKnive prefab, float speed, float attackInterval, float triggerDelay, float resetDelay)
    {
        transform.position = knives_waypointList[0];

        while (true)
        {
            Queue<Projectile> projList = new Queue<Projectile>();
            //Move to the next waypoint and create "knives" as you go
            bool waypointReached;
            float spawnTimer = attackInterval;
            do
            {
                //Move toward next thing waypoint
                waypointReached = knives_waypointer.MoveToNextWaypoint(rb, speed);  //temp code

                //Spawn knives as you go
                spawnTimer -= Time.deltaTime;

                if(spawnTimer <= 0)
                {
                    //spawn a knive and enque it
                    projList.Enqueue(SpawnStarKnive( true));
                    spawnTimer = attackInterval;
                }

                yield return null;
            } while (waypointReached == false);

            Debug.Log("waypoint reached");
            rb.velocity = Vector2.zero;

            //once reached wait for second
            yield return new WaitForSeconds(triggerDelay);

            //Trigger all of the knives to attack the player
            TriggerStarKnives();

            //Reset after all knives have been destroyed
            yield return new WaitForSeconds(resetDelay);
            //Predicate<List<GameObject>> allKnivesGone = allObjectsDestroyed;
            //yield return new WaitUntil(allKnivesGone(kniveList);
        }
    }
    //--------------------------------------------------------------------------------------

    [Header("TeleShoot Burst")]
    public Projectile teleShoot2_bulletPF;
    [Range(0, 10)] public int teleShoot2_bulletNum = 4;
    [Range(0, 1)] public float teleShoot2_refireTime = .15f;
    [Range(0, 1)] public float teleShoot2_shootLag = 1.5f;
    [Range(0, 1)] public float teleShoot2_teleportLag = .5f;
    [Range(1, 3)] public int teleShoot2_knivesTillTrigger = 3;

    /// <summary>
    /// DaBozz randomly teleports around the arena while firing bullets in a 4 round burst
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="bulletNum"></param>
    /// <param name="refireTime"></param>
    /// <param name="attackInterval"></param>
    /// <param name="tele2ShootDelay"></param>
    /// <returns></returns>
    protected IEnumerator TeleShoot_Burstfire(Projectile prefab, int bulletNum, float refireTime, float attackInterval, float tele2ShootDelay)
    {
        //this round lasts 50 percent longer
        BattleManager.instance.fightDuration *= 1.7f; 

        anim.SetBool("TeleShoot", true);
        Vector3 centerpoint = BattleBox.Instance.transform.position;
        //wait a little bit before 
        yield return new WaitForSeconds(.75f);
        int j = 0;
        while (true)
        {

            if (j % teleShoot2_knivesTillTrigger == 0)
            {
                TriggerStarKnives();
            }

            //wait while continually aiming toward the target
            targDir = (target.position - transform.position).normalized;
            float rotationZ = Mathf.Atan2(targDir.y, targDir.x) * Mathf.Rad2Deg;

            //fire multiple bullets in a row at a locked angle
            for (int i = 0; i < bulletNum; i++)
            {
                anim.SetTrigger("shoot");
                Instantiate(prefab, transform.position, Quaternion.Euler(0f, 0f, rotationZ));
                yield return new WaitForSeconds(refireTime);
            }

            //plant a star knive
            SpawnStarKnive(true);
            j++;

            yield return new WaitForSeconds(attackInterval);

            //Teleport to a random place in the Battle Arena
            transform.position = RandomTeleportDestination(centerpoint, teleShoot_teleRadius, teleShoot_minTeleRadius);
            SoundManager.instance.PlaySound("OH");

            //Delay between teleport and next attack
            yield return new WaitForSeconds(tele2ShootDelay);
        }
    }

    //--------------------------------------------------------------------------------------

    [Header("Checkmate Da")]
    public int teleShoot3_kniveNum = 20;
    [Range(0, 1)] public float teleShoot3_refireTime = .15f;
    [Range(0, 1)] public float teleShoot3_shootLag = .7f;
    [Range(0, 1)] public float teleShoot3_targetShootLag = .2f;
    [Range(1, 3)] public float teleShoot3_teleportRadiusMultiplier = 1;
    [Range(0, 4)] public float teleShoot3_dramaticPause = 1.5f;
    public bool teleShoot3_homingAttacks = false;

    /// <summary>
    /// DaBozz randomly teleports around the arena while firing bullets in a 4 round burst
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="bulletNum"></param>
    /// <param name="refireTime"></param>
    /// <param name="attackInterval"></param>
    /// <param name="tele2ShootDelay"></param>
    /// <returns></returns>
    protected IEnumerator CheckmateDa( int knivesNum,float shootLag, float targetSpeedIncrease)
    {
        anim.SetBool("TeleShoot", true);

        //Create Dramatic black screen that fades in
        DramaticScreen bg = Instantiate(GameManagment.Instance.dramaScreePF, new Vector2(3, -23), Quaternion.identity);
        bg.onLoadCommand = DramaticScreen_OnLoadCommands.FadeIn;
        bg.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
        bg.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -5;



        SoundManager.instance.PlaySound("anything");
        Vector3 centerpoint = BattleBox.Instance.transform.position;
        //This ensures that this attack will last as long as it needs to
        BattleManager.instance.fightDuration = 9999;

        //set the player to the center of the arena and then freeze theme
        PlayerControls player = GameObject.FindObjectOfType<PlayerControls>();
        player.frozen = true;
        player.transform.position = BattleBox.Instance.transform.position;

        //wait a little bit before begining the attack
        yield return new WaitForSeconds(.75f);

        float currentLag = shootLag;
        //Create a flurry of Star Knives all across the battle arena 
        for (int i = 0; i < knivesNum; i++)
        {
            //Randomly teleport around the screen 
            transform.position = RandomTeleportDestination(centerpoint, teleShoot_teleRadius * teleShoot3_teleportRadiusMultiplier, teleShoot_minTeleRadius * teleShoot3_teleportRadiusMultiplier);
           
         

            //Spawn Star Knive
            anim.SetTrigger("shoot");
            SoundManager.instance.PlaySound("OH");
            SpawnStarKnive(teleShoot3_homingAttacks);

            //increase speed
            yield return new WaitForSeconds(currentLag);
            currentLag =  Mathf.Lerp(shootLag,targetSpeedIncrease, i/(knivesNum * .75f));

        }

        //teleport back to idle position and pause for dramatic affect
        transform.position = idlePosition;
        SoundManager.instance.PlaySound("chuckle");
        yield return new WaitForSeconds(1);
        SoundManager.instance.PlaySound("byebye");
        yield return new WaitForSeconds(teleShoot3_dramaticPause);

        //give control back to the player and then trigger the knives
        player.frozen = false;
        yield return new WaitForSeconds(.35f);
        TriggerStarKnives();
        

        //let player dodge all the knives (or die)
        yield return new WaitForSeconds(4);
        //StartCoroutine(DramaticScreen.FadeOut());
        BattleManager.instance.endBattle();
    }

    //------------------------------------------------------------------------------------

    

    //==========================================================================================================

    /// <summary>
    /// Returns a random world position within a given radius
    /// </summary>
    /// <param name="centerpoint"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    private Vector3 RandomTeleportDestination(Vector3 centerpoint, float radius, float minRadius)
    {
        anim.ResetTrigger("shoot");
        //Teleport to a random place in the Battle Arena
        SoundManager.instance.PlaySound("teleport");
        Vector2 rand = UnityEngine.Random.insideUnitCircle * radius;

        //if the random vector is within the min radius
        if (rand.magnitude < minRadius)
            rand += new Vector2(minRadius, minRadius);

        Vector3 destination = centerpoint + new Vector3(rand.x, rand.y, 0);
        return destination;
    }

    private StarKnive SpawnStarKnive(bool isHoming)
    {
        StarKnive knive;
        if (isHoming == false)
        {
            //face the knive tworad the player 
            targDir = (target.position - transform.position).normalized;
            float rotationZ = Mathf.Atan2(targDir.y, targDir.x) * Mathf.Rad2Deg;
            knive = Instantiate(starKnivePF, transform.position, Quaternion.Euler(0f, 0f, rotationZ));
        }
        else
            knive = Instantiate(starKnivePF, transform.position, Quaternion.identity);
        
       //set fields and return
        knive.triggerMan = this;
        knive.homing = isHoming;
        return knive;
    }

    /// <summary>
    /// Event that triggers the knives to activate and go towards player
    /// </summary>
    public Action OnKniveTrigger;
    private void TriggerStarKnives()
    {
        if (OnKniveTrigger != null)
        {
            Debug.Log("knife trigger fired");

            float rand = UnityEngine.Random.value;
            //voice clips
            if (rand <= .30f)
                SoundManager.instance.PlaySound("jevilHurt");
            else if (rand >= .45f)
                SoundManager.instance.PlaySound("cackle");

            SoundManager.instance.PlaySound("kniveTrigger"); //play trigger sound
            OnKniveTrigger();
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        foreach(Vector3 pos in knives_waypointList)
        {
            Gizmos.DrawSphere(pos, .7f);
        }
    }

    //private bool allObjectsDestroyed(List<GameObject> objList) => 
}
