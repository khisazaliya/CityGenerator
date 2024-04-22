#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public List<BuildingSettings> buildingSettings;
    public GameObject renderedBuilding;
    public BuildingRenderer buildingRenderer;

  
    private void Awake()
    {
        buildingSettings = new List<BuildingSettings>();
    }

    public GameObject GenerateBuilding(Vector3 position, Quaternion rotation, BuildingSettings buildingSettings)
    {
        Building b = BuildingConstructor.Construct(buildingSettings);

        buildingRenderer = GetComponent<BuildingRenderer>();
        renderedBuilding = buildingRenderer.Render(b);
        renderedBuilding.transform.SetPositionAndRotation(position, rotation);
        return renderedBuilding;
    }
}
#endif