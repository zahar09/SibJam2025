using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadlyZone : MonoBehaviour
{
    [Header("��������� ����������")]
    [SerializeField] private Image fadePanel; // UI-������ ��� ����������
    [SerializeField] private float fadeDuration = 1.5f; // ����� ����������
    [SerializeField] private Color fadeColor = Color.black; // ���� ����������

    [Header("����� ������")]
    [SerializeField] private AudioClip[] deathSounds; // ������ ������ ������
    [SerializeField] private AudioSource audioSource; // AudioSource ��� ���������������
    [SerializeField] private float deathSoundVolume = 0.7f; // ��������� �����

    private bool hasPlayerEntered = false;

    private void Awake()
    {
        // ������������� AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, ����� �� ����� � ����
        if (other.CompareTag("Player") && !hasPlayerEntered)
        {
            hasPlayerEntered = true;
            StartCoroutine(HandleDeathSequence());
        }
    }

    private IEnumerator HandleDeathSequence()
    {
        // 1. ����������� ��������� ���� ������
        PlayRandomDeathSound();

        // 2. ���������� ������
        yield return FadeScreen(fadePanel, fadeColor, fadeDuration);

        // 3. ������������ ������� �����
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
            Debug.LogWarning("����� ������ �� ��������� ��� AudioSource �����������!");
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