using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject world1;
    [SerializeField] private GameObject world2;
    [SerializeField] private Image fadePanel; // ������ �� UI-����������
    [SerializeField] private float fadeDuration = 0.5f; // ����� ��������

    private bool isWorld1Active = true;

    private void Start()
    {
        if (world1 == null || world2 == null || fadePanel == null)
        {
            Debug.LogError("�� ��� ������� ��������� � ����������!");
            return;
        }

        world2.SetActive(false);
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
        // 1. ��������� �����
        yield return FadeScreen(1f);

        // 2. ����������� ����
        isWorld1Active = !isWorld1Active;

        world1.SetActive(isWorld1Active);
        world2.SetActive(!isWorld1Active);

        // 3. �������� �����
        yield return FadeScreen(0f);
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
}