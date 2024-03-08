using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public List<BuildingSettings> buildingSettings;
    public List<PrefabIndexes> prefabIndexesList; // Список для хранения индексов префабов
    private int currentBuildingIndex = 0;
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

        // Добавляем текущие настройки в список всех настроек
        allSettings.AddRange(buildingSettings);

        // Создаем новый объект-обертку для списка всех настроек
        BuildingSettingsWrapper newWrapper = new BuildingSettingsWrapper
        {
            buildingSettings = allSettings
        };

        // Преобразуем объект-обертку в JSON и сохраняем его в файл
        string json = JsonUtility.ToJson(newWrapper);
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
