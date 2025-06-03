using UnityEngine;

public class Door : Interactable
{
    [Header("Визуализация")]
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Transform teleportTarget;

    [Header("Состояние")]
    [SerializeField] private bool isOpen = false;

    [Header("Звуки")]
    [SerializeField] private AudioClip[] openSounds; // Массив звуков открытия
    [SerializeField] private AudioClip[] closeSounds; // Массив звуков закрытия
    [SerializeField] private AudioSource audioSource; // AudioSource для звуков

    private SpriteRenderer spriteRenderer;
    private GameObject player;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();

        // Инициализация AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        interactionText = "Открыть дверь";
    }

    public override void Interact()
    {
        base.Interact();

        if (isOpen && player != null && teleportTarget != null)
        {
            TeleportPlayer();
        }
        else if (!isOpen)
        {
            Open();
        }
    }

    public void Open()
    {
        if (isOpen) return;

        isOpen = true;
        UpdateVisual();
        interactionText = "Войти";

        PlayRandomSound(openSounds);
    }

    public void Close()
    {
        if (!isOpen) return;

        isOpen = false;
        UpdateVisual();
        interactionText = "Открыть дверь";

        PlayRandomSound(closeSounds);
    }

    private void TeleportPlayer()
    {
        player.transform.position = teleportTarget.position;
        Debug.Log("Игрок телепортирован!");
    }

    private void UpdateVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isOpen ? openSprite : closedSprite;
        }
    }

    private void PlayRandomSound(AudioClip[] clips)
    {
        if (clips.Length == 0 || audioSource == null)
        {
            Debug.LogWarning("Звуки не назначены или AudioSource отсутствует!");
            return;
        }

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }

    protected override void OnEnterRange()
    {
        base.OnEnterRange();
        player = GameObject.FindGameObjectWithTag("Player");

        if (isOpen)
        {
            interactionText = "Войти";
        }
        else
        {
            interactionText = "Открыть дверь";
        }
    }

    protected override void OnExitRange()
    {
        base.OnExitRange();
        player = null;
    }
}