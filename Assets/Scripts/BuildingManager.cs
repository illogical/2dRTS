using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private float maxConstructionRadius = 25f;
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

        Collider2D[] overlapColliders = Physics2D.OverlapBoxAll(position + (Vector3)collider.offset, collider.size, 0);

        // check that the area is clear. check that other objects are not in the position where the player is trying to build
        bool isAreaClear = overlapColliders.Length == 0;
        if (!isAreaClear) { return false; }

        // ensure the same type of building cannot be placed too close together
        overlapColliders = Physics2D.OverlapCircleAll(position, buildingType.minContructionRadius);
        foreach(var collision in overlapColliders) {
            //collisions inside of the building radius

            BuildingTypeHolder buildingTypeHolder = collision.GetComponent<BuildingTypeHolder>();
            if(buildingTypeHolder != null)
            {
                // there is a BuildingTypeHolder so this must be a building
                if(buildingTypeHolder.buildingType == buildingType)
                {
                    return false;
                }
            }
        }

        // prevent building too far apart from other buildings
        overlapColliders = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
        foreach (var collision in overlapColliders)
        {
            BuildingTypeHolder buildingTypeHolder = collision.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                // there is a BuildingTypeHolder so this must be a building
                return true;
            }
        }

        return false;
    }


    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }
}
