using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Logos_Active_Attacks
{
    SingleShot
}
public class Logos_Active : Logos, ActiveShard
{
    SpriteRenderer spr;

    protected ActiveShardManager _myManager;
    public ActiveShardManager MyManager
    {
        get => _myManager;
        set => _myManager = value;
    }

    protected bool _canAttack = true;
    public bool CanAttack => _canAttack;

    protected void Awake()
    {
        //make sure we have a reference to our active shard manager script
        if (MyManager == null)
        {
            MyManager = GetComponent<ActiveShardManager>();
        }
    }

    protected override void Start()
    {
        base.Start();
        spr = GetComponent<SpriteRenderer>();
        spr.color = shardColor;
    }

    //==================//LOGOS'S ACTIVE SHARD ATTACKS=============

    protected static Logos_Active_Attacks Equipped;

    public Coroutine UseEquippedAttack()
    {
        Coroutine attackCoroutine;
        switch (Equipped)
        {
            case Logos_Active_Attacks.SingleShot:
                attackCoroutine = StartCoroutine(SingleShot());
                break;
            default:
                Debug.LogWarning("This shouldn't of happen");
                attackCoroutine = null;
                break;
        }

        return attackCoroutine;
    }
    
    public static void ChangeEquippedAttack(Logos_Active_Attacks attack)
    {
        Equipped = attack;
    }

    public Color GetShardColor() { return shardColor; }

    //---------------------------------------------------------------

    public float attack1_interval = .1f;
    public float attack1_energyCost = 8;
    protected IEnumerator SingleShot()
    {
        Instantiate(MyManager.Logos_ProjPF, transform.position, MyManager.GetMouseDirection());
        yield return new WaitForSeconds(attack1_interval);

        //do not allow a refire until the button is pressed again
        while (MyManager.FireButtonHeld)
        {
            Debug.Log("you can't shoot again until you let go");
            yield return new WaitForSeconds(.1f);
        }
    }

}


