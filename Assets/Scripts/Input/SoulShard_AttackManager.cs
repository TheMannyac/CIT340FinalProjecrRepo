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
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int energyRegen = 2;
    [SerializeField] private float secondsBetweenEnergyRegen = 1;
    [SerializeField] private bool canRegenEnergy = true;
    public Slider energySlider;
    public int energyCost = 10;

    private int currentEnergy;
    
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
    MonoBehaviour parentScript; //determines which shard this is [CHANGE THIS LATER]
    MoveAndRotate moveScript;
    Animator anim;
    Rigidbody2D rb;

    private void Awake()
    {
        moveScript = GetComponent<MoveAndRotate>();

       
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set max energy
        currentEnergy = maxEnergy;

        if(attackType == State.melee)
        {
            //if the object is a melee type, then it follows the mouse
            moveScript.targetMouse = true;
            moveScript.useActivationRange = false;
            //moveScript.useFollowDistance = false;
        }

        if (energySlider == null)
        {
            Debug.LogWarning(gameObject.name + "does not have an energy slider attached");
        }

        //Set the energy bar to the current value
        energySlider.GetComponent<SliderScript>().setFillPercent((float)currentEnergy / maxEnergy);
        //begin passive energy regen
        StartCoroutine(RegenEnergy());
    }

    //called whenever game object is created or enabled
    private void OnEnable()
    {
        
    }

    //called whenever game object is destroyed or disabled
    private void OnDisable()
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
                    if (Input.GetAxis("Fire2") == 1) { Shoot(); }
                    break;
            }
        }
    }

    private IEnumerator RegenEnergy()
    {
       //Debug.Log("Begin Health Regen");
        while(true)
        {
            if(currentEnergy < maxEnergy && canFire)
            {
                //Debug.Log(gameObject.name + " regained " + energyRegen + " energy");
                GainEnergy(energyRegen);
            }
            yield return new WaitForSeconds(secondsBetweenEnergyRegen);
        }
    }

    void Shoot()
    {
        if (currentEnergy >= energyCost)
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

            //give other shard energy for dealing damage
            if (isPathos())
            {//[CHANGE THIS LATER]
               GetComponent<Pathos>().ShardDealsDamage();
            }
            else
            {
              GetComponent<Logos>().ShardDealsDamage();
            }

            //Update energy value 
            ConsumeEnergy(energyCost);
        }
    }

    void Melee()
    {
        if (currentEnergy >= energyCost)
        {
            //Play Attack Animation

            //SoundManager.instance.PlaySound("slash");

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

            //give other shard energy for dealing damage [CHANGE THIS LATER]
            if (isPathos())
            {
                GetComponent<Pathos>().ShardDealsDamage();
            }
            else
            {
                GetComponent<Logos>().ShardDealsDamage();
            }

            //Update slider
            ConsumeEnergy(energyCost);
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

    private bool isPathos()
    {
        if (attackType == State.melee)
            return false;
        else
            return true;
    }

    public void ConsumeEnergy(int energyCost)
    {
        currentEnergy = Mathf.Max(currentEnergy - energyCost, 0);
        //Debug.Log(gameObject.name + "'s energy is now " + currentEnergy);
        //Debug.Log(gameObject.name + "'s energy bar should be " + (float) currentEnergy/maxEnergy);

        energySlider.GetComponent<SliderScript>().setFillPercent((float) currentEnergy / maxEnergy);
    }

   public void GainEnergy(int energyRegained)
    {
        currentEnergy = Mathf.Min(currentEnergy + energyRegained, maxEnergy);

        energySlider.GetComponent<SliderScript>().setFillPercent((float)currentEnergy / maxEnergy);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
