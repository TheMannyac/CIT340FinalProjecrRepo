using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    private enum State
    {
        Neutral, ItemMenu, ActMenu, RunMenu, Disabled 
    }


    private State displaying;
    public List<Button> menuButtons = new List<Button>();
    //public Tree<int> menuTree; 

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        displaying = State.Neutral;
    }

    // Update is called once per frame
    void Update()
    {
        switch (displaying)
        {
            case State.Neutral:
                break;
            case State.ActMenu:
                break;
            case State.ItemMenu:
                break;
            case State.Disabled:
                break;
        }

        if (BattleManager.instance.playerTurn)
        {
            foreach(Button butt in menuButtons)
            {
                butt.interactable = true;
            }
        }
        else
        {
            foreach (Button butt in menuButtons)
            {
                butt.interactable = true;
            }
            displaying = State.Disabled;
        }
    }

    //called when the fight button is pressed
    public void Fight()
    {
        Debug.Log("FIGHT Button pressed");
        displaying = State.Disabled;
        BattleManager.instance.endTurn();
    }
    public void Act()
    {
        Debug.Log("ACT Button Pressed");
        displaying = State.ActMenu;
    }
    public void Item()
    {
        Debug.Log("ITEM Button Pressed");
        displaying = State.ItemMenu;
    }
    public void Run()
    {
        Debug.Log("RUN Button Pressed");
        displaying = State.RunMenu;
    }
   
    //TEMPORARY
    public void RefillLogos()
    {
        Debug.Log("Logos energy UP!!");
        ShardManager.current.GainEnergy(20,false);
    }

    public void RefillPathos()
    {
        Debug.Log("Pathos energy UP!!");
        ShardManager.current.GainEnergy(20, true);
    }

    public void HealingPotion()
    {
        Debug.Log("Health up!!");
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Health>().HealDamage(30);
    }
}
