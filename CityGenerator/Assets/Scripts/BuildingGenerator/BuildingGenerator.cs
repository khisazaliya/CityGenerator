using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public BuildingSettings[] buildingSettings;
    public GameObject renderedBuilding;
    public BuildingRenderer buildingRenderer;

    public GameObject GenerateBuilding(Vector3 position, Quaternion rotation, BuildingSettings buildingSettings)
    {
        Building b = BuildingData.Build(buildingSettings);

        buildingRenderer = GetComponent<BuildingRenderer>();
        renderedBuilding = buildingRenderer.Render(b);
        renderedBuilding.transform.SetPositionAndRotation(position, rotation);
        return renderedBuilding;
    }

    public BuildingGenerator()
    {
    }

    [ContextMenu("Load")]
    public void LoadField()
    {
        string jsonText = File.ReadAllText(Application.streamingAssetsPath + "/settings.json");
        var wrapper = JsonUtility.FromJson<BuildingSettingsWrapper>(jsonText);
        buildingSettings = wrapper.buildingSettings;
    }

    [System.Serializable]
    private class BuildingSettingsWrapper
    {
        public BuildingSettings[] buildingSettings;
    }

    [ContextMenu("Save")]
    public void SaveField()
    {
        var wrapper = new BuildingSettingsWrapper();
        wrapper.buildingSettings = buildingSettings;
        string json = JsonUtility.ToJson(wrapper, true); 

        File.WriteAllText(Application.streamingAssetsPath + "/settings.json", json);
    }
}
