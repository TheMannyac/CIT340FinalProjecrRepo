using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPellot : Projectile
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        //
        Debug.Log("fall lol");
    }
}
