using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioChannelSwitcher : MonoBehaviour
{
    [Header("Main Settings")]
    public AudioMixer audioMixer;
    public string[] mixerParameters; // Параметры громкости каналов (например "MusicVolume", "SFXVolume")

    private Dictionary<string, float> originalVolumes = new Dictionary<string, float>();
    private const float MIN_VOLUME = 0.0001f; // -80dB (практически тишина)

    void Start()
    {
        // Запоминаем исходные громкости всех каналов
        foreach (string parameter in mixerParameters)
        {
            if (audioMixer.GetFloat(parameter, out float currentVolume))
            {
                // Конвертируем из dB в линейное значение (0-1)
                float linearVolume = DBToLinear(currentVolume);
                originalVolumes[parameter] = linearVolume;
            }
            else
            {
                Debug.LogWarning($"Parameter {parameter} not found in AudioMixer!");
            }
        }
    }

    // Включить конкретный канал с исходной громкостью
    public void EnableChannel(string parameter, float duration = 1f)
    {
        if (!originalVolumes.ContainsKey(parameter))
        {
            Debug.LogError($"No original volume stored for {parameter}");
            return;
        }

        float targetVolume = originalVolumes[parameter];
        StartCoroutine(FadeChannel(parameter, targetVolume, duration));
    }

    // Выключить конкретный канал
    public void DisableChannel(string parameter, float duration = 1f)
    {
        //Debug.LogError($"Parameter {parameter} not configured in mixerParameters array");
        if (!ArrayContains(mixerParameters, parameter))
        {
            Debug.LogError($"Parameter {parameter} not configured in mixerParameters array");
            return;
        }

        StartCoroutine(FadeChannel(parameter, MIN_VOLUME, duration));
    }

    // Выключить все каналы
    public void DisableAllChannels(float duration = 1f)
    {
        foreach (string parameter in mixerParameters)
        {
            DisableChannel(parameter, duration);
        }
    }

    // Восстановить все исходные громкости
    public void RestoreAllChannels(float duration = 1f)
    {
        foreach (var pair in originalVolumes)
        {
            EnableChannel(pair.Key, duration);
        }
    }

    // Плавное изменение громкости канала
    private IEnumerator FadeChannel(string parameter, float targetLinearVolume, float duration)
    {
        float currentTime = 0f;
        float currentVol;

        // Получаем текущее значение громкости
        if (!audioMixer.GetFloat(parameter, out currentVol))
        {
            Debug.LogError($"Failed to get volume for parameter: {parameter}");
            yield break;
        }

        // Конвертируем в линейное значение
        float currentLinear = DBToLinear(currentVol);

        // Плавное изменение громкости
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime; // Используем unscaledDeltaTime
            float t = Mathf.Clamp01(currentTime / duration);
            float newLinear = Mathf.Lerp(currentLinear, targetLinearVolume, t);

            // Устанавливаем новое значение с защитой от ошибок
            if (!audioMixer.SetFloat(parameter, LinearToDB(newLinear)))
            {
                Debug.LogError($"Failed to set volume for parameter: {parameter}");
                yield break;
            }

            yield return null;
        }

        // Финализируем значение
        audioMixer.SetFloat(parameter, LinearToDB(targetLinearVolume));
    }

    // Проверка наличия элемента в массиве
    private bool ArrayContains(string[] array, string value)
    {
        foreach (string item in array)
        {
            if (item == value) return true;
        }
        return false;
    }

    // Конвертация между линейной шкалой и dB
    private float DBToLinear(float dB)
    {
        return Mathf.Pow(10, dB / 20f);
    }

    private float LinearToDB(float linear)
    {
        return linear <= MIN_VOLUME ? -80f : Mathf.Log10(linear) * 20f;
    }

    [ContextMenu("Test Disable Music")]
    private void TestDisableMusic()
    {
        DisableChannel("MusicVolume");
    }

    [ContextMenu("Test Enable Music")]
    private void TestEnableMusic()
    {
        EnableChannel("MusicVolume");
    }

    [ContextMenu("Test Disable All")]
    private void TestDisableAll()
    {
        DisableAllChannels();
    }

    [ContextMenu("Test Restore All")]
    private void TestRestoreAll()
    {
        RestoreAllChannels();
    }
}