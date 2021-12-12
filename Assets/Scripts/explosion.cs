using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : Projectile
{
    //public float knockbackPower = 8;

    [Header("Scatter Burst")]
    public Projectile pellotPF;
    public int pellotNum = 8;

    SpriteRenderer spr;
    Collider2D hitbox;
    Animator animator;

    private void Awake()
    {
        hitbox = GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        SoundManager.instance.PlaySound("Small Explosion");
        Projectile.ScatterBurstAttack(pellotPF, transform.position, pellotNum,Vector2.up,358);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {  //If normalizedTime is 0 to 1 means animation is playing, if greater than 1 means finished
            //Debug.Log("not playing");

            //make invisible and 
            //spr.enabled = false;
            //hitbox.enabled = false;
            Destroy(gameObject);
        }
        else
        {
            //Debug.Log("playing");
        }
    }

    protected override void Move()
    {
        //base.Move();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
       //overrideen
    }
}
