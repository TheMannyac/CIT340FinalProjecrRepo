using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    //[SerializeField] private float progress = 1;
    float targetFillPercent = 1;
    float fillPercentChange;
    float healthBarChangePerFrame = .01f;
    Coroutine updateCoroutine;
    //[SerializeField] private float fillTime = 1;
    //[SerializeField] private float fillRate = .1f;
    //[SerializeField] private float fillThreashhold = .01f;
    public Slider mySlider;

    //TEMP CODE
    public static Slider logosEnergyBar;
    public static Slider pathosEnergyBar;

    private void Start()
    {
        //TEMP CODE
        if (name == "Pathos Energy Bar")
        {
            pathosEnergyBar = mySlider;
        }
        else if (name == "Logos Energy Bar")
        {
            logosEnergyBar = mySlider;
        }
    }


    public void setFillPercent(float fillPercent)
    {
        targetFillPercent = fillPercent;
       //Debug.Log("Bar value is now " + targetFillPercent);
       mySlider.value = targetFillPercent;
        /*
        if (updateCoroutine != null)
            StopCoroutine(updateCoroutine);
        updateCoroutine = StartCoroutine(UpdateFillPercent());
        */
    }

    
    private IEnumerator UpdateFillPercent()
    {
        //difference between the previous and new fill percent 
        fillPercentChange = targetFillPercent - mySlider.value;
        while (mySlider.value != targetFillPercent)
        {
            if (Mathf.Abs(mySlider.value - targetFillPercent) < healthBarChangePerFrame)
                mySlider.value = targetFillPercent;
            else
            {
                mySlider.value += fillPercentChange * healthBarChangePerFrame;
                yield return new WaitForEndOfFrame();
            }
        }

        /*
        float startFill = slider.value; //fill percent before change
        float elapsedTime = 0;
        while (Mathf.Abs(progress - slider.value) < fillThreashhold)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / fillTime;
            slider.value = Mathf.Lerp(startFill,progress, percentageComplete);
            yield return new WaitForEndOfFrame();
        }
        */
    }
    
}

