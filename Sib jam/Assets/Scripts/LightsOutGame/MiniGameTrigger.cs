using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class MiniGameTrigger : Interactable
{
    [SerializeField] private GameObject miniGameUI;
    [SerializeField] private Sprite activeSprite;   // Спрайт активного состояния
    [SerializeField] private Sprite completedSprite; // Спрайт после прохождения
    [SerializeField] private UnityEvent onGameStart;
    [SerializeField] private UnityEvent onGameWin;

    private bool isMiniGameActive = false;
    private bool isGameCompleted = false;
    private SpriteRenderer spriteRenderer;
    private List<GameObject> playerObjects = new List<GameObject>();
    private List<bool> playerStates = new List<bool>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisualState();
    }

    protected override void OnEnterRange()
    {
        base.OnEnterRange();
        Debug.Log("Нажмите E чтобы начать мини-игру");
    }

    public override void Interact()
    {
        base.Interact();

        if (!isMiniGameActive && !isGameCompleted)
        {
            StartMiniGame();
        }
    }

    private void StartMiniGame()
    {
        SaveAndDisablePlayers();
        miniGameUI.SetActive(true);
        isMiniGameActive = true;

        LightsOutGame lightsOutGame = miniGameUI.GetComponentInChildren<LightsOutGame>();
        if (lightsOutGame != null)
        {
            lightsOutGame.onGameWin.AddListener(EndMiniGame);
        }

        onGameStart?.Invoke();
    }

    private void EndMiniGame()
    {
        miniGameUI.SetActive(false);
        isMiniGameActive = false;
        isGameCompleted = true;
        UpdateVisualState();

        RestorePlayers();
        onGameWin?.Invoke();
    }

    private void UpdateVisualState()
    {
        if (spriteRenderer == null) return;

        spriteRenderer.sprite = isGameCompleted ? completedSprite : activeSprite;
    }

    private void SaveAndDisablePlayers()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();

        foreach (var player in players)
        {
            playerObjects.Add(player.gameObject);
            playerStates.Add(player.gameObject.activeSelf);
            player.gameObject.SetActive(false);
        }
    }

    private void RestorePlayers()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i] != null)
            {
                playerObjects[i].SetActive(playerStates[i]);
            }
        }

        playerObjects.Clear();
        playerStates.Clear();
    }
}