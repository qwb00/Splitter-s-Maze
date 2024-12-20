using UnityEngine;

public class SmallButton : MonoBehaviour
{
    public Door door;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BigPlayer") || other.CompareTag("SmallPlayer"))
        {
            door.OpenDoor();
            transform.localScale = new Vector3(originalScale.x, originalScale.y / 2, originalScale.z);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BigPlayer") || other.CompareTag("SmallPlayer"))
        {
            door.CloseDoor();
            transform.localScale = originalScale;
        }
    }
}