using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarKnive : Projectile
{
    /// <summary>
    /// The Denemy that triggers this projectile to attack
    /// </summary>
    public DaBozz triggerMan;
    public bool homing = true;
    public Color homingColor = new Color(183, 137, 134);
    public Color nonHomingColor = new Color(134, 165, 183);
    [SerializeField] private bool triggered = false;
    public Transform target;
    Vector2 targDir;

    SpriteRenderer spr;

    protected override void Start()
    {
        base.Start();
        triggered = false;
        try
        {
            triggerMan.OnKniveTrigger += OnKniveTrigger;
        } catch (NullReferenceException e)
        {
            Debug.LogError(e.StackTrace);
        }

        spr = GetComponent<SpriteRenderer>();
        target = Player_Battle.instance.transform;
    }

    protected override void FixedUpdate()
    {
        if (triggered)
        {
            //fly forward
            base.Move();

            if (homing)
                spr.color = Color.red;
            else
                spr.color = Color.blue;

        } else
        {
            if (homing)
            {
                spr.color = homingColor;
                //wait while continually aiming toward the target
                targDir = (target.position - transform.position).normalized;
                float rotationZ = Mathf.Atan2(targDir.y, targDir.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            }
            else
                spr.color = nonHomingColor;
            
        }
    }

    protected virtual void OnKniveTrigger()
    {
        Debug.Log("Trigger has been pulled");
        triggered = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        switch (alignment)
        {
            case ProjectileAllignment.Enemy:
                if(collision.tag == "Player")
                {
                    SoundManager.instance.PlaySound("thud1");
                    Destroy(gameObject);
                }
                break;
        }
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        triggerMan.OnKniveTrigger -= OnKniveTrigger;
    }
}
