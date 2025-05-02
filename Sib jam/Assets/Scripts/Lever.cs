using UnityEngine;
using UnityEngine.Events;

public class Lever : Interactable
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private bool startActive = false;
    [SerializeField] private bool isToggleable = true;

    [Space(10)]
    [SerializeField] private UnityEvent onActivate;
    [SerializeField] private UnityEvent onDeactivate;

    private SpriteRenderer spriteRenderer;
    private bool isActive = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isActive = startActive;
        UpdateVisual();
    }

    public override void Interact()
    {
        base.Interact();

        if (isToggleable)
        {
            isActive = !isActive; // Переключение состояния
        }
        else
        {
            isActive = true; // Однократное активирование
        }

        UpdateVisual();

        if (isActive)
        {
            onActivate.Invoke(); // Вызов событий при активации
        }
        else
        {
            onDeactivate.Invoke(); // Вызов событий при деактивации
        }
    }

    private void UpdateVisual()
    {
        spriteRenderer.sprite = isActive ? activeSprite : inactiveSprite;
    }

    protected override void OnEnterRange()
    {
        base.OnEnterRange();
        Debug.Log($"Нажмите E чтобы {(isActive && isToggleable ? "выключить" : "включить")} рычаг");
    }
}