using UnityEngine;

public class WorldSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject world1; // Первый мир (активный по умолчанию)
    [SerializeField] private GameObject world2; // Второй мир (неактивный по умолчанию)

    private bool isWorld1Active = true;

    private void Start()
    {
        // Проверяем, назначены ли объекты в инспекторе
        if (world1 == null || world2 == null)
        {
            Debug.LogError("Мир не назначен в инспекторе!");
            return;
        }

        // Убедимся, что изначально второй мир выключен
        world2.SetActive(false);
    }

    private void Update()
    {
        // Переключение миров при нажатии Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWorld();
        }
    }

    private void SwitchWorld()
    {
        if (world1 == null || world2 == null) return;

        // Переключаем состояния
        isWorld1Active = !isWorld1Active;

        world1.SetActive(isWorld1Active);
        world2.SetActive(!isWorld1Active);

        Debug.Log($"Переключено на {(isWorld1Active ? "мир 1" : "мир 2")}");
    }
}