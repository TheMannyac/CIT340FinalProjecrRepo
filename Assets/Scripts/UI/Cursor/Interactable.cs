using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool displayDialouge = false;
    public bool changeLevel = false;
    public string levelName;


    // Start is called before the first frame update
    void Start()
    {
            
    }

   public void OnClick()
    {
        Debug.Log("You have interacted with " + gameObject.name);

        if (displayDialouge && GetComponent<DialougeTrigger>() != null)
        {
            GetComponent<DialougeTrigger>().TriggerDialouge();
        }
    }

}
