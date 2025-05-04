using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{
    public UnityEvent OnClick = new UnityEvent();

    private void OnMouseDown()
    {
        print("yes");
        OnClick?.Invoke();
    }
}