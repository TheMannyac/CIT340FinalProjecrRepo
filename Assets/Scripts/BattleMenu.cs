using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
    public enum MenuDisplayState
    {
        Neutral, ItemMenu, ActMenu, RunMenu, Disabled 
    }

public class BattleMenu : MonoBehaviour
{

    //Singleton Pattern\\
    /* ================================================
     *   Create object if not already in scene:     false
     *  Remove scene dupes:                          true
     *  Global access                                 true
     *  Keep across game scene loads                  false
     * =========================================================
     */

    private static BattleMenu instance;

    /// <summary>
    /// THe current singleton of the battlemanager 
    /// </summary>
    public static BattleMenu Instance { get { return instance; } }
    public bool UsedItem = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    [SerializeField] private MenuDisplayState Displaying = MenuDisplayState.Neutral;
    public MenuDisplayState displaying
    {
        get { return Displaying; }
        protected set
        {
            if (Displaying == value) return;

            switch (value)
            {
                case MenuDisplayState.Neutral:

                    break;
            }
        }

    } 

    public List<Button> menuButtons = new List<Button>();
    //public Tree<int> menuTree; 

    SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UsedItem = false;

    }
    private void OnEnable()
    {
        displaying = MenuDisplayState.Neutral;
    }

    //called when the fight button is pressed
    public void Fight()
    {
       // Debug.Log("FIGHT Button pressed");
        displaying = MenuDisplayState.Disabled;
        BattleManager.instance.endPlayerTurn();
    }
    public void Swap()
    {
        //Debug.Log("ACT Button Pressed");

        displaying = MenuDisplayState.ActMenu;
    }
    public void Item()
    {
        Debug.Log("ITEM Button Pressed");
        if(UsedItem == false)
        {
            Stylo.Heal(30);
            SoundManager.instance.PlaySound("heal");
            UsedItem = true;
        } else
        {
            SoundManager.instance.PlaySound("squish");
            if(Random.value >= .75f)
            {
                SoundManager.instance.PlaySound("chuckle");
            }
        }
    }
    public void Run()
    {
        Debug.Log("RUN Button Pressed");
        GameManagment.goToMenu();
    }
}
