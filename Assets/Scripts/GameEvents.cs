using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{

    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action onPlayerDeath;
    public event Action switchControlLock;
    public void PlayerDeath()
    {
        if (onPlayerDeath != null)
        {
            onPlayerDeath();
        }
    }

    public void SwitchControlLock()
    {
        if (switchControlLock != null)
        {
            switchControlLock();
        }
    }

}
