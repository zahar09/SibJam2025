using UnityEngine;
using UnityEngine.Events;

public class Lever : Interactable
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private bool startActive = false;
    [SerializeField] private bool isToggleable = true;

    [Space(10)]
    [SerializeField] private AudioClip[] switchSounds; // ������ ��������� ������
    [SerializeField] private AudioSource audioSource;   // AudioSource ��� ���������������

    [Space(10)]
    [SerializeField] private UnityEvent onActivate;
    [SerializeField] private UnityEvent onDeactivate;

    private SpriteRenderer spriteRenderer;
    private bool isActive = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isActive = startActive;
        UpdateVisual();

        // ������������� AudioSource, ���� �� ��������
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (isToggleable)
        {
            isActive = !isActive; // ������������ ���������
        }
        else
        {
            isActive = true; // ����������� �������������
        }

        UpdateVisual();
        PlayRandomSound(); // ������������� ��������� ����

        if (isActive)
        {
            onActivate.Invoke(); // ����� ������� ��� ���������
        }
        else
        {
            onDeactivate.Invoke(); // ����� ������� ��� �����������
        }
    }

    private void UpdateVisual()
    {
        spriteRenderer.sprite = isActive ? activeSprite : inactiveSprite;
    }

    private void PlayRandomSound()
    {
        if (switchSounds.Length > 0 && audioSource != null)
        {
            AudioClip clip = switchSounds[Random.Range(0, switchSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("����� ������ �� ��������� ��� AudioSource �����������!");
        }
    }

    protected override void OnEnterRange()
    {
        base.OnEnterRange();
        Debug.Log($"������� E ����� {(isActive && isToggleable ? "���������" : "��������")} �����");
    }
}