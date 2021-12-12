using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHP = 100;
    public bool hasInvincibilityFrames = false;
    public float invincibilityDuration = 1.5f; 
    public Slider _slider;

    private int currentHP;

    private void Awake()
    {
        //collider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        if (_slider != null)
        {
            _slider.GetComponent<SliderScript>().setFillPercent(currentHP/maxHP);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*returns whether or not th object dies
     * 
     */
    public bool takeDamage(int changeInHP)
    {
        //plays a hitsound if the object was attacked



        //apply change to current HP
        currentHP = Mathf.Max(0, currentHP - changeInHP);

        if (_slider != null)
        {
            _slider.GetComponent<SliderScript>().setFillPercent((float)currentHP / maxHP);
        }

        //Object dies and returns true if it goes below 0
        if (currentHP <= 0)
        {
            Die();
            return true;
        } else
        {
            //hitInvincibilty();

            //otherwise returns false
            return false;
        }
    }

    public bool HealDamage(int changeInHP)
    {
        //plays a hitsound if the object was attacked


        //apply change to current HP
        currentHP = Mathf.Min(maxHP, currentHP + changeInHP);

        //update healthbar
        if (_slider != null)
        {
            _slider.GetComponent<SliderScript>().setFillPercent((float)currentHP / maxHP);
        }

        //returns true if the health is maxed out
        if(currentHP == maxHP)
        {
            return true;
        } else
        {
            return false;
        }
       
    }

    private IEnumerator hitInvincibilty()
    {
        float elapsedTime = 0;
        //disable colliders [THIS IS A HOT FIX, SHOULD FIX LATER TO AVOID STUFF CLIPPING OUT OF BOUNDS] 
        //collider.enabled = false;
        Debug.LogWarning(name + "'s colliders have been disabled. You might wanna change this, dumbass");

        while (elapsedTime < invincibilityDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log(name + "'s invincibility has ended");
        //re-enable colliders
        //collider.enabled = true;
    }


    public void Die()
    {
        switch (gameObject.tag)
        {
            case "Player":
                //return to main menu if the player dies
                Debug.Log("the player has died");
                //GameManagment.goToLoseScreen();
                break;
              
        }
        Destroy(gameObject);
    }
}
