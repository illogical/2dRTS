using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager;

    private TextMeshProUGUI waveNumberText;
    private TextMeshProUGUI waveMessageText;

    private void Awake()
    {
        waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();
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
