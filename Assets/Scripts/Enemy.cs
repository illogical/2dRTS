using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float targetMaxRadius = 10f;

    private Transform targetTransform;
    private Rigidbody2D rigidbody2d;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;

    public static Enemy Create(Vector3 position)
    {
        Transform pfEnemy = Resources.Load<Transform>("pfEnemy"); // pfEnemy was added to a Resources folder for this to work
        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);

        Enemy newEnemy = enemyTransform.GetComponent<Enemy>();
        return newEnemy;
    }

    private void Start()
    {
        targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        rigidbody2d = GetComponent<Rigidbody2D>();

        // randomize when each enemy will be checking for nearby buildings (so they are not all checking on the same frame)
        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargeting();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if(building != null)
        {
            // collision with a building
            building.GetComponent<HealthSystem>().Damage(10);
            Destroy(gameObject);
        }
    }

    private void LookForTargets()
    {
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (var collider in colliderArray)
        {
            Building building = collider.GetComponent<Building>();
            if(building != null)
            {
                // building is nearby
                if(targetTransform == null)
                {
                    targetTransform = building.transform;
                }
                else
                {
                    if(Vector3.Distance(transform.position, building.transform.position) <
                        Vector3.Distance(transform.position, targetTransform.position))
                    {
                        // found a closer building
                        targetTransform = building.transform;
                    }
                }
            }
        }

        if(targetTransform == null)
        {
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
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

    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector3 moveDirection = (targetTransform.position - transform.position).normalized;
            rigidbody2d.velocity = moveDirection * moveSpeed;
        }
        else
        {
            rigidbody2d.velocity = Vector2.zero;
        }
    }
}
