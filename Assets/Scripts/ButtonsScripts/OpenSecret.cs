using UnityEngine;

public class SmallButtonNotCloseForSingleDoor : MonoBehaviour
{
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BigPlayer") || other.CompareTag("SmallPlayer"))
        {
            OpenSecretDoor();
        }
    }

    private void OpenSecretDoor()
    {
        GameObject secretDoor = GameObject.Find("SecretDoor(Clone)");

        if (secretDoor != null)
        {
            Door door = secretDoor.GetComponent<Door>();
            if (door != null)
            {
                door.OpenDoor();
            }
            else
            {
                Debug.LogError("Door not found");
            }
        }
        else
        {
            Debug.LogError("Door not found");
        }
    }
}
