using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public enum UIState {
    Home,
    Game,
    GameOver,
    Score
}

public class UIManager : MonoBehaviour
{
    GameManager gameManager = GameManager.Instance;
    HomeUI homeUI;
    GameUI gameUI;
    GameOverUI gameOverUI;
    ScoreUI scoreUI;

    private UIState currentState;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI restartText;

    private void Awake() {
        if (SceneManager.GetActiveScene().name == "DungeonScene") {
            homeUI = GetComponentInChildren<HomeUI>(true);
            homeUI.Init(this);
            gameUI = GetComponentInChildren<GameUI>(true);
            gameUI.Init(this);
            gameOverUI = GetComponentInChildren<GameOverUI>(true);
            gameOverUI.Init(this);

            ChangeState(UIState.Game);
        }
        
        if (SceneManager.GetActiveScene().name == "MiniGameScene") {
            scoreUI = GetComponentInChildren<ScoreUI>(true);
            scoreUI?.Init(this);
        }  
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MiniGameScene") {
            if (restartText == null) {
                Debug.LogError("restart text is null");
            }

            if (scoreText == null) {
                Debug.LogError("socre text is null");
            }

            restartText.gameObject.SetActive(false);
        }

    }

    public void SetPlayGame() {
        ChangeState(UIState.Game);
    }

    public void SetGameOver() {
        ChangeState(UIState.GameOver);
    }

    public void ChangeWave(int waveIndex) {
        gameUI.UpdateWaveText(waveIndex);
    }

    public void ChangePlayerHP(float currentHP, float maxHP) {
        gameUI.UpdateHPSlider(currentHP / maxHP);
    }

    public void ChangeState(UIState state) {
        currentState = state;
        // homeUI.SetActive(currentState);
        if (SceneManager.GetActiveScene().name == "DungeonScene") {
            gameUI.SetActive(currentState);
            gameOverUI.SetActive(currentState);
        }
        
        if (SceneManager.GetActiveScene().name == "MiniGameScene")
            scoreUI?.SetActive(currentState);
    }

    public void SetRestart() {
        
        scoreUI.gameObject.SetActive(true);
    }

    public void UpdateScore(int score) {
        scoreText.text = score.ToString();
    }

    public void SetScoreUI(int currentScore, int BestScore)
    {
        scoreUI.SetUI(currentScore, BestScore);
        ChangeState(UIState.Score);
    }

    public void OnClickStart() {
        gameManager.RestartGame();
        ChangeState(UIState.Game);
    }

    public void OnClickExit() {
        SceneManager.LoadScene("MainScene");
    }
}
