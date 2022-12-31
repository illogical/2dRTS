using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    private float resourceTypeOffset = -120f;
    private ResourceTypeListSO resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeDictionary;

    private void Awake()
    {
        resourceTypeDictionary = new Dictionary<ResourceTypeSO, Transform>();
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        Transform resourceTemplate = transform.Find("ResourceTemplate");
        resourceTemplate.gameObject.SetActive(false); // hide the original template

        int index = 0; 
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            resourceTransform.gameObject.SetActive(true);
            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(resourceTypeOffset * index, 0);
            resourceTypeDictionary[resourceType] = resourceTransform;

            index++;
        }
    }

    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
        UpdateResourceAmount();
    }

    // Increment totals using each resource's timer. Better performance than trying to update on every frame even though the totals could take 0.5-2 seconds to be incremented.
    private void ResourceManager_OnResourceAmountChanged(object sender, EventArgs e) => UpdateResourceAmount();

    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
            var resourceTransform = resourceTypeDictionary[resourceType];

            resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;
            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }
    }
}
