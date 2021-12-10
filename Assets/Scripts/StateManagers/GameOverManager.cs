using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    public static event EventHandler OnStateExit;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        RespawnManager.OnStateExit += RespawnManager_OnStateExit;
    }

    private void RespawnManager_OnStateExit(object sender, EventArgs e)
    {
        EnterState();
    }

    private void EnterState()
    {
        SetGameOver();
    }

    private void ExitState()
    {
        OnStateExit?.Invoke(this, EventArgs.Empty);
    }

    private bool IsGameOver()
    {
        foreach (GameObject hexagon in GridManager.BombHexagons)
        {
            if (ExplosionManager.Instance.IsCheckingInput)
                hexagon.GetComponent<Hexagon>().DecreaseBombCounter();

            if (hexagon.GetComponent<Hexagon>().HexagonBombCounter == 0)
                return true;
        }
        return false;
    }

    private void SetGameOver()
    {
        if (IsGameOver())
        {
            ScoreManager.StopScore();

            foreach (var hexagon in GridManager.Hexagons)
                Destroy(hexagon);

            UserInterfaceManager.Instance.DisplayGameOverScreen();
            UserInterfaceManager.Instance.HideInGameUI();
            GameStartManager.Instance.isGameStarted = false;
        }
        else
        {
            ExitState();
        }
    }

    public bool AnyMoveLeft()
    {
        foreach (var hexagon in GridManager.Hexagons)
        {
            foreach (var neighbour in GridManager.GetNeighbours(hexagon))
            {
                if (neighbour == null)
                    continue;

                hexagon.transform.position = neighbour.transform.position;

                if (hexagon.GetComponent<Hexagon>().CheckExplosion() != null)
                    return true;

                hexagon.transform.position = neighbour.transform.position;
            }

        }
        return false;
    }
}
