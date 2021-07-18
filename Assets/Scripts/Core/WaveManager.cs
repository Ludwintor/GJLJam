using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GJLJam
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField]
        private WaveUI waveUI;
        [SerializeField]
        private WaveData[] waveDatas;
        [SerializeField]
        private Transform[] spawnPoints;
        [SerializeField]
        private float minSpawnDelay;
        [SerializeField]
        private float maxSpawnDelay;
        [SerializeField]
        private float undissolveTime;

        private int currentWaveIndex;
        private bool allSpawned;
        private List<Enemy> aliveEnemies = new List<Enemy>();

        private void Start()
        {
            StartNextWave();
        }

        public void StartNextWave()
        {
            if (currentWaveIndex == waveDatas.Length)
                SceneManager.LoadScene(0);

            if (currentWaveIndex != 0)
                Player.Current.Stats.AddForEachMove();

            WaveData data = waveDatas[currentWaveIndex];
            waveUI.SetEnemies(data.enemies.Length);
            allSpawned = false;

            StartCoroutine(HandleSpawning(data));
            waveUI.SetWave(currentWaveIndex + 1);
            currentWaveIndex++;
        }

        private IEnumerator HandleSpawning(WaveData data)
        {
            foreach (GameObject obj in data.enemies)
            {
                float delay = Random.Range(minSpawnDelay, maxSpawnDelay);

                yield return new WaitForSeconds(delay);
                int randomSpawner = Random.Range(0, spawnPoints.Length);
                Transform spawnPoint = spawnPoints[randomSpawner];

                Enemy enemy = Instantiate(obj, spawnPoint.position, Quaternion.identity).GetComponent<Enemy>();
                enemy.Spawn(undissolveTime);
                aliveEnemies.Add(enemy);
            }

            allSpawned = true;
        }

        private void OnEnable()
        {
            Enemy.OnEnemyKilled += EnemyKilled;
        }

        private void OnDisable()
        {
            Enemy.OnEnemyKilled -= EnemyKilled;
        }

        private void EnemyKilled(Enemy enemy)
        {
            if (aliveEnemies.Contains(enemy))
                aliveEnemies.Remove(enemy);
            else
                return;

            waveUI.SetEnemies(aliveEnemies.Count);

            if (aliveEnemies.Count == 0 && allSpawned)
            {
                StartNextWave();
            }
        }
    }
}
