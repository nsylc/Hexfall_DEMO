using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionManager : MonoBehaviour
{
    void Start()
    {
        float x = (GridProperties.Instance.GridSize.x * Hexagon.longRadius * 0.75f) / 2;
        float y = (GridProperties.Instance.GridSize.y * Hexagon.shortRadius) / 2;
        transform.position = new Vector3(x, y, transform.position.z);

        this.GetComponent<Camera>().orthographicSize = GridProperties.Instance.GridSize.x; //GridSize.x - 1 -> kenarlara degiyor
    }
}
