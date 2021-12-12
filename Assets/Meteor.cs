using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Projectile
{
    /// <summary>
    /// Prefab for the explosion gameObject that will be created when the meteor is destroyed
    /// </summary>
    public GameObject explosionPF;
    /// <summary>
    /// Torque force to add to tumble speed
    /// </summary>
    public float tumble = 2;
    /// <summary>
    /// Number of seconds until the meteor explodes
    /// </summary>
    [SerializeField] private float explodeTimer = 2.5f;
    /// <summary>
    /// Random explode timer offset
    /// </summary>
    [SerializeField] private float randomTmerOffset = 2;

    SpriteRenderer spr;

    protected override void Start()
    {
        base.Start();
        spr = GetComponent<SpriteRenderer>();
        explodeTimer += Random.Range(-randomTmerOffset,randomTmerOffset);

    }

    protected void Update()
    {
        explodeTimer -= Time.deltaTime;

        if(explodeTimer <= 0)
        {
            Explode();
        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Move()
    {
        rb.velocity = new Vector2(0, -speed);
    }

    private void Explode()
    {
        //Debug.Log("EXPLODE!");

        //Instansiate explosion obejct
        Instantiate(explosionPF, transform.position, Quaternion.identity);

        //Destroy game object
        Destroy(gameObject);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        switch (alignment)
        {
            case ProjectileAllignment.Enemy:
                if (collision.tag == "Player")
                {
                    Explode();
                }
                break;
        }
    }
}
