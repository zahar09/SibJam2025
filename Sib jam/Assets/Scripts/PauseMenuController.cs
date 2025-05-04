using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float buttonsDelay = 0.2f;
    [SerializeField] private float scaleDuration = 0.3f;
    [SerializeField] private float targetDarkness = 0.7f;
    [SerializeField] private Ease fadeEase = Ease.OutQuad;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    [Header("UI References")]
    [SerializeField] private CanvasGroup pauseOverlay;
    [SerializeField] private GameObject[] menuButtons;
    [SerializeField] private Image darkeningPanel;

    private bool isPaused = false;
    private List<Tween> activeTweens = new List<Tween>();
    private AudioChannelSwitcher channelSwitcher;

    private void Start()
    {
        channelSwitcher = FindAnyObjectByType<AudioChannelSwitcher>();
        InitializeMenu();

        channelSwitcher.DisableChannel("Menu");
    }

    private void InitializeMenu()
    {
        pauseOverlay.alpha = 0;
        pauseOverlay.blocksRaycasts = false;
        pauseOverlay.interactable = false;

        foreach (var button in menuButtons)
        {
            button.transform.localScale = Vector3.zero;
            button.SetActive(false);
        }

        darkeningPanel.color = new Color(0, 0, 0, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {

        channelSwitcher.EnableChannel("Menu");
        channelSwitcher.DisableChannel("LevelVirtual");
        channelSwitcher.DisableChannel("LevelReal");

        isPaused = true;
        Time.timeScale = 0;

        ClearActiveTweens();
        pauseOverlay.gameObject.SetActive(true);
        pauseOverlay.alpha = 1f;

        // Debug затемнения
        Debug.Log("Starting screen darkening...");

        var fadeTween = darkeningPanel.DOFade(targetDarkness, fadeDuration)
            .SetEase(fadeEase)
            .SetUpdate(true)
            .OnComplete(() => Debug.Log("Screen darkened!"));
        activeTweens.Add(fadeTween);

        // Анимация кнопок
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i] == null)
            {
                Debug.LogError($"Button {i} is not assigned!");
                continue;
            }

            GameObject button = menuButtons[i];
            button.SetActive(true);

            Debug.Log($"Animating button {i} ({button.name})...");

            var buttonTween = button.transform.DOScale(1, scaleDuration)
                .SetDelay(fadeDuration + i * buttonsDelay)
                .SetEase(scaleEase)
                .SetUpdate(true)
                .OnStart(() => Debug.Log($"Start animating {button.name}"))
                .OnComplete(() => Debug.Log($"{button.name} animation complete"));

            activeTweens.Add(buttonTween);
        }

        pauseOverlay.blocksRaycasts = true;
        pauseOverlay.interactable = true;
    }

    private void ResumeGame()
    {
        channelSwitcher.DisableChannel("Menu");
        channelSwitcher.EnableChannel("LevelVirtual");
        channelSwitcher.EnableChannel("LevelReal");

        ClearActiveTweens();

        // Последовательное скрытие кнопок в обратном порядке
        for (int i = menuButtons.Length - 1; i >= 0; i--)
        {
            GameObject button = menuButtons[i];

            var buttonTween = button.transform.DOScale(0, scaleDuration)
                .SetDelay((menuButtons.Length - 1 - i) * buttonsDelay)
                .SetEase(fadeEase)
                .SetUpdate(true)
                .OnComplete(() => button.SetActive(false));

            activeTweens.Add(buttonTween);
        }

        // Осветление экрана после скрытия кнопок
        var fadeTween = darkeningPanel.DOFade(0, fadeDuration)
            .SetDelay(menuButtons.Length * buttonsDelay)
            .SetEase(fadeEase)
            .SetUpdate(true)
            .OnComplete(() => {
                pauseOverlay.blocksRaycasts = false;
                pauseOverlay.interactable = false;
                isPaused = false;
                Time.timeScale = 1;
            });

        activeTweens.Add(fadeTween);
    }

    private void ClearActiveTweens()
    {
        foreach (var tween in activeTweens)
        {
            if (tween != null && tween.IsActive()) tween.Kill();
        }
        activeTweens.Clear();
    }

    // Методы для кнопок (добавьте их вручную на каждую кнопку)
    public void ResumeGameButton() => ResumeGame();
    public void RestartLevelButton() { /* реализация */ }
    public void SettingsButton() { /* реализация */ }
    public void MainMenuButton() { /* реализация */ }
}