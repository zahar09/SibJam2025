using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Button : Interactable
{
    [SerializeField] private UnityEvent onActivate;
    [SerializeField] private UnityEvent onDeactivate;
    [SerializeField] private float delay = 1f;

    private bool isPressed = false;
    private Coroutine toggleCoroutine = null;

    public override void Interact()
    {
        base.Interact();

        // Останавливаем предыдущую корутину, если она есть
        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
            Debug.Log("Предыдущая корутина остановлена");
        }

        // Активируем кнопку
        isPressed = true;
        Debug.Log("Кнопка активирована");
        onActivate.Invoke();

        // Запускаем таймер для деактивации
        toggleCoroutine = StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        Debug.Log($"Ожидание {delay} секунд...");
        yield return new WaitForSeconds(delay);
        Debug.Log("Таймер сработал");

        // Деактивируем кнопку
        isPressed = false;
        Debug.Log("Кнопка деактивирована");
        onDeactivate.Invoke();
        toggleCoroutine = null;
    }

    protected override void OnEnterRange()
    {
        base.OnEnterRange();
        Debug.Log("Игрок вошёл в зону кнопки");
    }

    protected override void OnExitRange()
    {
        base.OnExitRange();
        Debug.Log("Игрок вышел из зоны кнопки");

        // ❌ Убрали остановку корутины при выходе
        // ✅ Теперь корутина продолжит работу даже после выхода игрока
    }
}