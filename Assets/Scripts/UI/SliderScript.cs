using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{

    [SerializeField] private float progress = 1;
    public Slider _slider;
   
    public void UpdateValue(float newValue)
    {
        Debug.Log("value uppdated");
        progress = newValue;
        _slider.value = progress;
        Debug.Log(_slider.value);
    }

}
