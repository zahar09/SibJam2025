using UnityEngine;

public class FogOfWarSimple : MonoBehaviour
{
    [SerializeField] private GameObject fogCirclePrefab; // Префаб круга
    [SerializeField] private Transform player; // Ссылка на игрока
    [SerializeField] private float revealRadius = 5f; // Максимальный радиус
    [SerializeField] private float spawnDistance = 1f; // Расстояние между кругами
    [SerializeField] private float scaleDuration = 0.5f; // Время анимации появления

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = player.position;
    }

    private void Update()
    {
        if (Vector3.Distance(player.position, lastPosition) > spawnDistance)
        {
            CreateRevealCircle();
            lastPosition = player.position;
        }
    }

    private void CreateRevealCircle()
    {
        GameObject circle = Instantiate(fogCirclePrefab, player.position, Quaternion.identity, transform);
        Destroy(circle, 30f);
        circle.transform.localScale = Vector3.zero; // Начальный масштаб 0
        circle.GetComponent<FogCircleAnimation>()?.StartAnimation(revealRadius, scaleDuration);
    }
}