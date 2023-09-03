using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private PlayerController playerPrefab;

    private PlayerController playerController;
    private EnemyController enemyController;
    private int maxEnemiesInWave = 3;
    private int enemiesToSpawn = 0;
    private int currentLevel = 1;
    private int currentEnemies = 0;
    public bool isGameRunning { get; private set; }

    public void PrepareGameplay(int level)
    {
        currentLevel = level;
        enemiesToSpawn = level;
        SpawnPlayer();
        StartCoroutine(SpawnEnemies());
    }

    public void StartGamePlay()
    {
        isGameRunning = true;
    }

    private void SpawnPlayer()
    {
        Transform trans = SpawnWaypointHandler.Instance.GetSpawnPoint();
        playerController = Instantiate<PlayerController>(playerPrefab, trans.position, trans.rotation);
        playerController.Initialize(UIController.Instance.moveJoystick,
            UIController.Instance.rotateJoystick, UIController.Instance.healthBar);
    }

    private IEnumerator SpawnEnemies()
    {
        int enemies = 0;
        enemies = enemiesToSpawn;
        if (enemiesToSpawn > maxEnemiesInWave) { enemies = maxEnemiesInWave; }

        for (int i = 0; i < enemies; i++)
        {
            Transform[] wayPoints = SpawnWaypointHandler.Instance.GetWayPoints();
            EnemyController enemy = Instantiate<EnemyController>(enemyPrefab, wayPoints[Random.Range(0, wayPoints.Length)].position, Quaternion.identity);
            enemy.SetUpEnemy(playerController, wayPoints);
            enemiesToSpawn--;
            currentEnemies++;
            yield return null;
        }
    }

    private IEnumerator StartLevel(int level)
    {
        //GameplayUIController.Instance.EnableLevelDisplayScreen(level);
        currentLevel = level;
        enemiesToSpawn = level;
        //GameplayUIController.Instance.UpdateLevelTargetText(level);
        StartCoroutine(SpawnEnemies());

        yield return new WaitForSeconds(5);

        //GameplayUIController.Instance.DisableLevelDisplayScreen();
        isGameRunning = true;
    }

    private void StartNextLevel()
    {
        isGameRunning = false;
        currentLevel++;
        enemiesToSpawn = currentLevel;
        //playerPrefab.ResetHealth();
        StartCoroutine(StartLevel(currentLevel));
    }

    

    public void EnemyDied()
    {
        currentEnemies--;
        if(currentEnemies == 0 && enemiesToSpawn > 0)
        {
            StartCoroutine(SpawnEnemies());
        } else if( currentEnemies == 0)
        {
            StartNextLevel();
        }
    }
}
