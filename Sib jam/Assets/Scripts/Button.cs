using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Button : Interactable
{
    [SerializeField] private UnityEvent onActivate;
    [SerializeField] private UnityEvent onDeactivate;
    [SerializeField] private float delay = 1f;

    // Звуки активации
    [Header("Звуки активации")]
    [SerializeField] private AudioClip[] activateSounds;      // Массив звуков активации
    [SerializeField] private AudioSource activateAudioSource; // Отдельный AudioSource для активации
    //[SerializeField] private float activateSoundVolume = 0.7f; // Громкость звука активации

    private bool isPressed = false;
    private Coroutine toggleCoroutine = null;

    private void Awake()
    {
        // Инициализация AudioSource для активации
        if (activateAudioSource == null)
        {
            activateAudioSource = gameObject.AddComponent<AudioSource>();
            activateAudioSource.playOnAwake = false;
        }
    }

    public override void Interact()
    {
        base.Interact();

        // Останавливаем предыдущую корутину, если она есть
        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
            Debug.Log("Предыдущая корутина остановлена");
        }

        // Активируем кнопку
        isPressed = true;
        Debug.Log("Кнопка активирована");
        onActivate.Invoke();
        PlayRandomActivateSound(); // Воспроизводим случайный звук активации

        // Запускаем таймер для деактивации
        toggleCoroutine = StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        Debug.Log($"Ожидание {delay} секунд...");
        yield return new WaitForSeconds(delay);
        Debug.Log("Таймер сработал");

        // Деактивируем кнопку
        isPressed = false;
        Debug.Log("Кнопка деактивирована");
        onDeactivate.Invoke(); // Звук деактивации больше не воспроизводится
        toggleCoroutine = null;
    }

    private void PlayRandomActivateSound()
    {
        if (activateSounds.Length > 0 && activateAudioSource != null)
        {
            AudioClip clip = activateSounds[Random.Range(0, activateSounds.Length)];
            activateAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Звуки активации не назначены или AudioSource отсутствует!");
        }
    }

    protected override void OnEnterRange()
    {
        base.OnEnterRange();
        Debug.Log("Игрок вошёл в зону кнопки");
    }

    protected override void OnExitRange()
    {
        base.OnExitRange();
        Debug.Log("Игрок вышел из зоны кнопки");
    }
}