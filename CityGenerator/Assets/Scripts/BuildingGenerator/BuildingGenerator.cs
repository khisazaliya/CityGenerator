using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public BuildingSettings[] buildingSettings;
    GameObject renderedBuilding;

    public GameObject GenerateBuilding(Vector3 position, Quaternion rotation, BuildingSettings buildingSettings)
    {
        Building b = BuildingBuilder.Build(buildingSettings);
        BuildingRenderer buildingRenderer = GetComponent<BuildingRenderer>();
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

        // ������������� JSON-������ � ������ �������� BuildingSettings
        buildingSettings = JsonHelper.FromJson<BuildingSettings>(jsonText);

        // ���������, ��� ������ �� ������
        if (buildingSettings != null)
        {
            Debug.Log("Loaded " + buildingSettings.Length + " BuildingSettings objects from the file.");
        }
        else
        {
            Debug.LogError("Failed to load BuildingSettings from JSON.");
        }
    }

    [ContextMenu("Save")]
    public void SaveField()
    {
        // ��������� ������ ���������� BuildingSettings � ���� settings.json
        File.WriteAllText(Application.streamingAssetsPath + "/settings.json", JsonUtility.ToJson(buildingSettings));
    }
}
