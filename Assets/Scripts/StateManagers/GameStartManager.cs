using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    public static GameStartManager Instance { get; private set; }

    public event EventHandler OnStateExit;

    public bool isGameStarted = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartGame()
    {
        if(!isGameStarted)
        {
            EnterState();
        }
    }

    private void EnterState()
    {
        Configure();
        ExitState();
    }

    private void ExitState()
    {
        OnStateExit?.Invoke(this, EventArgs.Empty);
    }

    private void Configure()
    {
        isGameStarted = true;
        InputManager.Instance.isFirstInput = true;
        UserInterfaceManager.Instance.HideGameOverScreen();
        ScoreManager.ResetScore();
        RepositionManager.Instance.SetSpeed(10);
    }

public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
