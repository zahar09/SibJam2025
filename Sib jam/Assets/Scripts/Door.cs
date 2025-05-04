using UnityEngine;

public class Door : Interactable
{
    [Header("Визуализация")]
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Transform teleportTarget; // Позиция телепортации

    [Header("Состояние")]
    [SerializeField] private bool isOpen = false;

    private SpriteRenderer spriteRenderer;
    private GameObject player;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();

        // Убедимся, что триггер из Interactable работает
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
        isOpen = true;
        UpdateVisual();
        interactionText = "Войти";
    }

    public void Close()
    {
        isOpen = false;
        UpdateVisual();
        interactionText = "Открыть дверь";
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