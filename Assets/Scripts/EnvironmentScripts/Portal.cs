using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string nextLevelName;
    public PlayerController playerController; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerController != null && playerController.isCombined)
        {
            LoadNextLevel();
        }
        else
        {
            Debug.Log("Cannot transition: Player is not in combined mode.");
        }
    }

    private void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.LogError("No next level name in Portal");
        }
    }
}
