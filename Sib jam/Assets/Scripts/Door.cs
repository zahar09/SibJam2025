using UnityEngine;

public class Door : Interactable
{
    [Header("������������")]
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Transform teleportTarget; // ������� ������������

    [Header("���������")]
    [SerializeField] private bool isOpen = false;

    private SpriteRenderer spriteRenderer;
    private GameObject player;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();

        // ��������, ��� ������� �� Interactable ��������
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
        isOpen = true;
        UpdateVisual();
        interactionText = "�����";
    }

    public void Close()
    {
        isOpen = false;
        UpdateVisual();
        interactionText = "������� �����";
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