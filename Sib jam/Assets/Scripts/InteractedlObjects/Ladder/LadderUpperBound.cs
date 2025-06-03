using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderUpperBound : MonoBehaviour
{
    private bool isPlayerInTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            isPlayerInTrigger = false;
        }
    }

    public bool GetisPlayerInTrigger()
    {
        return isPlayerInTrigger;
    }
}
