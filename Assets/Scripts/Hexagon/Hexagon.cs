using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Hexagon : MonoBehaviour
{
    public static readonly float longRadius = 1;
    public static readonly float shortRadius = 0.866f;
    public static readonly float margin = 0.1f;

    public bool isBombHexagon { get; private set; }

    private int hexagonBombCounter = 5;
    public int HexagonBombCounter { get {return hexagonBombCounter; } }

    private GameObject hexagonFrame = null;

    private GameObject hexagonBomb = null;

    private GameObject hexagonBombCounterText = null;

    ////PARTICLE

    private void Awake()
    {
        AssignRandomMaterial();
    }

    private void AssignRandomMaterial()
    {
        int randomNumber = Random.Range(0, HexagonProperties.Instance.Materials.Capacity);
        GetComponent<Renderer>().material = HexagonProperties.Instance.Materials[randomNumber];
    }

    public void SetAsBombHexagon()
    {
        isBombHexagon = true;

        Vector3 positionToSpawn = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.01f);
        hexagonBomb = Instantiate(HexagonProperties.Instance.HexagonBombPrefab, positionToSpawn, Quaternion.identity);
        hexagonBomb.transform.SetParent(transform);

        positionToSpawn = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.02f);
        hexagonBombCounterText = Instantiate(HexagonProperties.Instance.HexagonBombCounterPrefab, positionToSpawn, Quaternion.identity);
        hexagonBombCounterText.transform.SetParent(transform);
        hexagonBombCounterText.GetComponentInChildren<TextMeshProUGUI>().text = hexagonBombCounter.ToString();
    }

    public void DecreaseBombCounter()
    {
        if (isBombHexagon)
        {
            hexagonBombCounter--;
            hexagonBombCounterText.GetComponentInChildren<TextMeshProUGUI>().text = hexagonBombCounter.ToString();
        }
    }

    public GameObject CheckExplosion()
    {
        List<GameObject> neighbours = GridManager.GetNeighbours(gameObject);
        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i] == null)
                continue;

            if (GetComponent<Renderer>().material.color == neighbours[i].GetComponent<Renderer>().material.color)
            {
                int nextIndex = (i + 1) > 5 ? 0 : (i + 1);

                if (neighbours[nextIndex] == null)
                    continue;

                if (GetComponent<Renderer>().material.color == neighbours[nextIndex].GetComponent<Renderer>().material.color)
                    return gameObject;
            }
        }
        return null;
    }

    public void SpawnFrame()
    {
        hexagonFrame = Instantiate(HexagonProperties.Instance.HexagonFramePrefab, transform.position, Quaternion.identity);
        hexagonFrame.transform.SetParent(transform);
    }

    public void DestroyFrame()
    {
        Destroy(hexagonFrame);
    }

    public Vector3 MoveTowards(Vector3 origin, Vector3 target, float maxDistanceDelta)
    {
        Vector3 positionToSet = Vector3.MoveTowards(origin, target, Time.deltaTime);

        transform.position = positionToSet;

        return positionToSet;
    }

    private void OnDestroy()
    {
        if (!ScoreManager.IsScoreStopped)
            ScoreManager.IncreaseScore();
        //////////////////// PARTICLE FX from PROPs ////////////////////
    }
}

