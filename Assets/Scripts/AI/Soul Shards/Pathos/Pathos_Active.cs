using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pathos_Active_Attacks
{
    longShot
}
public class Pathos_Active : Pathos, ActiveShard
{
    SpriteRenderer spr;
    //Animator anim;

    protected ActiveShardManager _myManager;
    public ActiveShardManager MyManager {
        get => _myManager;
        set =>  _myManager = value;
    }

    public bool CanAttack => throw new System.NotImplementedException();

    private void Awake()
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

    //==================//PATHOS'S ACTIVE SHARD ATTACKS=============

    protected static Pathos_Active_Attacks Equipped;

    public Coroutine UseEquippedAttack()
    {
        Coroutine attackCoroutine;
        switch (Equipped)
        {
            case Pathos_Active_Attacks.longShot:
                attackCoroutine = StartCoroutine(LongShot());
                break;
            default:
                Debug.LogWarning("This shouldn't of happen");
                attackCoroutine = null;
                break;
        }

        return attackCoroutine;
    }

    public static void ChangeEquippedAttack(Pathos_Active_Attacks attack)
    {
        Equipped = attack;
    }

    public Color GetShardColor() { return shardColor; }

    //-----------------------------------------------------------------------

    public float attack1_interval = .5f;
    public float attack1_energyCost = 3;
    protected IEnumerator LongShot()
    {

        //do while makes sure at least 1 bullet is fired
        do
        {
            Instantiate(MyManager.Pathos_ProjPF,transform.position, MyManager.GetMouseDirection());

            yield return new WaitForSeconds(attack1_interval);
        } while (MyManager.FireButtonHeld);

        //ASSERT: the button has stopped being held
    }
}

