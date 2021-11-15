using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private float progress = 1;
    [SerializeField] private float fillTime = 1;
    [SerializeField] private float fillRate = .1f;
    [SerializeField] private float fillThreashhold = .01f;
    public Slider slider;

    private void Start()
    {
       
    }


    public void setFillPercent(float fillPercent)
    {
        progress = fillPercent;
        //Debug.Log("Bar value is now " + progress);
        slider.value = progress;
        StartCoroutine(GradualFill());
    }

    
    private IEnumerator GradualFill()
    {

        float startFill = slider.value; //fill percent before change
        float elapsedTime = 0;
        while (Mathf.Abs(progress - slider.value) < fillThreashhold)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / fillTime;
            slider.value = Mathf.Lerp(startFill,progress, percentageComplete);
            yield return new WaitForEndOfFrame();
        }
    }
    
}

