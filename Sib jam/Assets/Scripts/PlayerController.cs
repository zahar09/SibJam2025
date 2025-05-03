using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    // Звуки ходьбы
    [Header("Звуки ходьбы")]
    [SerializeField] private AudioClip[] walkSounds;      // Массив звуков ходьбы
    [SerializeField] private AudioSource walkAudioSource; // Отдельный AudioSource для ходьбы
    [SerializeField] private float walkSoundInterval = 0.5f; // Интервал между звуками ходьбы

    // Звуки прыжка
    [Header("Звуки прыжка")]
    [SerializeField] private AudioClip[] jumpSounds;       // Массив звуков прыжка
    [SerializeField] private AudioSource jumpAudioSource; // Отдельный AudioSource для прыжка

    private Rigidbody2D rb;
    private bool isClimbing = false;
    private Ladder currentLadder;

    private float lastWalkSoundTime; // Время последнего звука ходьбы
    private bool wasGroundedLastFrame; // Был ли игрок на земле в предыдущем кадре

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Инициализация AudioSource для ходьбы
        if (walkAudioSource == null)
        {
            walkAudioSource = gameObject.AddComponent<AudioSource>();
            walkAudioSource.playOnAwake = false;
        }

        // Инициализация AudioSource для прыжка
        if (jumpAudioSource == null)
        {
            jumpAudioSource = gameObject.AddComponent<AudioSource>();
            jumpAudioSource.playOnAwake = false;
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
        bool isGrounded = IsGrounded();

        // Проверка ходьбы для звука
        if (!isClimbing && isGrounded && Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            if (Time.time - lastWalkSoundTime > walkSoundInterval)
            {
                PlayRandomWalkSound();
                lastWalkSoundTime = Time.time;
            }
        }

        wasGroundedLastFrame = isGrounded;

        // Можно добавить визуализацию для отладки
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius,
            isGrounded ? Color.green : Color.red);
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
            PlayRandomJumpSound();
            wasGroundedLastFrame = true;
        }
        else if (wasGroundedLastFrame)
        {
            // Буфер времени для прыжка после схода с платформы
            Invoke(nameof(PlayRandomJumpSound), 0.1f);
            wasGroundedLastFrame = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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

    private void PlayRandomWalkSound()
    {
        if (walkSounds.Length > 0 && walkAudioSource != null)
        {
            AudioClip clip = walkSounds[Random.Range(0, walkSounds.Length)];
            walkAudioSource.PlayOneShot(clip);
        }
    }

    private void PlayRandomJumpSound()
    {
        if (jumpSounds.Length > 0 && jumpAudioSource != null)
        {
            AudioClip clip = jumpSounds[Random.Range(0, jumpSounds.Length)];
            jumpAudioSource.PlayOneShot(clip);
        }
    }
}