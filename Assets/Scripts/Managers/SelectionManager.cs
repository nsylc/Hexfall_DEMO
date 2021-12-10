using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField]
    private List<GameObject> selectedHexagons = new List<GameObject>();
    public List<GameObject> SelectedHexagons { get { return selectedHexagons; } }

    public void Select(GameObject hexagon)
    {
        selectedHexagons.Add(hexagon);
        hexagon.GetComponent<Hexagon>().SpawnFrame();
    }

    public void MakeSelection(GameObject hexagon, RaycastHit2D hitInfo)
    {
        this.ClearSelectedHexagons();
        SelectionHitAreaManager.Instance.DetectHitArea(hexagon, hitInfo);
        this.Select(hexagon);
        NeighbourSelectionManager.Instance.SelectNeighbours(hexagon);
    }

    public void ClearSelectedHexagons()
    {
        foreach (var selectedHexagon in selectedHexagons)
        {
            if (selectedHexagon != null)
                selectedHexagon.GetComponent<Hexagon>().DestroyFrame();

        }
        selectedHexagons.Clear();
    }

    public Vector3 GetSelectionCenter()
    {
        if (selectedHexagons == null)
        {
            return Vector3.zero;
        }

        bool isOnLeft = true;
        foreach(GameObject hexagon in selectedHexagons)
        {
            if (hexagon != null)
            {
                if (TopHexagon().transform.position.x > hexagon.transform.position.x)
                    isOnLeft = false;
            }
           
        }

        float y = TopHexagon().transform.position.y + NeighbourOffsets.Bottom.y / 2;

        float offsetX = (isOnLeft) ? NeighbourOffsets.TopRight.x / 2 : NeighbourOffsets.TopLeft.x / 2;
        float x = TopHexagon().transform.position.x + offsetX;

        return new Vector3(x,y,0);
    }

    public GameObject TopHexagon()
    {
        GameObject topHexagon = null;
        float yAxis = -10;
        foreach(GameObject hexagon in selectedHexagons)
        {
            if (hexagon == null) return null;
            if (hexagon.transform.position.y > yAxis)
            {
                topHexagon = hexagon; 
                yAxis = topHexagon.transform.position.y;
            }

        }
        return topHexagon;
    }
}
