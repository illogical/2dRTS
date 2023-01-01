using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Sprite arrowSprite;

    private float buildingTypeOffset = 120f;
    private Dictionary<BuildingTypeSO, Transform> btnTransformDictionary;
    private Transform arrowButtonTransform;

    private void Awake()
    {
        Transform btnTemplate = transform.Find("btnTemplate");
        btnTemplate.gameObject.SetActive(false);

        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        // create the arrow button for when no building is selected
        arrowButtonTransform = CreateButtonFromTemplate(btnTemplate, null, 0);
        var arrowButtonImage = arrowButtonTransform.Find("image");
        arrowButtonImage.GetComponent<Image>().sprite = arrowSprite;
        arrowButtonImage.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30); // make the icon smaller

        int index = 1;
        foreach(BuildingTypeSO buildingType in buildingTypeList.list)
        {
            btnTransformDictionary[buildingType] = CreateButtonFromTemplate(btnTemplate, buildingType, index);

            index++;
        }
    }

    private void Update()
    {
        UpdateActiveBuidlingType();
    }

    private Transform CreateButtonFromTemplate(Transform buttonTemplate, BuildingTypeSO buildingType, float offsetIndex)
    {
        Transform btnTransform = Instantiate(buttonTemplate, transform);
        btnTransform.gameObject.SetActive(true);

        btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(buildingTypeOffset * offsetIndex, 0);

        btnTransform.Find("image").GetComponent<Image>().sprite = buildingType?.sprite;
        btnTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.SetActiveBuildingType(buildingType);
        });

        return btnTransform;
    }

    private void UpdateActiveBuidlingType()
    {
        // deselect all
        arrowButtonTransform.Find("selected").gameObject.SetActive(false);
        foreach (BuildingTypeSO buildType in btnTransformDictionary.Keys)
        {
            Transform btnTransform = btnTransformDictionary[buildType];
            btnTransform.Find("selected").gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();

        if(activeBuildingType == null)
        {
            arrowButtonTransform.Find("selected").gameObject.SetActive(true);
            return;
        }

        btnTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
    }
}
