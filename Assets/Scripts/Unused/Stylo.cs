using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class Stylo : MonoBehaviour
{
    /// <summary>
    /// The max amount of hitpoints that 
    /// </summary>
    public static int maxHP = 75;
    /// <summary>
    /// The current amount of health this character has
    /// </summary>
    public static int currentHP;

    public int _currentHp { get { return currentHP; } } 
    /// <summary>
    /// Current Level of this character
    /// </summary>
    public static int Level = 1;
    /// <summary>
    /// Current Amount of experience
    /// </summary>
    public static int currentXP = 0;

    
    [Header("Scene Specific Stuff")]
    [SerializeField] private Slider healthBar;

    //Singleton Pattern\\
    /* ================================================
     *  Create object if not already in scene:     false
     *  Remove scene dupes:                          true
     *  Global access                                 true
     *  Keep across game scene loads                  false
     * =========================================================
     */

    public static Stylo instance;

    /// <summary>
    /// THe current singleton instance of this component 
    /// </summary>
    public static Stylo Instance { get { return instance; } }


    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning(name + "'s" + GetType() + " was removed since "
                             + instance.gameObject.name + "'s already exists");

            Instance.healthBar = this.healthBar;
            //destroys the attached component NOT the game object
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        if(BattleManager.instance != null)
        {
            BattleManager.instance.OnBanterEnter += OnBanterEnter;
        }

        //Setup health
        currentHP = maxHP;
    }

    public static bool TakeDamage(int damage)
    {
        //plays a hitsound if the object was attacked
        SoundManager.instance.PlaySound("playerHit");

        //apply damage
        currentHP = Mathf.Max(0, currentHP - damage);
        Debug.Log("Stylo's health is now " + currentHP);

        //Update Health bar
        try
        {
            if (instance.healthBar != null)
            {
                Debug.Log("trying to access health bar");
                instance.healthBar.GetComponent<SliderScript>().setFillPercent((float)currentHP / maxHP);
            }
        } catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        //Check if we're dead and return the result
        if (currentHP <= 0)
        {
            SoundManager.instance.PlaySound("playerLastHit");
            Die();
            return true;
        }
        else
            return false;
    }

    public static bool Heal(int healing)
    {
        //play audio/visual effects

        //apply healing
        currentHP = Mathf.Min(currentHP + healing, maxHP);

        //Update Health bar
        try
        {
            if (instance.healthBar != null)
            {
                instance.healthBar.GetComponent<SliderScript>().setFillPercent((float)currentHP / maxHP);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        //return whether or not health has been maxed out
        return (currentHP >= maxHP);
    }


    /// <summary>
    /// Event that fires when this character dies
    /// </summary>
    public static Action OnDeath;
    protected static void Die()
    {
        Debug.Log("I'm dead!!");
        if( OnDeath != null)
        {
            OnDeath();
        }
        GameManagment.GameOver();
        currentHP = maxHP;
    }

    protected void OnBanterEnter()
    {
        healthBar.gameObject.SetActive(false);
    }
}
