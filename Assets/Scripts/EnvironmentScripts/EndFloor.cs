using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFloor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BigPlayer") || other.CompareTag("SmallPlayer"))
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
