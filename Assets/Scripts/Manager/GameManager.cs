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

    private int currentScore = 0;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);  

        gameManager = this;
        uiManager = FindObjectOfType<UIManager>();

        if (SceneManager.GetActiveScene().name == "DungeonScene")
        {
            player = FindObjectOfType<PlayerController>();
            player.Init(this);

            enemyManager = GetComponentInChildren<EnemyManager>();
            enemyManager.Init(this);

            _playerResourceController = player.GetComponent<ResourceController>();
            _playerResourceController.RemoveHealthChangeEvent(uiManager.ChangePlayerHP);
            _playerResourceController.AddHealthChangeEvent(uiManager.ChangePlayerHP);
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
        enemyManager.StartWave(1 + currentWaveIndex / 5);
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
        uiManager.SetRestart();
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score) {
        currentScore += score;
        Debug.Log("Score: " + currentScore);
        uiManager.UpdateScore(currentScore);
    }
}