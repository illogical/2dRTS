using Assets.Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave
    }

    [SerializeField] private Transform spawnPositionTransform;

    private State state;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;

    private void Start()
    {
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
                    nextWaveSpawnTimer = 10f;
                }
                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0f)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = Random.Range(0, 0.2f);
                        Enemy.Create(spawnPosition + GlobalUtils.GetRandomDirection() * Random.Range(0, 10f));
                        remainingEnemySpawnAmount--;

                        if(remainingEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                        }
                    }
                }
                break;
        }

       

        

    }

    private void SpawnWave()
    {
        spawnPosition = spawnPositionTransform.position;
        nextWaveSpawnTimer = 10f;
        remainingEnemySpawnAmount = 10;
        state = State.SpawningWave;
    }
}
