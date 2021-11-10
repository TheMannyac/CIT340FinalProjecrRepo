
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagment: MonoBehaviour
{

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
