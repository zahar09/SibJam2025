using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 50f; // Скорость расхода в секунду

    [Header("UI")]
    [SerializeField] private Image staminaBar; // Просто Image (не RectTransform)

    private float currentStamina;

    private void Awake()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    private void Update()
    {
        
        DrainStamina();
        
    }

    private void DrainStamina()
    {
        currentStamina = Mathf.Max(0, currentStamina - staminaDrainRate * Time.deltaTime);
        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        float normalized = currentStamina / maxStamina;
        staminaBar.fillAmount = normalized;
    }
}