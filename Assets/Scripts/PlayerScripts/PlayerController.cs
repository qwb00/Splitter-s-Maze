using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D bigRb;
    public Rigidbody2D smallRb;
    public Transform bigTransform;
    public Transform smallTransform;

    public float bigMoveSpeed = 5f;
    public float smallMoveSpeed = 5f;
    public float jumpForce = 7f;

    public bool isCombined = true;
    private bool isTossMode = false;

    private float tossAngle = 45f;
    private float tossPower = 10f;
    public float minAngle = 0f;
    public float maxAngle = 90f;
    public float minPower = 5f;
    public float maxPower = 20f;

    public GameObject dotPrefab;
    public int trajectoryPointsCount = 20;
    public float trajectoryStep = 0.1f;
    private GameObject[] trajectoryDots;

    private Vector2 initialVelocity;

    private bool isSmallInFlight = false;
    private bool isBigGrounded = false;
    private bool isSmallGrounded = false;

    void Start()
    {
        CombinePlayers();
        CreateTrajectoryDots();
        HideTrajectory();
        ResetCollider();
    }

    void ResetCollider()
    {
        BoxCollider2D bigCollider = bigRb.GetComponent<BoxCollider2D>();
        if (bigCollider != null)
        {
            bigCollider.offset = Vector2.zero;
            bigCollider.size = new Vector2(1, 1);

            Debug.Log($"Collider reset: Offset = {bigCollider.offset}, Size = {bigCollider.size}");
        }
        else
        {
            Debug.LogError("BoxCollider2D not found in the object");
        }
    }

    void CreateTrajectoryDots()
    {
        trajectoryDots = new GameObject[trajectoryPointsCount];
        for (int i = 0; i < trajectoryPointsCount; i++)
        {
            GameObject dot = Instantiate(dotPrefab, Vector3.zero, Quaternion.identity);
            dot.SetActive(false);
            trajectoryDots[i] = dot;
        }
    }

    void ShowTrajectory()
    {
        if (trajectoryDots == null) return;
        Vector2 startPos = bigTransform.position + Vector3.up * 0.5f;
        float radAngle = tossAngle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)) * tossPower;
        initialVelocity = dir;
        float gravity = Physics2D.gravity.y * smallRb.gravityScale;
        for (int i = 0; i < trajectoryPointsCount; i++)
        {
            float t = i * trajectoryStep;
            float x = dir.x * t;
            float y = dir.y * t + 0.5f * gravity * t * t;
            Vector3 pos = new Vector3(startPos.x + x, startPos.y + y, 0);
            trajectoryDots[i].transform.position = pos;
            trajectoryDots[i].SetActive(true);
        }
    }

    void HideTrajectory()
    {
        if (trajectoryDots == null) return;
        for (int i = 0; i < trajectoryDots.Length; i++)
        {
            trajectoryDots[i].SetActive(false);
        }
    }

    void Update()
    {
        if (!isTossMode)
        {
            if (isCombined)
            {
                HandleBigMovement();
                HandleCombinedJump();

                if (Input.GetKeyDown(KeyCode.R))
                {
                    SeparatePlayers();
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    EnterTossMode();
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                    ReloadLevel();
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ReturnToMainMenu();
                }
            }
            else
            {
                HandleBigMovement();

                if (!isSmallInFlight)
                {
                    HandleSmallMovement();
                }

                HandleIndividualJumps();

                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (Vector2.Distance(bigTransform.position, smallTransform.position) < 1f && !isSmallInFlight)
                    {
                        CombinePlayers();
                    }
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                    ReloadLevel();
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ReturnToMainMenu();
                }
            }
        }
        else
        {
            HandleBigMovementInTossMode();
            AdjustTossParameters();
            ShowTrajectory();

            if (Input.GetKeyDown(KeyCode.E))
            {
                PerformToss();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
            {
                ExitTossMode();
            }
        }
    }

    void ReloadLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void HandleBigMovement()
    {
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        Vector2 vel = bigRb.velocity;
        vel.x = moveX * bigMoveSpeed;
        bigRb.velocity = vel;
    }

    void HandleSmallMovement()
    {
        float moveX = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) moveX = 1f;

        Vector2 vel = smallRb.velocity;
        vel.x = moveX * smallMoveSpeed;
        smallRb.velocity = vel;
    }

    void HandleCombinedJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isBigGrounded)
        {
            bigRb.velocity = new Vector2(bigRb.velocity.x, jumpForce); ;
        }
    }

    void HandleIndividualJumps()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isBigGrounded)
        {
            bigRb.velocity = new Vector2(bigRb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isSmallGrounded && !isSmallInFlight)
        {
            smallRb.velocity = new Vector2(smallRb.velocity.x, jumpForce);
        }
    }

    void CombinePlayers()
    {
        isCombined = true;
        isTossMode = false;
        smallTransform.SetParent(bigTransform);
        smallTransform.localPosition = new Vector3(0, 0.5f, 0);
        smallRb.velocity = Vector2.zero;
        smallRb.isKinematic = true;
        smallRb.simulated = false;
        isSmallInFlight = false;
    }

    void SeparatePlayers()
    {
        isCombined = false;
        smallTransform.SetParent(null);
        smallRb.isKinematic = false;
        smallTransform.position = bigTransform.position;
        smallRb.simulated = true;
    }

    void EnterTossMode()
    {
        if (isCombined)
        {
            isTossMode = true;
            ShowTrajectory();
        }
    }

    void ExitTossMode()
    {
        isTossMode = false;
        HideTrajectory();
    }

    void HandleBigMovementInTossMode()
    {
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        Vector2 vel = bigRb.velocity;
        vel.x = moveX * bigMoveSpeed;
        bigRb.velocity = vel;
    }

    void AdjustTossParameters()
    {
        if (Input.GetKey(KeyCode.UpArrow)) tossAngle += 40f * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow)) tossAngle -= 40f * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow)) tossPower -= 10f * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) tossPower += 10f * Time.deltaTime;

        tossAngle = Mathf.Clamp(tossAngle, minAngle, maxAngle);
        tossPower = Mathf.Clamp(tossPower, minPower, maxPower);
    }

    void PerformToss()
    {
        float radAngle = tossAngle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)) * tossPower;
        initialVelocity = dir;

        ExitTossMode();
        smallTransform.SetParent(null);
        smallRb.isKinematic = false;
        smallRb.velocity = Vector2.zero;

        smallRb.velocity = initialVelocity;

        isSmallInFlight = true;
        isCombined = false;
        smallRb.simulated = true;
    }
    public void SetBigGrounded(bool grounded)
    {
        isBigGrounded = grounded;
    }

    public void SetSmallGrounded(bool grounded)
    {
        isSmallGrounded = grounded;
    }

    public void SetSmallInFlight(bool inFlight)
    {
        isSmallInFlight = inFlight;
    }
}