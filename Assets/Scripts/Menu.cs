using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";
    public string firstLevelName = "Level1";
    public string loadLevelMenu = "LoadLevelMenu";

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(firstLevelName);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        Debug.Log("Game Exited");
    }

    public void LoadLevelMenu()
    {
        SceneManager.LoadScene(loadLevelMenu);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
