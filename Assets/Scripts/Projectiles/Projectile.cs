using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileAllignment
{
    neutral, Ally, Enemy
}
public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    
   [SerializeField] protected ProjectileAllignment alignment = ProjectileAllignment.neutral;
   public virtual ProjectileAllignment Alignment
    {
        get { return alignment; }
        protected set => alignment = value;
    }

    protected Rigidbody2D rb;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //set tag to projectile
        gameObject.tag = "Projectile";

        BattleManager.instance.OnPlayerTurnEnter += OnPlayerTurnEnter;

        rb = GetComponent<Rigidbody2D>();
    }

    
    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name + " was hit");
        SoundManager.instance.PlaySound("thud2");
        Destroy(gameObject);
    }

    protected virtual void OnPlayerTurnEnter()
    {
        Destroy(gameObject);
    }

    protected virtual void Move()
    {
        //movement 
        if (rb != null)
        {
            rb.velocity = transform.up * speed;
        }
        else
        {
            transform.Translate(new Vector3(Time.fixedDeltaTime * speed, 0, 0));
        }
    }

    protected virtual void OnDisable()
    {
        //BattleManager.instance.OnPlayerTurnEnter -= OnPlayerTurnEnter;
    }

    protected virtual void OnDestroy()
    {
        if(BattleManager.instance != null)
        {
            BattleManager.instance.OnPlayerTurnEnter -= OnPlayerTurnEnter;
        }
    }

    //BULLET PATTERNS============================
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bullet"> prefab of bullet to instansiate </param>
    /// <param name="pellotsPerburst">Number of bullets int the burst </param>
    /// <param name="spreadAngle">the number of degrees that the attack will spread across </param>
    public static List<Projectile> ScatterBurstAttack(Projectile bullet, Vector3 position, int bulletNum, Vector3 aimDir, float spreadAngle)
    {
        //makes sure spread within 360 degrees
        spreadAngle = spreadAngle % 360;

        //assert: 0 should be where the game object is currently facing

        float sign = aimDir.y >= 0 ? 1 : -1;
        float offset = (sign >= 0) ? 0 : 360;
        float targAngle = Vector2.Angle(Vector2.right, aimDir) * sign + offset;
        Debug.Log(targAngle);

        List<Projectile> bulletsInBurst = new List<Projectile>();
        //the begining offset of the bullet
        float bulletAngleOffset = -((float)spreadAngle / 2);
        //angular distance between each bullet
        float angleIncrement = (float)spreadAngle / bulletNum;


        for (int i = 0; i < bulletNum; i++)
        {
            //SoundManager.instance.PlaySound("shot");
            //find the offset of the bullet based on the attack
            float ang = (targAngle + bulletAngleOffset);
            Quaternion bulletAngle = Quaternion.Euler(new Vector3(0, 0, ang));

            //Create bullet
            bulletsInBurst.Add(Instantiate(bullet, position, bulletAngle));
            //increment the bullet offset
            bulletAngleOffset += angleIncrement;
        }

        //returns a list of all the instansiated projectiles
        return bulletsInBurst;
    }
}
