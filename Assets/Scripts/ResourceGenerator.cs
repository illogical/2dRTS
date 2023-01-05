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
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, resourceGeneratorData.resourceDetectionRadius);

        int nearbyResourceAmount = 0;
        foreach(Collider2D collision in collisions)
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

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

        if(nearbyResourceAmount == 0)
        {
            // No resource nodes are nearby
            // Disable resource generator
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

    public void OnDrawGizmos()
    {
        if (resourceGeneratorData == null) { return; }
        // display the distance a building needs to be from a resource to include it
        Gizmos.DrawWireSphere(transform.position, resourceGeneratorData.resourceDetectionRadius);

    }
}
