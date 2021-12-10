using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    InitialSpawn,
    Input,
    WaitingRotationInput,
    Explosion,
    Rotation,
    SwipingDown,
    Reposition,
    Respawn,
    ExplosionDuringRotate,
    ExplosionDuringRespawn,
    GameOver,
    GameStart,
    CheckingIfGameOver,
}

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }

    public static States State = States.GameStart;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
