using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadlyZone : MonoBehaviour
{
    [Header("Настройки затемнения")]
    [SerializeField] private Image fadePanel; // UI-панель для затемнения
    [SerializeField] private float fadeDuration = 1.5f; // Время затемнения
    [SerializeField] private Color fadeColor = Color.black; // Цвет затемнения

    [Header("Звуки смерти")]
    [SerializeField] private AudioClip[] deathSounds; // Массив звуков смерти
    [SerializeField] private AudioSource audioSource; // AudioSource для воспроизведения
    [SerializeField] private float deathSoundVolume = 0.7f; // Громкость звука

    private bool hasPlayerEntered = false;

    private void Awake()
    {
        // Инициализация AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, вошел ли игрок в зону
        if (other.CompareTag("Player") && !hasPlayerEntered)
        {
            hasPlayerEntered = true;
            StartCoroutine(HandleDeathSequence());
        }
    }

    private IEnumerator HandleDeathSequence()
    {
        // 1. Проигрываем случайный звук смерти
        PlayRandomDeathSound();

        // 2. Затемнение экрана
        yield return FadeScreen(fadePanel, fadeColor, fadeDuration);

        // 3. Перезагрузка текущей сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PlayRandomDeathSound()
    {
        if (deathSounds.Length > 0 && audioSource != null)
        {
            AudioClip clip = deathSounds[Random.Range(0, deathSounds.Length)];
            audioSource.PlayOneShot(clip, deathSoundVolume);
        }
        else
        {
            Debug.LogWarning("Звуки смерти не назначены или AudioSource отсутствует!");
        }
    }

    private IEnumerator FadeScreen(Image panel, Color targetColor, float duration)
    {
        float elapsed = 0f;
        Color startColor = panel.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            panel.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        panel.color = targetColor;
    }
}