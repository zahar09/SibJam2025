using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsSoundManager : MonoBehaviour
{
    [SerializeField] PlayerController player;

    public void PlayFootStepsSound()
    {
        player.PlayRandomFootStepsSound();
    }
}
