using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    private float constructionTimerMax;
    private BuildingTypeSO buildingType;
    private BoxCollider2D boxCollider;

    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType)
    {
        Transform pfArrowProjectile = Resources.Load<Transform>("pfBuildingConstruction"); // pfBuildingConstruction was added to a "Resources" folder for this to work
        Transform enemyTransform = Instantiate(pfArrowProjectile, position, Quaternion.identity);

        BuildingConstruction newBuildingConstruction = enemyTransform.GetComponent<BuildingConstruction>();
        newBuildingConstruction.StartConstruction(buildingType);
        return newBuildingConstruction;
    }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    private void StartConstruction(BuildingTypeSO buildingType)
    {
        constructionTimerMax = buildingType.constructionTimerMax;
        this.buildingType = buildingType;
        boxCollider.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;

        FunctionTimer.Create(onConstructionComplete, constructionTimerMax, true);
    }

    private void onConstructionComplete()
    {
        if (buildingType == null) { return; }

        Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
