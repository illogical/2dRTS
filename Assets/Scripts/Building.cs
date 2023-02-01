using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private HealthSystem healthSystem;
    private Transform buildingDemolishBtn;

    private void Awake()
    {
        buildingDemolishBtn = transform.Find("pfBuildingDemolishBtn");
        HideDemolishButton();
    }

    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

        healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void Update()
    {
        
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    private void ShowDemolishButton()
    {
        //if(buildingDemolishBtn == null)
        //{
        //    return;
        //}

        buildingDemolishBtn?.gameObject.SetActive(true);
    }
    private void HideDemolishButton()
    {
        //if (buildingDemolishBtn == null)
        //{
        //    return;
        //}

        buildingDemolishBtn?.gameObject.SetActive(false);
    }

    private void OnMouseEnter() => ShowDemolishButton();

    private void OnMouseExit() => HideDemolishButton();
}
