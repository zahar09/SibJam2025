using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [Header("UI ��������")]
    [SerializeField] private Button startButton; // ������ ������� �����
    [SerializeField] private Image fadePanel; // ������ ����������
    [SerializeField] private Image cutsceneImage; // �������� �����
    [SerializeField] private TextMeshProUGUI cutsceneText; // ����� �����

    [Header("��������� ������")]
    [SerializeField] private string[] messages; // ������ ���������
    [SerializeField] private float typeSpeed = 0.05f; // �������� ������
    [SerializeField] private float textDisplayTime = 3f; // ����� �����������
    [SerializeField] private float eraseSpeed = 0.03f; // �������� ��������

    [Header("�����")]
    [SerializeField] private AudioClip[] buttonSounds; // ������ ������ ������
    [SerializeField] private AudioSource audioSource; // AudioSource

    private void Awake()
    {
        // ������������� AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // �������� � ���������
        //if (startButton != null)
        //{
        //    startButton.onClick.AddListener(StartCutscene);
        //}

        // �������� ��������
        if (cutsceneImage != null) cutsceneImage.enabled = false;
        if (cutsceneText != null) cutsceneText.text = "";
    }

    public void StartCutscene()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // 1. ������������� ��������� ���� ������
        PlayRandomSound(buttonSounds);

        // 2. ���������� ������
        yield return FadeToBlack();

        // 3. ���������� ��������
        if (cutsceneImage != null) cutsceneImage.enabled = true;

        // 4. ������� �����
        if (cutsceneText != null && messages.Length > 0)
        {
            foreach (string message in messages)
            {
                yield return TypeText(message, typeSpeed);
                yield return new WaitForSeconds(textDisplayTime);
                yield return EraseText(eraseSpeed);
            }
        }

        // 5. ����� ����� ������
        yield return new WaitForSeconds(1f);

        // 6. ���������� ������
        yield return FadeToBlack();

        // 7. ������� � ��������� �����
        SceneManager.LoadScene("Level 2");
    }

    private void PlayRandomSound(AudioClip[] clips)
    {
        if (clips.Length > 0 && audioSource != null)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private IEnumerator FadeToBlack(float duration = 1f)
    {
        fadePanel.gameObject.SetActive(true);
        Color targetColor = Color.black;
        Color startColor = fadePanel.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            fadePanel.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        fadePanel.color = targetColor;
    }

    private IEnumerator TypeText(string message, float speed)
    {
        if (cutsceneText == null) yield break;

        cutsceneText.text = "";
        int charIndex = 0;

        while (charIndex < message.Length)
        {
            charIndex++;
            cutsceneText.text = message.Substring(0, charIndex);
            yield return new WaitForSeconds(speed);
        }
    }

    private IEnumerator EraseText(float speed)
    {
        if (cutsceneText == null) yield break;

        while (cutsceneText.text.Length > 0)
        {
            cutsceneText.text = cutsceneText.text.Substring(0, cutsceneText.text.Length - 1);
            yield return new WaitForSeconds(speed);
        }
    }
}