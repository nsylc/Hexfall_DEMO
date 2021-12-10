using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager Instance { get; private set; }

    public static event EventHandler<OnStateExitEventArgs> OnStateExit;
    public class OnStateExitEventArgs : EventArgs
    {
        public States stateToGo;
    }

    private bool isAnyExploded = false;
    public bool IsAnyExploded { get { return isAnyExploded; } }
    private bool isCheckingInput = false;
    public bool IsCheckingInput { get { return isCheckingInput; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SpawnManager.OnStateExit += SpawnManager_OnStateExit;
        GameOverManager.OnStateExit += GameOverManager_OnStateExit;
        RotationManager.OnStateExit += RotationManager_OnStateExit;
    }

    private void SpawnManager_OnStateExit(object sender, EventArgs e)
    {
        isCheckingInput = false;
        EnterState();
    }

    private void GameOverManager_OnStateExit(object sender, EventArgs e)
    {
        isCheckingInput = false;
        EnterState();
    }

    private void RotationManager_OnStateExit(object sender, RotationManager.OnStateExitEventArgs e)
    {
        isCheckingInput = true;
        EnterState();
    }

    private void EnterState()
    {
        Explode();
    }

    private void ExitState()
    {
        States stateToGo;

        if (isAnyExploded)
            stateToGo = States.Reposition;
        else
        {
            if (RotationManager.Instance.isOver)
                stateToGo = States.Input;
            else
                stateToGo = States.Rotation;
        }
        OnStateExit?.Invoke(this, new OnStateExitEventArgs { stateToGo = stateToGo } );
    }

    public void Explode()
    {
        isAnyExploded = false;

        foreach (GameObject hexagon in HexagonsToExplode())
        {
            if(hexagon != null)
            {
                Destroy(hexagon);
                isAnyExploded = true;
            }
        }

        if (isAnyExploded)
        {
            RotationManager.Instance.isOver = true;

            foreach (GameObject hexagon in SelectionManager.Instance.SelectedHexagons)
            {
                if (hexagon)
                    hexagon.GetComponent<Hexagon>().DestroyFrame();
            }
        }
        ExitState();
    }

    public List<GameObject> HexagonsToExplode()
    {
        List<GameObject> hexagonsToExplode = new List<GameObject>();

        foreach (GameObject hexagon in GridManager.Hexagons)
            hexagonsToExplode.Add(hexagon.GetComponent<Hexagon>().CheckExplosion());

        return hexagonsToExplode;
    }
}
