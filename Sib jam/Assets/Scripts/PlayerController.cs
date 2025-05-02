using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck; // Точка проверки касания земли
    [SerializeField] private LayerMask groundLayer; // Слой земли
    [SerializeField] private float groundCheckRadius = 0.2f; // Радиус проверки

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

            // Проверка прыжка только в Update
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryJump();
            }
        }
    }

    private void FixedUpdate()
    {
        // Физика проверки касания земли в FixedUpdate для стабильности
        if (!isClimbing)
        {
            bool isGrounded = IsGrounded();

            // Можно добавить визуализацию для отладки
            Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius,
                isGrounded ? Color.green : Color.red);
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    private void TryJump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        // Проверяем есть ли что-то под ногами
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}