using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{

    public Text timerText;
    public float timeValue = 90; //amount of time, in seconds, to count down from
    public bool holdLastSecondAtOne = true; // when displaying the last second, makes sure that the value displays 1 instead of 0 in between seconds

    float baseTimeValue;

    // Start is called before the first frame update
    void Start()
    {
        //records the timer's base value
        baseTimeValue = timeValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        } else
        {
            //makes sure timer does not go below 0;
            timeValue = 0;
            Debug.Log("time has run out");
            GameManagment.goToLoseScreen();
            //Invoke("GameManagement.goToMenu", .5f);

        }

        displayTime(timeValue);
    }

    void displayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (holdLastSecondAtOne && timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        //Convert to minutes
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        //Convert remaining seconds that don't make up whole minute
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        //Change text color to red when Timer hits 10 seconds
        if (timeToDisplay < 10)
        {
            timerText.color = Color.red;
        } else
        {
            timerText.color = Color.black;
        }
    }

}

