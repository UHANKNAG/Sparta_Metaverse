using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Coroutine waveRoutine;

    [SerializeField] private List<GameObject> enemyPrefabs;

    [SerializeField] List<Rect> spawnAreas;
    [SerializeField] private Color gizmoColor = new Color(1, 0, 0, 0.3f);
    private List<EnemyController> activeEnemies = new List<EnemyController>();

    private bool enemySpawnComplite;

    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;

    GameManager gameManager;

    public void Init(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    public void StartWave(int waveCount) {
        // 다음 웨이브를 불러오게
        if (waveCount <= 0) {
            gameManager.EndOfWave();
            return;
        }

        if (waveRoutine != null) {
            StopCoroutine(waveRoutine);
        }

        //  StartCoroutine으로 실행시켜 줘야 Coroutine이 동작함
        waveRoutine = StartCoroutine(SpawnWave(waveCount));
    }

    public void StopWave() {
        StopAllCoroutines();
    }

    private IEnumerator SpawnWave(int waveCount) {
        enemySpawnComplite = false;
        // Wave 시작할 때 일정 시간 대기
        yield return new WaitForSeconds(timeBetweenWaves);

        for (int i = 0; i < waveCount; i++) {
            // 몬스터 생성하고, 기다리고 반복
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnRandomEnemy();
        }

        enemySpawnComplite = true;
    }

    private void SpawnRandomEnemy() {
        if(enemyPrefabs.Count == 0 || spawnAreas.Count == 0) {
            Debug.LogWarning("Enemy Prefabs 또는 Spawn Areas가 설정되지 않았습니다.");
            return;
        }

        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        // 영역 안에 랜덤 포지션 설정
        Vector2 randomPosition = new Vector2(
            Random.Range(randomArea.xMin, randomArea.xMax),
            Random.Range(randomArea.yMin, randomArea.yMax));

        // 생성
        GameObject spawnedEnemy = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y), Quaternion.identity);
        EnemyController enemyController = spawnedEnemy.GetComponent<EnemyController>();
        enemyController.Init(this, gameManager.player.transform);
        

        activeEnemies.Add(enemyController);
    }

    // Gizmo가 개발을 위한 아이콘 정도
    // 몬스터 생성 영역을 Rect로 설정하고 있는데 눈으로 봐야 작업이 편하니까...(?)
    private void OnDrawGizmosSelected() {
        if (spawnAreas == null) return;

        Gizmos.color = gizmoColor;
        foreach(var area in spawnAreas) {
            Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, area.height);

            Gizmos.DrawCube(center, size);
        }
    }

    // 스테이지 순환
    public void RemoveEnemyOnDeath(EnemyController enemy) {
        activeEnemies.Remove(enemy);

        if (enemySpawnComplite && activeEnemies.Count == 0) {
            gameManager.EndOfWave();
        }
    }
}
