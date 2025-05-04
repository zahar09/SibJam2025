using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    private SpriteRenderer spriteRenderer;
    private bool isOn = false;

    public bool IsOn => isOn;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(bool startState)
    {
        isOn = startState;
        UpdateVisual();
    }

    public void Toggle()
    {
        isOn = !isOn;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        spriteRenderer.sprite = isOn ? onSprite : offSprite;
    }
}