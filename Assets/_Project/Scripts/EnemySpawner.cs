using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private void Start()
    {
        SpawnWave();
        SpawnEnemy();
    }

    private void SpawnWave()
    {
        StartCoroutine(SpawnWaveRoutine());
    }

    private IEnumerator SpawnWaveRoutine()
    {
        int randomEnemyCount = Random.Range(2, 10);
        for (int i = 0; i < randomEnemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSecondsRealtime(.2f);
        }

        yield return new WaitForSecondsRealtime(5f);
        SpawnWave();
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(0, -0.1f, 0), Quaternion.identity);
        enemy.GetComponent<Enemy>().Init(0.5f, 200);
    }
}
