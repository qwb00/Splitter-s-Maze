using UnityEngine;

public class BigPlayerCollision : MonoBehaviour
{
    public PlayerController gameManager;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameManager.SetBigGrounded(true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameManager.SetBigGrounded(false);
        }
    }
}
