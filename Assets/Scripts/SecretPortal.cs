using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportOnTouch : MonoBehaviour
{
    public string targetSceneName = "ALT";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("No scene name");
        }
    }
}