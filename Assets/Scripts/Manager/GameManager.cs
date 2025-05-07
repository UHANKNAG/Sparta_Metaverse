using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    static GameManager gameManager;
    public PlayerController player {get; private set;} 
    private ResourceController _playerResourceController;

    [SerializeField] private int currentWaveIndex = 0;

    private EnemyManager enemyManager;

    public UIManager uiManager;
    public static bool isFirstLoading = true;

    public GameObject scoreText;

    public int currentScore = 0;
    public int bestScore = 0;
    public int BestScore { get => bestScore; }
    private const string BestScoreKey = "BestScore";

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);  

        gameManager = this;
        uiManager = FindObjectOfType<UIManager>();
        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);

        if (SceneManager.GetActiveScene().name == "DungeonScene")
        {
            player = FindObjectOfType<PlayerController>();
            player.Init(this);

            enemyManager = GetComponentInChildren<EnemyManager>();
            enemyManager.Init(this);

            _playerResourceController = player.GetComponent<ResourceController>();
            _playerResourceController.RemoveHealthChangeEvent(uiManager.ChangePlayerHP);
            _playerResourceController.AddHealthChangeEvent(uiManager.ChangePlayerHP);

            currentWaveIndex = -1;
        }
    }

    private void Start() {
        if(!isFirstLoading) {
            StartGame();
        }
        else {
            isFirstLoading = false;
        }
    }

    public void StartGame() {
        uiManager.SetPlayGame();
        StartNextWave();
    }

    void StartNextWave() {
        currentWaveIndex += 1;
        enemyManager.StartWave(1 + currentWaveIndex / 3);
        uiManager.ChangeWave(currentWaveIndex);
    }

    public void EndOfWave() {
        StartNextWave();
    }

    public void GameOver() {
        enemyManager.StopWave();
        uiManager.SetGameOver();
    }

    public void MiniGameOver() {
        scoreText.SetActive(false);
        uiManager.SetRestart();
        uiManager.SetScoreUI(currentScore, bestScore);
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score) {
        currentScore += score;
        Debug.Log("Score: " + currentScore);

        if(bestScore < currentScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
        }

        uiManager.UpdateScore(currentScore);
    }
}