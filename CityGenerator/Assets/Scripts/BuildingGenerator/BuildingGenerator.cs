using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public List<BuildingSettings> buildingSettings;
    public List<PrefabIndexes> prefabIndexesList; 
    public GameObject renderedBuilding;
    public BuildingRenderer buildingRenderer;

    private void Awake()
    {
        buildingSettings = new List<BuildingSettings>();
        prefabIndexesList = new List<PrefabIndexes>();
    }

    public GameObject GenerateBuilding(Vector3 position, Quaternion rotation, BuildingSettings buildingSettings)
    {
        Building b = BuildingData.Build(buildingSettings);

        buildingRenderer = GetComponent<BuildingRenderer>();
        renderedBuilding = buildingRenderer.Render(b);
        renderedBuilding.transform.SetPositionAndRotation(position, rotation);
        return renderedBuilding;
    }

    [ContextMenu("Load")]
    public void LoadField()
    {
        string filePath = Application.streamingAssetsPath + "/settings.json";
        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            var wrapper = JsonUtility.FromJson<BuildingSettingsWrapper>(jsonText);
            buildingSettings = wrapper.buildingSettings;
        }
    }

    [ContextMenu("Save")]
    public void SaveField()
    {
        string filePath = Application.streamingAssetsPath + "/settings.json";
        List<BuildingSettings> allSettings = new List<BuildingSettings>();
        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            var wrapper = JsonUtility.FromJson<BuildingSettingsWrapper>(jsonText);
            if (wrapper != null && wrapper.buildingSettings != null)
                allSettings.AddRange(wrapper.buildingSettings);
        }

        allSettings.AddRange(buildingSettings);
        BuildingSettingsWrapper newWrapper = new BuildingSettingsWrapper
        {
            buildingSettings = allSettings
        };
        string json = JsonUtility.ToJson(newWrapper, true);
        File.WriteAllText(filePath, json);
    }


    [ContextMenu("Clear")]
    private void ClearSettingsFile()
    {
        string filePath = Application.streamingAssetsPath + "/settings.json";
        if (File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
            Debug.Log("Settings file cleared successfully.");
        }
        else
        {
            Debug.LogWarning("Settings file not found.");
        }
    }

    private class BuildingSettingsWrapper
    {
        public List<BuildingSettings> buildingSettings;
    }
}
