using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool isPlayerInRange = false;
    public string interactionText = "Нажмите E для взаимодействия";

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
        // Базовая логика взаимодействия
    }

    protected virtual void OnEnterRange()
    {
        // Действия при входе в зону взаимодействия
    }

    protected virtual void OnExitRange()
    {
        // Действия при выходе из зоны взаимодействия
    }
}