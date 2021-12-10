using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionManager : MonoBehaviour
{
    public static RepositionManager Instance { get; private set; }

    public static event EventHandler OnStateExit;

    private float repositionSpeed = 10f;
    public float RepositionSpeed { get { return repositionSpeed; } }

    Dictionary<GameObject, Vector3> targetPositions = new Dictionary<GameObject, Vector3>();

    bool isStateOver = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ExplosionManager.OnStateExit += ExplosionManager_OnStateExit;
    }

    private void Update()
    {
        if (!isStateOver)
            Reposition();
    }

    private void ExplosionManager_OnStateExit(object sender, ExplosionManager.OnStateExitEventArgs e)
    {
        if (e.stateToGo == States.Reposition)
            EnterState();
    }

    private void EnterState()
    {
        PrepareState();
    }

    private void ExitState()
    {
        isStateOver = true;
        targetPositions.Clear();
        OnStateExit?.Invoke(this, EventArgs.Empty);
    }

    private void PrepareState()
    {
        isStateOver = false;
        targetPositions.Clear();
    }

    private void Reposition()
    {
        if (targetPositions.Count == 0)
            AssignTargetPositions();
        else
            Move();

        if (isStateOver)
            ExitState();
    }

    private void Move()
    {
        bool isRepositioned = false;
        foreach (var x in targetPositions)
        {
            if (x.Key.transform.position != x.Value)
            {
                x.Key.transform.position = Vector3.MoveTowards(x.Key.transform.position, x.Value, Time.deltaTime * repositionSpeed);
                isRepositioned = true;
            }
        }

        if (isRepositioned == false)
            isStateOver = true;
    }

    void AssignTargetPositions()
    {
        for (int x = 0; x < GridManager.GridPositions2D.Count; x++)
        {
            for (int y = 0; y < GridManager.GridPositions2D[x].Count - 1; y++)
            {
                if (GridManager.GetHexagon(GridManager.GridPositions2D[x][y]) == null)
                {
                    int index = y + 1;
                    while (index < GridManager.GridPositions2D[x].Count)
                    {
                        GameObject topHexagon = GridManager.GetHexagon(GridManager.GridPositions2D[x][index]);
                        if (topHexagon != null)
                        {
                            Vector3 targetPosition =
                                topHexagon.transform.position + new Vector3(NeighbourOffsets.Bottom.x, NeighbourOffsets.Bottom.y, topHexagon.transform.position.z);
                            if (targetPositions.ContainsKey(topHexagon))
                                targetPositions[topHexagon] += new Vector3(NeighbourOffsets.Bottom.x, NeighbourOffsets.Bottom.y, topHexagon.transform.position.z);
                            else
                                targetPositions.Add(topHexagon, targetPosition);
                        }
                        index++;
                    }
                }

            }
        }

        if (targetPositions.Count == 0)
        {
            ExitState();
        }
    }

    public void SetSpeed(float newSpeed)
    {
        repositionSpeed = newSpeed;
    }
}