using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string buildingName;
    public Transform prefab;
    public bool hasResourceGeneratorData;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minContructionRadius;
    public int healthAmountMax;
    public ResourceAmount[] constructionCostArray;


    public string GetConstructionCostString()
    {
        StringBuilder sb = new StringBuilder();
        int index = 0;
        foreach(ResourceAmount resourceAmount in constructionCostArray)
        {
            sb.Append($"<color=#{resourceAmount.resourceType.colorHex}>");
            sb.Append($"{resourceAmount.resourceType.nameShort}{resourceAmount.amount} ");
            sb.Append("</color>");

            index++;
        }

        return sb.ToString();
    }
}
