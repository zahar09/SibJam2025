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

    private bool hasPlayerEntered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, вошел ли игрок в зону
        if (other.CompareTag("Player") && !hasPlayerEntered)
        {
            hasPlayerEntered = true; // Защита от повторного вызова
            StartCoroutine(HandleDeathSequence());
        }
    }

    private IEnumerator HandleDeathSequence()
    {
        // 1. Затемнение экрана
        yield return FadeScreen(fadePanel, fadeColor, fadeDuration);

        // 2. Перезагрузка текущей сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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