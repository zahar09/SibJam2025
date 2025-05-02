using UnityEngine;

public class WorldSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject world1; // ������ ��� (�������� �� ���������)
    [SerializeField] private GameObject world2; // ������ ��� (���������� �� ���������)

    private bool isWorld1Active = true;

    private void Start()
    {
        // ���������, ��������� �� ������� � ����������
        if (world1 == null || world2 == null)
        {
            Debug.LogError("��� �� �������� � ����������!");
            return;
        }

        // ��������, ��� ���������� ������ ��� ��������
        world2.SetActive(false);
    }

    private void Update()
    {
        // ������������ ����� ��� ������� Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWorld();
        }
    }

    private void SwitchWorld()
    {
        if (world1 == null || world2 == null) return;

        // ����������� ���������
        isWorld1Active = !isWorld1Active;

        world1.SetActive(isWorld1Active);
        world2.SetActive(!isWorld1Active);

        Debug.Log($"����������� �� {(isWorld1Active ? "��� 1" : "��� 2")}");
    }
}