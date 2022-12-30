using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string BuildingName;
    public Transform Prefab;
    public ResourceGeneratorData resourceGeneratorData;
    
}
