using UnityEngine;

public class SmallPlayerCollision : MonoBehaviour
{
    public PlayerController gameManager;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameManager.SetSmallGrounded(true);
            gameManager.SetSmallInFlight(false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameManager.SetSmallGrounded(false);
        }
    }
}
