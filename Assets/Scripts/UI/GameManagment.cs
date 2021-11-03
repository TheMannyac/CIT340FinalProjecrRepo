
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagment: MonoBehaviour
{
    

   //public Scene nextScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Returns to the main menu
    public static void goToMenu()
    {
        Debug.Log("You Win!");
       SceneManager.LoadScene("Main Menu");
    }

    //Goes to Lose Screen
    public static void goToLoseScreen()
    {
        Debug.Log("You Lost!");
        SceneManager.LoadScene("Lose Screen");
    }
}
