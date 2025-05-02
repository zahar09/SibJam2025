using UnityEngine;

public class SmoothCamera2D : MonoBehaviour
{
    [Header("��������� ����������")]
    [SerializeField] private Transform target;            // ������ ��������
    [SerializeField] private float smoothTime = 0.15f;   // ����� �����������
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // ��������

    [Header("�������")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private Vector3 _currentVelocity = Vector3.zero;
    private float _fixedZPosition;  // ������������� ������� �� Z

    private void Awake()
    {
        _fixedZPosition = transform.position.z;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // ������� ������� (������ X � Y �� ���� + ��������)
        Vector3 targetPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            _fixedZPosition  // ��������� �������� Z-�������
        );

        // ������� �������� � SmoothDamp (������������ Lerp)
        Vector3 smoothedPosition = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref _currentVelocity,
            smoothTime
        );

        // ����������� ���������
        if (useBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
        }

        transform.position = smoothedPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (!useBounds) return;

        Gizmos.color = Color.cyan;
        Vector3 bottomLeft = new Vector3(minBounds.x, minBounds.y, _fixedZPosition);
        Vector3 topRight = new Vector3(maxBounds.x, maxBounds.y, _fixedZPosition);

        Gizmos.DrawLine(bottomLeft, new Vector3(topRight.x, bottomLeft.y, _fixedZPosition));
        Gizmos.DrawLine(new Vector3(topRight.x, bottomLeft.y, _fixedZPosition), topRight);
        Gizmos.DrawLine(topRight, new Vector3(bottomLeft.x, topRight.y, _fixedZPosition));
        Gizmos.DrawLine(new Vector3(bottomLeft.x, topRight.y, _fixedZPosition), bottomLeft);
    }
}