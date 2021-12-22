
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagment: MonoBehaviour
{
    private static GameManagment _instance;
    public DramaticScreen dramaScreePF;
    public static GameManagment Instance { get; protected set; }

    private void Awake()
    {
        if (Instance == false)
        {
            Instance = this;
        } else
        {
            //later add code to MERge game management settings between scenes
            Destroy(gameObject);
            return;
        }

    DontDestroyOnLoad(gameObject);
    }

    //Returns to the main menu
    public static void goToMenu()
    {
        Debug.Log("You Win!");
       SceneManager.LoadScene("Main Menu");
    }

    public static void goToBattle()
    {
        Debug.Log("beginFight");
        SceneManager.LoadScene("BattleScene");
    }

   public static void GameOver()
    {
        Debug.Log("DED");
        SceneManager.LoadScene("Game Over");
    }

    public static void ExitGame()
    {
        Application.Quit();
    /*
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    application.Quit();
#endif */
    }

}
