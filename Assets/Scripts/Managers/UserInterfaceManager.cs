using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterfaceManager : MonoBehaviour
{
    public static UserInterfaceManager Instance { get; private set; }

    [SerializeField]
    private Canvas inGame = null;

    [SerializeField]
    private Canvas gameOverScreen = null;

    [SerializeField]
    private TMP_Text lastScore;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        inGame.GetComponentInChildren<Text>().text = ScoreManager.Score.ToString();
        lastScore.text = "Score: " + ScoreManager.Score.ToString();
    }

    public void DisplayGameOverScreen()
    {
        gameOverScreen.gameObject.SetActive(true);
    }

    public void HideGameOverScreen()
    {
        gameOverScreen.gameObject.SetActive(false);
    }

    public void DisplayInGameUI()
    {
        inGame.gameObject.SetActive(true);
    }

    public void HideInGameUI()
    {
        inGame.gameObject.SetActive(false);
    }

}
