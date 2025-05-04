using UnityEngine;

public class Door : Interactable
{
    [Header("������������")]
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Transform teleportTarget;

    [Header("���������")]
    [SerializeField] private bool isOpen = false;

    [Header("�����")]
    [SerializeField] private AudioClip[] openSounds; // ������ ������ ��������
    [SerializeField] private AudioClip[] closeSounds; // ������ ������ ��������
    [SerializeField] private AudioSource audioSource; // AudioSource ��� ������

    private SpriteRenderer spriteRenderer;
    private GameObject player;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();

        // ������������� AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        interactionText = "������� �����";
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
        interactionText = "�����";

        PlayRandomSound(openSounds);
    }

    public void Close()
    {
        if (!isOpen) return;

        isOpen = false;
        UpdateVisual();
        interactionText = "������� �����";

        PlayRandomSound(closeSounds);
    }

    private void TeleportPlayer()
    {
        player.transform.position = teleportTarget.position;
        Debug.Log("����� ��������������!");
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
            Debug.LogWarning("����� �� ��������� ��� AudioSource �����������!");
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
            interactionText = "�����";
        }
        else
        {
            interactionText = "������� �����";
        }
    }

    protected override void OnExitRange()
    {
        base.OnExitRange();
        player = null;
    }
}