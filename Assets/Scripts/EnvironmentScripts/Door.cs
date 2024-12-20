using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 openPosition;
    public Vector3 closedPosition;
    public float openSpeed = 2f;
    private BoxCollider2D doorCollider;
    private bool isOpen = false;

    private void Start()
    {
        closedPosition = transform.position;
        doorCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * openSpeed);
            doorCollider.enabled = false;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime * openSpeed);
            doorCollider.enabled = true;
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }
}
