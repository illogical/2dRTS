using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager;

    private TextMeshProUGUI waveNumberText;
    private TextMeshProUGUI waveMessageText;
    private RectTransform enemyWaveSpawnPositionIndicator;
    private RectTransform enemyClosestPositionIndicator;
    private Camera mainCamera;

    private void Awake()
    {
        waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();
        enemyWaveSpawnPositionIndicator = transform.Find("enemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();
        enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, System.EventArgs e)
    {
        SetWaveNumberText($"Wave {enemyWaveManager.GetWaveNumber()}");
    }

    private void Update()
    {
        float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveTimer();
        if(nextWaveSpawnTimer <= 0 ) 
        {
            SetMessageText("");
        }
        else
        {
            SetMessageText($"Next Wave in {nextWaveSpawnTimer.ToString("F1")}s");
        }

        SetWaveSpawnIndicatorDirection();
        SetClosestEnemyIndicatorDirection(LookForClosestEnemyToCamera());

    }

    private void SetWaveSpawnIndicatorDirection()
    {
        Vector3 directionToNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - mainCamera.transform.position).normalized;
        // offset the arrow from the center of the screen in the direction that it will be pointing
        enemyWaveSpawnPositionIndicator.anchoredPosition = directionToNextSpawnPosition * 300f;
        enemyWaveSpawnPositionIndicator.eulerAngles = 
            new Vector3(0, 0, Assets.Scripts.Utilities.GlobalUtils.GetAngleFromVector(directionToNextSpawnPosition));

        // hide the indicator when the camera gets near it
        float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), mainCamera.transform.position);
        enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5f); // the multiplier is a rough estimate of the screen ratio
    }

    private void SetClosestEnemyIndicatorDirection(Enemy targetEnemy)
    {
        if(targetEnemy == null)
        {
            enemyClosestPositionIndicator.gameObject.SetActive(false);
            return;
        }

        Vector3 directionToClosestEnemyPosition = (targetEnemy.transform.position - mainCamera.transform.position).normalized;
        // offset the arrow from the center of the screen in the direction that it will be pointing
        enemyClosestPositionIndicator.anchoredPosition = directionToClosestEnemyPosition * 250f;
        enemyClosestPositionIndicator.eulerAngles =
            new Vector3(0, 0, Assets.Scripts.Utilities.GlobalUtils.GetAngleFromVector(directionToClosestEnemyPosition));

        // hide the indicator when the camera gets near it
        float distanceToClosestEnemy = Vector3.Distance(targetEnemy.transform.position, mainCamera.transform.position);
        enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > mainCamera.orthographicSize * 1.5f); // the multiplier is a rough estimate of the screen ratio
    }

    private Enemy LookForClosestEnemyToCamera()
    {
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(mainCamera.transform.position, 9999f); // look everywhere from the camera's position

        Enemy targetEnemy = null;

        foreach (var collider in colliderArray)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                // enemy is nearby
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        // found a closer building
                        targetEnemy = enemy;
                    }
                }
            }
        }

        return targetEnemy;
    }

    private void SetMessageText(string message)
    {
        waveMessageText.text = message;
    }

    private void SetWaveNumberText(string text)
    {
        waveNumberText.text = text;
    }
}
