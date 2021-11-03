using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHP = 100;
    public GameObject dropObject = null;   //prefab that wil be spawned on death
    public float dropRate = .1f;   //percent chance from 0 to 1 that item will drop
    public Slider _slider;

    [SerializeField] private int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        if (_slider != null)
        {
            _slider.GetComponent<SliderScript>().UpdateValue(currentHP/maxHP);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*returns whether or not th object dies
     * 
     */
    public bool changeHP(int changeInHP)
    {
        //plays a hitsound if the object was attacked
        if (changeInHP >= 0)
        {
           // playHitSound();
        }

        //apply change to current HP
        currentHP = Mathf.Min(maxHP, currentHP - changeInHP);

        if (_slider != null)
        {
            _slider.GetComponent<SliderScript>().UpdateValue(currentHP/maxHP);
        } 

        //Object dies and returns true if it goes below 0
        if (currentHP <= 0)
        {
            Die();
            return true;
        }

        //otherwise returns false
        return false;
    }

    void playHitSound()
    {
        switch (gameObject.tag)
        {
            case "Player":
                SoundManager.instance.PlaySound("Player_hit");
                break;
            case "Enemy":
                SoundManager.instance.PlaySound("Player_hit");
                break; 
        }
    }

    public void Die()
    {
        switch (gameObject.tag)
        {
            case "Player":
                //return to main menu if the player dies
                Debug.Log("the player has died");
                GameManagment.goToLoseScreen();
                //Invoke("GameManagment.goToMenu", .5f);
                break;
            case "Enemy":
                //randomly drop a powerup when an enemy is killed
                if(dropObject != null)
        {
                    //checks item drop probability
                    if (Random.value <= dropRate)
                    {
                        Instantiate(dropObject, transform.position, transform.rotation);
                    }
                }
                break;
    }
        //item drop on death
        
        Destroy(gameObject);
    }
}
