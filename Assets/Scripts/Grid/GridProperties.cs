using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridProperties : MonoBehaviour
{
    public static GridProperties Instance { get; private set; }

    [SerializeField]
    private Vector2 gridSize = new Vector2(8, 9);
    public Vector3 GridSize { get { return gridSize; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}