using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonProperties : MonoBehaviour
{
    #region Singleton Pointer
    public static HexagonProperties Instance { get; private set; }
    #endregion

    [SerializeField]
    private List<Material> materials;
    public List<Material> Materials { get { return materials; } }

    [SerializeField]
    private GameObject hexagonFramePrefab;
    public GameObject HexagonFramePrefab { get { return hexagonFramePrefab; } }

    [SerializeField]
    private GameObject hexagonBombPrefab;
    public GameObject HexagonBombPrefab { get { return hexagonBombPrefab; } }

    [SerializeField]
    private GameObject hexagonBombCounterPrefab;
    public GameObject HexagonBombCounterPrefab { get { return hexagonBombCounterPrefab; } }

    //////////////////// PARTICLE FX ////////////////////
    /*[SerializeField]
    private ParticleSystem hexagonParticlePrefab;
    public ParticleSystem HexagonParticlePrefab { get { return hexagonParticlePrefab; } }*/

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}