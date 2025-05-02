using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isClimbing = false;
    private Ladder currentLadder;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetClimbing(bool climbing, Ladder ladder)
    {
        isClimbing = climbing;
        currentLadder = ladder;

        if (climbing)
        {
            rb.gravityScale = 0;
            currentLadder.SetPlayerRigidbody(rb);
        }
        else
        {
            rb.gravityScale = 1;
            rb.velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (isClimbing)
        {
            currentLadder.Climb();
        }
        else
        {
            HandleMovement();
            HandleJump();
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        // Реализуйте проверку касания земли через Raycast или Collider
        return true; // Временная заглушка
    }
}