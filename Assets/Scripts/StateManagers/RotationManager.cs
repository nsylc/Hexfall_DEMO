using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotationManager : MonoBehaviour
{
    public static RotationManager Instance { get; private set; }
    public static event EventHandler<OnStateExitEventArgs> OnStateExit;
    public class OnStateExitEventArgs : EventArgs
    {
        public bool isOver;
    }
    private List<Vector3> TargetLocations = new List<Vector3>();
    private int counter = 0;
    public bool isOver = true;
    private bool isStepOver = true;
    private bool isClockwise = false;
    private float rotationSpeed = 2.5f;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InputManager.OnStateExit += InputManager_OnStateExit;
        ExplosionManager.OnStateExit += ExplosionManager_OnStateExit;
    }

    private void Update()
    {
        if (!isStepOver)
        {
            Rotate();
        }
    }

    private void InputManager_OnStateExit(object sender, InputManager.OnStateExitEventArgs e)
    {
        isClockwise = e.isClockwise;
        EnterState();
    }

    private void ExplosionManager_OnStateExit(object sender, ExplosionManager.OnStateExitEventArgs e)
    {
        if (e.stateToGo == States.Rotation)
            ContinueState();
    }

    private void EnterState()
    {
        isOver = false;
        isStepOver = false;
        counter = 0;
        TargetLocations.Clear();
    }

    private void ContinueState()
    {
        isStepOver = false;
        TargetLocations.Clear();
    }

    private void ExitState()
    {
        isStepOver = true;
        counter++;
        if (counter == 3)
            isOver = true;
        OnStateExit?.Invoke(this, new OnStateExitEventArgs { isOver = isOver });
    }

    public void Rotate()
    {
        List<GameObject> hexagons = SelectionManager.Instance.SelectedHexagons;
        if (hexagons == null || hexagons.Count == 0)
            return;

        AssignTargetLocations();

        if (isClockwise)
        {
            if (hexagons[0].transform.position != TargetLocations[1] && hexagons[1].transform.position != TargetLocations[2] && hexagons[2].transform.position != TargetLocations[0])
            {
                hexagons[0].transform.position = Vector3.MoveTowards(hexagons[0].transform.position, TargetLocations[1], Time.deltaTime * rotationSpeed);
                hexagons[1].transform.position = Vector3.MoveTowards(hexagons[1].transform.position, TargetLocations[2], Time.deltaTime * rotationSpeed);
                hexagons[2].transform.position = Vector3.MoveTowards(hexagons[2].transform.position, TargetLocations[0], Time.deltaTime * rotationSpeed);
            }
            else
                ExitState();
        }
        else
        {
            if (hexagons[0].transform.position != TargetLocations[2] && hexagons[1].transform.position != TargetLocations[0] && hexagons[2].transform.position != TargetLocations[1])
            {
                hexagons[0].transform.position = Vector3.MoveTowards(hexagons[0].transform.position, TargetLocations[2], Time.deltaTime * rotationSpeed);
                hexagons[1].transform.position = Vector3.MoveTowards(hexagons[1].transform.position, TargetLocations[0], Time.deltaTime * rotationSpeed);
                hexagons[2].transform.position = Vector3.MoveTowards(hexagons[2].transform.position, TargetLocations[1], Time.deltaTime * rotationSpeed);
            }
            else
                ExitState();
        }

    }

    private void AssignTargetLocations()
    {
        if (TargetLocations.Count != 0)
            return;

        List<GameObject> hexagons = SelectionManager.Instance.SelectedHexagons;
        if (hexagons == null || hexagons.Count == 0)
            UnexpectedExit();

        foreach (var selectedHexagon in SelectionManager.Instance.SelectedHexagons)
        {
            if (selectedHexagon == null)
                UnexpectedExit();
            TargetLocations.Add(selectedHexagon.transform.position);
        }
    }

    private void UnexpectedExit()
    {
        isOver = true;
        ExitState();
    }
}
