using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private EnemyController enemyPrefab;
        [SerializeField] private PlayerController player;
        private int maxEnemiesInWave = 3;
        private int enemiesToSpawn = 0;
        private int currentLevel = 1;
        private int currentEnemies = 0;
        public bool isLevelStart { get; private set; }

        private void Start()
        {
            StartCoroutine(StartLevel(currentLevel));
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
            isLevelStart = true;
        }

        private void StartNextLevel()
        {
            isLevelStart = false;
            currentLevel++;
            enemiesToSpawn = currentLevel;
            player.ResetHealth();
            StartCoroutine(StartLevel(currentLevel));
        }

        private IEnumerator SpawnEnemies()
        {
            int enemies = 0;
            enemies = enemiesToSpawn;
            if(enemiesToSpawn > maxEnemiesInWave) { enemies = maxEnemiesInWave; }
            
            for(int i = 0; i < enemies; i++)
            {
                Transform[] wayPoints = SpawnWaypointHandler.Instance.GetWayPoints();
                EnemyController enemy = Instantiate<EnemyController>(enemyPrefab, wayPoints[Random.Range(0, wayPoints.Length)].position, Quaternion.identity);
                enemy.SetUpEnemy(player, wayPoints);
                enemiesToSpawn--;
                currentEnemies++;
                yield return null;
            }
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
