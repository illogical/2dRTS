using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceGeneratorData resourceGeneratorData;
    float timer;
    float timerMax;

    public void Awake()
    {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    private void Start()
    {
        int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);
        if (nearbyResourceAmount == 0)
        {
            // No resource nodes are nearby
            // Disable resource generator
            timerMax = 0;
            enabled = false;
        }
        else
        {
            // increase the amount of the timer as this is placed near more nodes. The smaller timerMax is, the faster resources are gathered.
            timerMax = (resourceGeneratorData.timerMax / 2f) +
                resourceGeneratorData.timerMax *
                (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);
        }
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0f)
        {
            timer += timerMax;
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
        }
    }

    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

        int nearbyResourceAmount = 0;
        foreach (Collider2D collision in collisions)
        {
            ResourceNode resourceNode = collision.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                {
                    // same type
                    nearbyResourceAmount++;
                }
            }
        }

        return Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);
    }

    public float GetTimerNormalized() => timerMax == 0 ? 0 : timer / timerMax;  // value between 0 and 1

    public ResourceGeneratorData GetResourceGeneratorData() => resourceGeneratorData;

    public float GetAmountGeneratedPerSecond() => timerMax == 0 ? 0 : 1 / timerMax;

    public void OnDrawGizmos()
    {
        if (resourceGeneratorData == null) { return; }
        // display the distance a building needs to be from a resource to include it
        Gizmos.DrawWireSphere(transform.position, resourceGeneratorData.resourceDetectionRadius);
    }
}
