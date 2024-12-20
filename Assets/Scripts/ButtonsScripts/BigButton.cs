using UnityEngine;

public class BigButton : MonoBehaviour
{
    public Door door;
    public PlayerController playerController;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BigPlayer") && playerController.isCombined)
        {
            door.OpenDoor();
            transform.localScale = new Vector3(originalScale.x, originalScale.y / 2, originalScale.z);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BigPlayer") && playerController.isCombined)
        {
            door.CloseDoor();
            transform.localScale = originalScale;
        }
    }
}
