using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeLevel : MonoBehaviour
{

    public string targetLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            SwitchLevel(false, targetLevel);
        }
    }

    static public void SwitchLevel(bool restartCurrent, string level = "")
    {
        if (restartCurrent)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else
        {
            //SceneManager.LoadScene(SceneManager.GetSceneAt(targetLevel));
        }
    }
    public void SaveStuff()
    {
        //PlayerPref.
    }
}
