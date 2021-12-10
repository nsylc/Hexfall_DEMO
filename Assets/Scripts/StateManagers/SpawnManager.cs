using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public static event EventHandler OnStateExit;

    [SerializeField]
    private GameObject hexagonPrefab = null;
    public GameObject HexagonPrefab { get { return hexagonPrefab; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GameStartManager.Instance.OnStateExit += GameStartManager_OnStateExit;
    }

    private void GameStartManager_OnStateExit(object sender, EventArgs e)
    {
        EnterState();
    }

    private void EnterState()
    {
        InitialSpawn();
    }

    private void ExitState()
    {
        OnStateExit?.Invoke(this, EventArgs.Empty);
    }

    private void InitialSpawn()
    {
        foreach (Vector3 position in GridManager.GridPositions)
            SpawnHexagon(position);

        ExitState();
    }

    public GameObject SpawnHexagon(Vector3 positionToSpawn)
    {
        GameObject spawnedHexagon = Instantiate(hexagonPrefab, positionToSpawn, Quaternion.identity);
        return spawnedHexagon;
    }

    public GameObject SpawnBombHexagon(Vector3 positionToSpawn)
    {
        GameObject spawnedHexagon = Instantiate(hexagonPrefab, positionToSpawn, Quaternion.identity);
        spawnedHexagon.GetComponent<Hexagon>().SetAsBombHexagon();
        return spawnedHexagon;
    }
}
