using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private PlayerController playerPrefab;

    private PlayerController playerController;
    private List<EnemyController> enemyControllers = new List<EnemyController>();
    private int maxEnemiesInWave = 3;
    private int enemiesToSpawn = 0;
    private int currentLevel = 1;
    private bool isGameRunning;

    public void PrepareGameplay(int level)
    {
        currentLevel = level;
        enemiesToSpawn = level;
        UIController.Instance.UpdateLevelTargetText(level);
        UIController.Instance.UpdateKillText(0);
        SpawnPlayer();
        StartCoroutine(SpawnEnemies());
    }

    public void StartLevel()
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
            enemyControllers.Add(enemy);
            yield return null;
        }
    }

    public void RetryLevel()
    {
        ClearEnemies();
        enemiesToSpawn = currentLevel;
        UIController.Instance.UpdateLevelTargetText(currentLevel);
        UIController.Instance.UpdateKillText(0);
        StartCoroutine(SpawnEnemies());
        playerController.Reset(SpawnWaypointHandler.Instance.GetSpawnPoint());
    }

    public bool IsGameRunning
    {
        get { return isGameRunning; }
        set { isGameRunning = value; }
    }

    public int CurrentLevel
    {
        get { return currentLevel; }
    }

    public void EnemyDied(EnemyController diedEnemy)
    {
        foreach(EnemyController enemy in enemyControllers)
        {
            if(enemy == diedEnemy)
            {
                enemyControllers.Remove(enemy);
                break;
            }
        }

        if(enemyControllers.Count == 0 && enemiesToSpawn > 0)
        {
            StartCoroutine(SpawnEnemies());
        } 
        else if(enemyControllers.Count == 0)
        {
            //StartNextLevel();
        }
    }

    public void PlayerDied()
    {
        UIController.Instance.EnableGameOverScreen();
        isGameRunning = false;
    }

    public void ClearEnemies()
    {
        foreach (EnemyController enemy in enemyControllers)
        {
            Destroy(enemy.gameObject);
        }
        enemyControllers.Clear();
    }

    public void ClearAll()
    {
        currentLevel = 1;
        enemiesToSpawn = 0;

        Destroy(playerController.gameObject);
        ClearEnemies();
    }
}
