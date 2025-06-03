using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool isPlayerInRange = false;
    public string interactionText = "������� E ��� ��������������";

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            OnEnterRange();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            OnExitRange();
        }
    }

    protected virtual void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public virtual void Interact()
    {
        // ������� ������ ��������������
    }

    protected virtual void OnEnterRange()
    {
        // �������� ��� ����� � ���� ��������������
    }

    protected virtual void OnExitRange()
    {
        // �������� ��� ������ �� ���� ��������������
    }
}