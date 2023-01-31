using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionTimerUI : MonoBehaviour
{
    [SerializeField] private BuildingConstruction buildingConstruction;

    private Image constuctionProgressImage;

    private void Awake()
    {
        constuctionProgressImage = transform.Find("mask").Find("image").GetComponent<Image>();
    }

    private void Update()
    {
        constuctionProgressImage.fillAmount = buildingConstruction.GetConstuctionTimerNormalized();
    }
}
