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
        // ��������� ������ �� ����� settings.json
        string jsonText = File.ReadAllText(Application.streamingAssetsPath + "/settings.json");

        // ������� ������� ��� �������
        var wrapper = JsonUtility.FromJson<BuildingSettingsWrapper>(jsonText);

        // ����������� ������ �� ������� ���������� buildingSettings
        buildingSettings = wrapper.buildingSettings;
    }

    // ������� ��� ������� �������� BuildingSettings
    [System.Serializable]
    private class BuildingSettingsWrapper
    {
        public BuildingSettings[] buildingSettings;
    }

    [ContextMenu("Save")]
    public void SaveField()
    {
        // ������� ������ ��� �������� ����� JSON ������� ������� BuildingSettings
        List<string> jsonList = new List<string>();

        // ����������� ������ ������ BuildingSettings � ������ JSON � ��������� � ������
        foreach (BuildingSettings settings in buildingSettings)
        {
            string json = JsonUtility.ToJson(settings);
            jsonList.Add(json);
        }

        // ���������� ��� ������ JSON � ����, �������� �� �������� ����� ������
        string combinedJson = string.Join("\n", jsonList.ToArray());

        // ��������� ������������ ������ JSON � ���� settings.json
        File.WriteAllText(Application.streamingAssetsPath + "/settings.json", combinedJson);
    }
}
