using Assets.Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave
    }

    [SerializeField] private float waveTimerMax = 10f;
    [SerializeField] private float waveSpreadDistanceMax = 10f;
    [SerializeField] private int initialSpawnCount = 5;
    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextSpawnPositionTransform;

    private State state;
    private int waveNumber;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;

    private void Start()
    {
        state = State.WaitingToSpawnNextWave;
        spawnPosition = GetRandomSpawnPosition();
        nextSpawnPositionTransform.position = spawnPosition;
        nextWaveSpawnTimer = 3f;
    }

    private void Update()
    {
        switch(state)
        {
            case State.WaitingToSpawnNextWave:
                
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f)
                {
                    SpawnWave();
                    nextWaveSpawnTimer = waveTimerMax;
                }
                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0f)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = Random.Range(0, 0.2f);    // spreads out the timing of the spawns
                        Enemy.Create(spawnPosition + GlobalUtils.GetRandomDirection() * Random.Range(0, waveSpreadDistanceMax));
                        remainingEnemySpawnAmount--;

                        if(remainingEnemySpawnAmount <= 0)
                        {
                            spawnPosition = GetRandomSpawnPosition();
                            nextSpawnPositionTransform.position = spawnPosition;
                            state = State.WaitingToSpawnNextWave;
                        }
                    }
                }
                break;
        }
    }

    private void SpawnWave()
    {
        nextWaveSpawnTimer = 3f;
        remainingEnemySpawnAmount = initialSpawnCount + 3 * waveNumber;
        state = State.SpawningWave;
        waveNumber++;
    }

    private Vector3 GetRandomSpawnPosition() => spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
}
