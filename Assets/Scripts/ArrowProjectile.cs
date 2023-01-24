using Assets.Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float timeToDie = 2f;
    [SerializeField] private int damageAmount = 10;

    private Enemy targetEnemy;
    private Vector3 lastMoveDirection;

    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        Transform pfArrowProjectile = Resources.Load<Transform>("pfArrowProjectile"); // pfArrowProjectile was added to a "Resources" folder for this to work
        Transform enemyTransform = Instantiate(pfArrowProjectile, position, Quaternion.identity);

        ArrowProjectile newArrowProjectile = enemyTransform.GetComponent<ArrowProjectile>();
        newArrowProjectile.SetTarget(enemy);
        return newArrowProjectile;
    }

    private void Update()
    {
        Vector3 moveDirection;
        if(targetEnemy != null)
        {
            moveDirection = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDirection = moveDirection;
        }
        else
        {
            moveDirection = lastMoveDirection;
        }

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, 0, GlobalUtils.GetAngleFromVector(moveDirection));

        timeToDie -= Time.deltaTime;
        if (timeToDie <= 0f) {
            Destroy(gameObject);        
        }
    }

    private void SetTarget(Enemy targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            // hit an ememy
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);

            Destroy(gameObject);
        }
    }
}
