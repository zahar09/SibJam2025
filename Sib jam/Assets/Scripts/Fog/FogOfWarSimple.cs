using UnityEngine;

public class FogOfWarSimple : MonoBehaviour
{
    [SerializeField] private GameObject fogCirclePrefab; // ������ �����
    [SerializeField] private Transform player; // ������ �� ������
    [SerializeField] private float revealRadius = 5f; // ������������ ������
    [SerializeField] private float spawnDistance = 1f; // ���������� ����� �������
    [SerializeField] private float scaleDuration = 0.5f; // ����� �������� ���������

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
        circle.transform.localScale = Vector3.zero; // ��������� ������� 0
        circle.GetComponent<FogCircleAnimation>()?.StartAnimation(revealRadius, scaleDuration);
    }
}