using UnityEngine;
using UnityEngine.Events;

public class Lever : Interactable
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private bool startActive = false;
    [SerializeField] private bool isToggleable = true;

    [Space(10)]
    [SerializeField] private AudioClip[] switchSounds; // Массив случайных звуков
    [SerializeField] private AudioSource audioSource;   // AudioSource для воспроизведения

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

        // Инициализация AudioSource, если не назначен
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
            isActive = !isActive; // Переключение состояния
        }
        else
        {
            isActive = true; // Однократное активирование
        }

        UpdateVisual();
        PlayRandomSound(); // Воспроизвести случайный звук

        if (isActive)
        {
            onActivate.Invoke(); // Вызов событий при активации
        }
        else
        {
            onDeactivate.Invoke(); // Вызов событий при деактивации
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
            Debug.LogWarning("Звуки рычага не назначены или AudioSource отсутствует!");
        }
    }

    protected override void OnEnterRange()
    {
        base.OnEnterRange();
        Debug.Log($"Нажмите E чтобы {(isActive && isToggleable ? "выключить" : "включить")} рычаг");
    }
}