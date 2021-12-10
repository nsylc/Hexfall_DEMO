using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    public static event EventHandler OnStateExit;
    private bool spawnBombHexagon = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        RepositionManager.OnStateExit += RepositionManager_OnStateExit;
    }

    private void RepositionManager_OnStateExit(object sender, EventArgs e)
    {
        EnterState();
    }

    private void EnterState()
    {
        Respawn();
    }

    private void ExitState()
    {
        OnStateExit?.Invoke(this, EventArgs.Empty);
    }

    private void Respawn()
    {
        foreach (var position in GridManager.GridPositions)
        {
            if (GridManager.GetHexagon(position) == null)
            {
                if (spawnBombHexagon)
                {
                    SpawnManager.Instance.SpawnBombHexagon(position);
                    spawnBombHexagon = false;
                }
                else
                    SpawnManager.Instance.SpawnHexagon(position);
            }
        }
        ExitState();
    }

    public void SpawnBombHexagon()
    {
        spawnBombHexagon = true;
    }
}
