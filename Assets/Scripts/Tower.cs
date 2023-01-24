using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float targetMaxRadius = 20f;
    [SerializeField] private float shootTimerMax;
    private float shootTimer;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;

    private Vector3 projectileSpawnPosition;
    private Enemy targetEnemy;


    private void Awake()
    {
        projectileSpawnPosition = transform.Find("projectileSpawnPosition").position;
    }

    private void Update()
    {
        HandleTargeting();
        HandleShooting();
    }

    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;

        if(shootTimer <= 0f)
        {
            shootTimer += shootTimerMax;
            if (targetEnemy != null)
            {
                ArrowProjectile.Create(projectileSpawnPosition, targetEnemy);
            }
        }
    }

    private void HandleTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets()
    {
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

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
    }


}
