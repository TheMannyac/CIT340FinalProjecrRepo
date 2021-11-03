using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Stylo : MonoBehaviour
{
    public static Stylo instance;
    public static int currentLevel = 1;
    public static int EXP = 0;

    public Text levelText;
    public Slider expBar;

    [SerializeField] private static int nextLevelRequirement = 100;
    [SerializeField] private static int maxLevel = 10;


    // Start is called before the first frame update
    void Start()
    {
        //Ensure that there is only one player per scene
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            Debug.LogWarning(gameObject.name + "Destroyed");
            return;
        }

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    //TODO make the bar gradually increase
    private void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text = "Lvl: " + currentLevel;
        }
        if (expBar != null)
        {
            Debug.Log("HUD updated");
            //EXP bar displays the percentage toward next levelup
            expBar.value = EXP/nextLevelRequirement;
        }
    }
    
}
