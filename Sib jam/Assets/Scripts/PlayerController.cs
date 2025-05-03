using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    // ����� ������
    [Header("����� ������")]
    [SerializeField] private AudioClip[] walkSounds;
    [SerializeField] private AudioSource walkAudioSource;
    [SerializeField] private float walkSoundInterval = 0.5f;

    // ����� ������
    [Header("����� ������")]
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private AudioSource jumpAudioSource;

    // ��������
    [Header("��������")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private bool isClimbing = false;
    private Ladder currentLadder;

    private float lastWalkSoundTime;
    private bool wasGroundedLastFrame;

    // ���� ��� ��������
    private static readonly int IdleHash = Animator.StringToHash("idle");
    private static readonly int RunHash = Animator.StringToHash("run");
    private static readonly int JumpHash = Animator.StringToHash("jump");
    private static readonly int ClimbHash = Animator.StringToHash("climb");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
       // animator = GetComponent<Animator>();

        // ������������� AudioSource ��� ������
        if (walkAudioSource == null)
        {
            walkAudioSource = gameObject.AddComponent<AudioSource>();
            walkAudioSource.playOnAwake = false;
        }

        // ������������� AudioSource ��� ������
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
            currentLadder.Climb(); // �������������� ������ �� ��������
        }
        else
        {
            HandleMovement();

            // ������ ��� ������� Space
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryJump();
            }
        }
    }

    private void FixedUpdate()
    {
        bool isGrounded = IsGrounded();

        // ���������� ����������
        if (isClimbing)
        {
            animator.SetBool(ClimbHash, true);
            animator.SetBool(RunHash, false);
            animator.SetBool(IdleHash, false);
        }
        else
        {
            animator.SetBool(ClimbHash, false);

            // �������� ������ (������ ���� �� �� ����� � �� ������)
            if (rb.velocity.y > 0.1f)
            {
                animator.SetTrigger(JumpHash);
            }

            // �������� idle/run
            float speedX = Mathf.Abs(rb.velocity.x);
            animator.SetBool(RunHash, speedX > 0.1f);
            animator.SetBool(IdleHash, speedX <= 0.1f);
        }

        // ���� ������
        if (!isClimbing && isGrounded && Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            if (Time.time - lastWalkSoundTime > walkSoundInterval)
            {
                PlayRandomWalkSound();
                lastWalkSoundTime = Time.time;
            }
        }

        wasGroundedLastFrame = isGrounded;

        // ������� ��� �������� ������� �����
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius,
            isGrounded ? Color.green : Color.red);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        // ������� ���������
        if (horizontal != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontal), 1f, 1f);
        }
    }

    private void TryJump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger(JumpHash);
            PlayRandomJumpSound();
            wasGroundedLastFrame = true;

            // ����� �������� ������ ����� 0.5 �������
            Invoke(nameof(OnJumpAnimationEnd), 0.5f);
        }
        else if (wasGroundedLastFrame)
        {
            Invoke(nameof(PlayRandomJumpSound), 0.1f);
            wasGroundedLastFrame = false;
        }
    }

    private void OnJumpAnimationEnd()
    {
        animator.ResetTrigger(JumpHash);
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