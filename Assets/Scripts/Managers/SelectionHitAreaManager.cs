using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHitAreaManager : MonoBehaviour
{
    public static SelectionHitAreaManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private int hitArea = 1;
    public int HitArea { get { return hitArea; } }

    //Calculates rotation between two points degrees
    public int DetectHitArea(GameObject hexagon, RaycastHit2D hitInfo)
    {
        Vector2 distance = hitInfo.point - new Vector2(hexagon.transform.position.x, hexagon.transform.position.y);

        float angleToHitPoint = Mathf.Atan2(distance.y, distance.x) * 180 / Mathf.PI; 
        if (angleToHitPoint < 0)
            angleToHitPoint += 360;

        hitArea = Mathf.CeilToInt(angleToHitPoint / 60);
        return hitArea;
    }

    public void SwitchToNextArea()
    {
        if (hitArea != 6)
            hitArea += 1;
        else
            hitArea = 1;
    }
}
