using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private float progress = 1;
    [SerializeField] private float fillRate = .01f;
    public Slider slider;

    private void Start()
    {
       
    }

    public void setFillPercent(float fillPercent)
    {
        progress = fillPercent;
        //Debug.Log("Bar value is now " + progress);
        StartCoroutine(GradualChange());
    }

    private IEnumerator GradualChange()
    {
        while (progress != slider.value)
        {
            slider.value += fillRate * Mathf.Sign(progress - slider.value);
            yield return new WaitForFixedUpdate();
        }
    }

   // private IEnumerator 

}
