using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChange;

    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);     
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if(activeBuildingType != null && CanSpawnBuilding(activeBuildingType, GlobalUtils.GetMouseWorldPosition()))
            {
                Instantiate(activeBuildingType.prefab, GlobalUtils.GetMouseWorldPosition(), Quaternion.identity);
            }
        }
    }

    

    public BuildingTypeSO GetActiveBuildingType() => activeBuildingType;

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChange?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = buildingType });
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position)
    {
        BoxCollider2D collider =  buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collisions = Physics2D.OverlapBoxAll(position + (Vector3)collider.offset, collider.size, 0);

        foreach(var collision in collisions) { 
            
        }

        return collisions.Length == 0;
    }


    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }
}
