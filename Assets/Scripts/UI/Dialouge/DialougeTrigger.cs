using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialougeTrigger : MonoBehaviour
{
    public Dialouge dialouge;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TriggerDialouge()
    {
        FindObjectOfType<DialougeManager>().StartDialouge(dialouge);
    }
}
