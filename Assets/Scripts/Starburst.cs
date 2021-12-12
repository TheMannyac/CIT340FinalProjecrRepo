using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starburst : Projectile
{
    int rotSpeed = 600;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector2.right * Time.deltaTime * rotSpeed);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        switch (alignment)
        {
            case ProjectileAllignment.Enemy:
                if(collision.tag == "Player")
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SoundManager.instance.PlaySound("thud1");
    }
}
