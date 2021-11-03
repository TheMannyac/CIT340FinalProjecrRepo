using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public string nextSceneName;  //the scene that will be loaded if the play() function is called

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        if (nextSceneName != null)
        {
            SceneManager.LoadScene(nextSceneName);
        } else
        {
            Debug.Log("A scene was not assigned to button: " + gameObject.name);
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    application.Quit();
#endif
    }
}
