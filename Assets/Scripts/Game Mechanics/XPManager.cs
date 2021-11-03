using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    

    //ublic Text levelText;
    //public Slider expBar;
    [SerializeField] private static int currentLevel = 1;
    [SerializeField] private static int maxLevel = 10;
    [SerializeField] private int EXP = 0;
    [SerializeField] private static int nextLevelRequirement = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gainExp(int xp)
    {
        Debug.Log("gained " + xp + "XP");
        EXP += xp;

        //Level up if we have enough EXP to do so
        if (EXP >= nextLevelRequirement)
            levelIp();

        //UpdateUI();
    }

    //TODO make this an event
    private void levelIp()
    {
        Debug.Log("LEVEL UP!");
        SoundManager.instance.PlaySound("levelUp");
        //Increase in level
        currentLevel = Mathf.Min(currentLevel + 1, maxLevel);

        //Makes sure that roll-over exp is retained
        EXP = EXP % nextLevelRequirement;

        //Increase next level requirement
        nextLevelRequirement *= (int)1.5f;

    }
}
