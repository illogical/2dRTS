using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    private float constructionTimerMax;
    private BuildingTypeSO buildingType;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private BuildingTypeHolder buildingTypeHolder;
    private FunctionTimer constructionTimer;

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
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    private void StartConstruction(BuildingTypeSO buildingType)
    {
        this.buildingType = buildingType;
        buildingTypeHolder.buildingType = buildingType;

        boxCollider.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;
        spriteRenderer.sprite = buildingType.sprite;

        constructionTimerMax = buildingType.constructionTimerMax;
        constructionTimer = FunctionTimer.Create(onConstructionComplete, constructionTimerMax, true);
    }

    private void onConstructionComplete()
    {
        if (buildingType == null) { return; }

        Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public float GetConstuctionTimerNormalized() => 1 - (constructionTimer?.GetTimer() / constructionTimerMax ?? 0);

}
