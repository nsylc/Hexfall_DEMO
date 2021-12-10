using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public static List<GameObject> Hexagons
    {
        get
        {
            List<GameObject> hexagons = new List<GameObject>();

            foreach (Vector3 position in GridPositions)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(position, Vector2.zero);
                if (raycastHit2D.collider != null && raycastHit2D.transform.gameObject.tag == "Hexagon")
                    hexagons.Add(raycastHit2D.transform.gameObject);
            }
            return hexagons;
        }
    }

    public static List<Vector3> GridPositions
    {
        get
        {
            List<Vector3> positions = new List<Vector3>();

            Vector2 gridSize = GridProperties.Instance.GridSize;
            Vector2 offset = new Vector2(NeighbourOffsets.TopRight.x, NeighbourOffsets.Top.y);

            for (float x = 0f; x < (gridSize.x * offset.x) * 0.99f; x += offset.x)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector3 spawnPosition = new Vector3(x, (y * offset.y), 0);
                    spawnPosition.y = ((Mathf.Round(x / offset.x) % 2) == 0) ? spawnPosition.y : spawnPosition.y - offset.y / 2;
                    positions.Add(spawnPosition);
                }
            }
            return positions;
        }
    }

    public static List<List<Vector3>> GridPositions2D
    {
        get
        {
            Vector2 gridSize = GridProperties.Instance.GridSize;
            Vector2 offset = new Vector2(NeighbourOffsets.TopRight.x, NeighbourOffsets.Top.y);

            List<List<Vector3>> positions = new List<List<Vector3>>();

            for (float x = 0f; x < (gridSize.x * offset.x) * 0.99f; x += offset.x)
            {
                List<Vector3> columnList = new List<Vector3>();

                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector3 spawnPosition = new Vector3(x, (y * offset.y), 0);
                    spawnPosition.y = ((Mathf.Round(x / offset.x) % 2) == 0) ? spawnPosition.y : spawnPosition.y - offset.y / 2;
                    columnList.Add(spawnPosition);
                }
                positions.Add(columnList);
            }

            return positions;
        }
    }

    public static List<List<GameObject>> Hexagons2D
    {
        get
        {
            List<List<GameObject>> hexagons = new List<List<GameObject>>();

            foreach (List<Vector3> column in GridPositions2D)
            {
                List<GameObject> hexagonsOnThisColumn = new List<GameObject>();
                foreach (Vector3 position in column)
                {
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(position, Vector2.zero);
                    if (raycastHit2D.collider != null && raycastHit2D.transform.gameObject.tag == "Hexagon")
                        hexagonsOnThisColumn.Add(raycastHit2D.transform.gameObject);
                    else
                        hexagonsOnThisColumn.Add(null);
                }
                hexagons.Add(hexagonsOnThisColumn);
            }

            return hexagons;
        }
    }

    public static List<GameObject> BombHexagons
    {
        get
        {
            List<GameObject> bombHexagons = new List<GameObject>();
            foreach (GameObject hexagon in Hexagons)
            {
                if (hexagon.GetComponent<Hexagon>().isBombHexagon)
                    bombHexagons.Add(hexagon);

            }
            return bombHexagons;
        }
    }

    public static GameObject GetHexagon(Vector3 position)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(position, Vector2.zero);
        if (raycastHit2D.collider != null && raycastHit2D.transform.gameObject.tag == "Hexagon")
            return raycastHit2D.transform.gameObject;
        else
            return null;
    }

    public static List<GameObject> GetNeighbours(GameObject hexagon)
    {
        List<GameObject> neighbours = new List<GameObject>();
        foreach (Vector2 offset in NeighbourOffsets.ToList())
        {
            Vector3 offset3 = new Vector3(offset.x, offset.y, hexagon.transform.position.z);
            GameObject neighbour = GridManager.GetHexagon(hexagon.transform.position + offset3);
            neighbours.Add(neighbour);
        }
        return neighbours;
    }
}