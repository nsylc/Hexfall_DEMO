using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public static int hexValue = 50; //5

    private static bool isScoreStopped = true;
    public static bool IsScoreStopped { get { return isScoreStopped; } }

    private static int score = 0;
    public static int Score
    {
        get { return score; }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public static void IncreaseScore()
    {
        if (isScoreStopped)
            return;

        score += hexValue;

        if (score % 1000 == 0)
            RespawnManager.Instance.SpawnBombHexagon();
            
    }

    public static void Start()
    {
        isScoreStopped = true;
    }

    public static void StartScore()
    {
        isScoreStopped = false;
    }

    public static void StopScore()
    {
        isScoreStopped = true;
    }

    public static void ResetScore()
    {
        score = 0;
    }
}
