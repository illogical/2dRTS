using Assets.Scripts.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave
    }

    public event EventHandler OnWaveNumberChanged;

    [SerializeField] private float waveTimerMax = 10f;
    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float waveSpreadDistanceMax = 10f;
    [SerializeField] private int initialSpawnCount = 5;
    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextSpawnPositionTransform;

    private State state;
    private int waveNumber;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;

    private FunctionTimer waitingToSpawnNextWaveTimer;
    private FunctionTimer spawnTimer;

    private void Start()
    {
        state = State.WaitingToSpawnNextWave;
        spawnPosition = GetRandomSpawnPosition();
        nextSpawnPositionTransform.position = spawnPosition;

        waitingToSpawnNextWaveTimer = FunctionTimer.Create(WaitingToSpawnNextWave, 3f); // start the first wave after 3 seconds
        spawnTimer = FunctionTimer.CreateRandom(SpawningWave, 0, timeBetweenSpawns); // spreads out the timing of the spawns
    }


    private void WaitingToSpawnNextWave()
    {
        if (state != State.WaitingToSpawnNextWave)
        {
            return;
        }

        SpawnWave();
        state = State.SpawningWave;
    }

    private void SpawningWave()
    {
        if(state != State.SpawningWave)
        {
            return;
        }

        if (remainingEnemySpawnAmount > 0f)
        { 
            Enemy.Create(spawnPosition + GlobalUtils.GetRandomDirection() * UnityEngine.Random.Range(0, waveSpreadDistanceMax));
            remainingEnemySpawnAmount--;

            if (remainingEnemySpawnAmount <= 0)
            {
                spawnPosition = GetRandomSpawnPosition();
                nextSpawnPositionTransform.position = spawnPosition;
                waitingToSpawnNextWaveTimer.SetTimer(waveTimerMax);
                state = State.WaitingToSpawnNextWave;
            }
        }
    }

    private void SpawnWave()
    {
        remainingEnemySpawnAmount = initialSpawnCount + 3 * waveNumber;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    private Vector3 GetRandomSpawnPosition() => spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
    public int GetWaveNumber() => waveNumber;
    public float GetNextWaveTimer() => waitingToSpawnNextWaveTimer.GetTimer();
    public Vector3 GetSpawnPosition() => spawnPosition;
}
