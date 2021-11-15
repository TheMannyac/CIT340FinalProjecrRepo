using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//TODO split this into its own thing
public class SoulShard_AttackManager : MonoBehaviour
{

    //State Machine
    private enum State
    {
        shooting, melee
    }

    //General Fields
    [SerializeField] private State attackType;   //Shard's current attacking method
    public LayerMask enemyLayers;   //Types of objects that can be hit
    public int energyCost = 10;

    
    //public Input input = Input.g 

    //Melee Fields
    public Transform attackPoint;
    public float attackRange = .5f;
    public int attackDmg = 6;
    public float knockbackPower = 5;

    //Shooting Fields
    public GameObject bulletPrefab;
    public float currentFireRate = 5;
    int dmgMultiplier = 0;
    Vector3 targetPosition;
    bool canFire = true;


    //Misc
    MonoBehaviour parentScript; //determines which shard this is [CHANGE THIS LATE
    Animator anim;
    Rigidbody2D rb;

    private void Awake()
    {

       
    }

    // Start is called before the first frame update
    void Start()
    {


    }


    // Update is called once per frame
    void Update()
    {
        if (canFire)
        {
            switch (attackType)
            {
                case State.melee:
                    //Look for attack input
                    if (Input.GetAxis("Fire1") == 1) { Melee(); }
                    break;
                case State.shooting:
                    //Scan for Attack input
                    if (Input.GetAxis("Fire1") == 1) { Shoot(); }
                    break;
            }
        }
    }


    void Shoot()
    {
        if (ShardManager.current.GetCurrentEnergy_Pathos() >= energyCost)
        {
            //Play Firing Sound Effect
            SoundManager.instance.PlaySound("firecracker");

            //Find Mouse world position
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mouseWorldPosition.z = 0;   //mouse z is naturally same as camera
            targetPosition = mouseWorldPosition;


            //Get the direction of the mouse
            Quaternion rot = Quaternion.FromToRotation(Vector3.right, targetPosition - transform.position);
            //not optimal way to access child obj
            GameObject laser = Instantiate(bulletPrefab, transform.position, rot);
            laser.GetComponent<laser>().damage *= dmgMultiplier;
            canFire = false;
            Invoke("Attackooldown", 1 / currentFireRate);

            
            ShardManager.current.LoseEnergy(energyCost, true);
        }
    }

    void Melee()
    {
        if (ShardManager.current.GetCurrentEnergy_Logos() >= energyCost)
        {
            //Play Attack Animation

            SoundManager.instance.PlaySound("slash");

            //Detect enemies within range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            //Apply damage to enemies

            foreach (Collider2D enemy in hitEnemies)
            {

                //Debug.Log("We hit " + enemy.name);
                //Apply damage and effects
                bool killedTarget = enemy.GetComponent<Health>().takeDamage(attackDmg);

                if (killedTarget)
                {
                    //Stylo.instance.gainExp(50);
                }

                /*
                //Deal knockback
                Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();
                enemyRB.isKinematic = false;
                Vector2 dir = ((enemy.transform.position - this.transform.position).normalized) * knockbackPower;
                enemyRB.AddForce(dir, ForceMode2D.Impulse);
                */
            }

            canFire = false;
            Invoke("Attackooldown", 1 / currentFireRate);


            //Update slider
            ShardManager.current.LoseEnergy(energyCost,false);
        }
    }
    
    /*
    public IEnumerator Knockback(float knockbackDuration, Transform obj)
    {
        float timer = 0;
        Rigidbody2D objRB = obj.GetComponent<Rigidbody2D>();

        while (knockbackDuration > timer)
        {
            Debug.Log("Knockback applied");
            timer += Time.deltaTime;
            Vector2 dir = (obj.transform.position - this.transform.position).normalized;
            objRB.AddForce(-dir * knockbackPower);
        }
        yield return 0;
    }
    */

    void Attackooldown()
    {
        canFire = true;
    }

    public void SwapAttack()
    {
        if(attackType == State.melee)
        {
            attackType = State.shooting;
        }
        else
        {
            attackType = State.melee;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
