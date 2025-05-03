using UnityEngine;

public class ObjectActivationSound : MonoBehaviour
{
    [Header("Звуки появления")]
    [SerializeField] private AudioClip[] appearSounds;
    [SerializeField] private AudioSource appearAudioSource;
    //[SerializeField] private float appearVolume = 0.7f;

    [Header("Звуки исчезновения")]
    [SerializeField] private AudioClip[] disappearSounds;
    [SerializeField] private AudioSource disappearAudioSource;
    //[SerializeField] private float disappearVolume = 0.7f;

    private void Awake()
    {
        // Инициализация AudioSource для появления
        if (appearAudioSource == null)
        {
            appearAudioSource = gameObject.AddComponent<AudioSource>();
            appearAudioSource.playOnAwake = false;
        }

        // Инициализация AudioSource для исчезновения
        if (disappearAudioSource == null)
        {
            disappearAudioSource = gameObject.AddComponent<AudioSource>();
            disappearAudioSource.playOnAwake = false;
        }
    }

    private void OnEnable()
    {
        PlayRandomAppearSound();
    }

    private void OnDisable()
    {
        PlayRandomDisappearSound();
    }

    private void PlayRandomAppearSound()
    {
        if (appearSounds.Length > 0 && appearAudioSource != null)
        {
            AudioClip clip = appearSounds[Random.Range(0, appearSounds.Length)];
            appearAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Звуки появления не назначены или AudioSource отсутствует!");
        }
    }

    private void PlayRandomDisappearSound()
    {
        if (disappearSounds.Length > 0 && disappearAudioSource != null)
        {
            AudioClip clip = disappearSounds[Random.Range(0, disappearSounds.Length)];
            disappearAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Звуки исчезновения не назначены или AudioSource отсутствует!");
        }
    }
}