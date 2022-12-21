using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    void Start()
    {
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        activeBuildingType = buildingTypeList.list[0];
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(activeBuildingType.Prefab, GetMouseWorldPosition(), Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            activeBuildingType = buildingTypeList.list[0];
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            activeBuildingType = buildingTypeList.list[1];
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        var mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        return mouseWorldPosition;
    }
}
