using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject world1;
    [SerializeField] private GameObject world2;
    [SerializeField] private Image fadePanel; // UI-затемнение
    [SerializeField] private float fadeDuration = 0.5f; // Время анимации

    [Header("Звуки переключения")]
    [SerializeField] private AudioClip[] switchSounds;
    [SerializeField] private AudioSource switchAudioSource;

    [Header("Музыка миров")]
    [SerializeField] private AudioClip musicWorld1;
    [SerializeField] private AudioClip musicWorld2;
    [SerializeField] private AudioSource musicAudioSource1;
    [SerializeField] private AudioSource musicAudioSource2;

    [SerializeField] private float musicFadeDuration = 0.5f; // Плавность смены музыки

    private bool isWorld1Active = true;

    private void Awake()
    {
        // Инициализация AudioSource для звука переключения
        if (switchAudioSource == null)
        {
            switchAudioSource = gameObject.AddComponent<AudioSource>();
            switchAudioSource.playOnAwake = false;
        }

        // Инициализация AudioSource для музыки
        if (musicAudioSource1 == null)
        {
            musicAudioSource1 = gameObject.AddComponent<AudioSource>();
            musicAudioSource1.playOnAwake = false;
            musicAudioSource1.loop = true;
        }

        if (musicAudioSource2 == null)
        {
            musicAudioSource2 = gameObject.AddComponent<AudioSource>();
            musicAudioSource2.playOnAwake = false;
            musicAudioSource2.loop = true;
        }
    }

    private void Start()
    {
        if (world1 == null || world2 == null || fadePanel == null)
        {
            Debug.LogError("Не все объекты назначены в инспекторе!");
            return;
        }

        world2.SetActive(false);

        // Включаем музыку текущего мира
        if (musicWorld1 != null)
        {
            musicAudioSource1.clip = musicWorld1;
            musicAudioSource1.volume = 1f;
            musicAudioSource1.Play();
        }

        if (musicWorld2 != null)
        {
            musicAudioSource2.clip = musicWorld2;
            musicAudioSource2.volume = 0f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(SwitchWorldWithFade());
        }
    }

    private IEnumerator SwitchWorldWithFade()
    {
        // 1. Воспроизвести звук переключения
        PlayRandomSwitchSound();

        // 2. Начать фейд-аут текущей музыки
        yield return FadeOutMusic();

        // 3. Затемнить экран
        yield return FadeScreen(1f);

        world1.SetActive(!isWorld1Active);
        world2.SetActive(isWorld1Active);

        // 4. Включить новую музыку с фейд-ин
        yield return FadeInMusic();

        // 4. Показать экран
        yield return FadeScreen(0f);

        // 6. Переключить миры
        isWorld1Active = !isWorld1Active;
    }

    private IEnumerator FadeScreen(float targetAlpha)
    {
        float startAlpha = fadePanel.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadePanel.color = new Color(0, 0, 0, targetAlpha);
    }

    private IEnumerator FadeOutMusic()
    {
        print(isWorld1Active);
        // Определяем, какой AudioSource сейчас активен
        AudioSource currentMusic = isWorld1Active ? musicAudioSource1 : musicAudioSource2;

        float elapsed = 0f;
        float startVolume = currentMusic.volume;

        while (elapsed < musicFadeDuration)
        {
            elapsed += Time.deltaTime;
            float volume = Mathf.Lerp(startVolume, 0f, elapsed / musicFadeDuration);
            currentMusic.volume = volume;
            yield return null;
        }

        currentMusic.Stop(); // Явно останавливаем старую музыку
        currentMusic.volume = 1f; // Сбрасываем громкость
    }

    private IEnumerator FadeInMusic()
    {
        print(isWorld1Active);
        // Определяем, какую музыку включать
        AudioSource targetMusic = isWorld1Active ? musicAudioSource2 : musicAudioSource1;
        AudioClip newClip = isWorld1Active ? musicWorld2 : musicWorld1;

        // Проверяем, что клип назначен
        if (newClip == null)
        {
            Debug.LogWarning($"Музыка для {(isWorld1Active ? "мира 2" : "мира 1")} не назначена!");
            yield break;
        }

        // Если клип изменился — обновляем
        if (targetMusic.clip != newClip)
        {
            targetMusic.clip = newClip;
        }

        // Явно останавливаем все музыки перед включением новой
        StopAllMusic();

        // Всегда сбрасываем громкость перед воспроизведением
        targetMusic.volume = 0f;
        targetMusic.Play(); // Включаем новую музыку

        float elapsed = 0f;
        while (elapsed < musicFadeDuration)
        {
            elapsed += Time.deltaTime;
            float volume = Mathf.Lerp(0f, 1f, elapsed / musicFadeDuration);
            targetMusic.volume = volume;
            yield return null;
        }

        targetMusic.volume = 1f;
    }

    // Явно останавливаем все музыки
    private void StopAllMusic()
    {
        if (musicAudioSource1 != null && musicAudioSource1.isPlaying)
        {
            musicAudioSource1.Stop();
            musicAudioSource1.volume = 1f;
        }

        if (musicAudioSource2 != null && musicAudioSource2.isPlaying)
        {
            musicAudioSource2.Stop();
            musicAudioSource2.volume = 1f;
        }
    }

    private void PlayRandomSwitchSound()
    {
        if (switchSounds.Length > 0 && switchAudioSource != null)
        {
            AudioClip clip = switchSounds[Random.Range(0, switchSounds.Length)];
            switchAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Звуки переключения не назначены или AudioSource отсутствует!");
        }
    }
}