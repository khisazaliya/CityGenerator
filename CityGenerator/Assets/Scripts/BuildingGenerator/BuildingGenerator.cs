using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    [ContextMenu("Save")]
    public void SaveField()
    {
        string filePath = Application.streamingAssetsPath + "/settings.json";

        // Создаем список для хранения всех сохраненных данных о зданиях
        List<BuildingSettings> allSettings = new List<BuildingSettings>();

        // Проверяем, существует ли файл
        if (File.Exists(filePath))
        {
            // Если файл существует, сначала считываем уже сохраненные данные
            string jsonText = File.ReadAllText(filePath);
            var wrapper = JsonUtility.FromJson<BuildingSettingsWrapper>(jsonText);
            allSettings.AddRange(wrapper.buildingSettings);
        }

        // Добавляем новые данные о здании в список
        allSettings.AddRange(buildingSettings);

        // Обновляем данные в обертке
        var newWrapper = new BuildingSettingsWrapper();
        newWrapper.buildingSettings = allSettings.ToArray();

        // Сохраняем данные в файле
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

    [System.Serializable]
    private class BuildingSettingsWrapper
    {
        public BuildingSettings[] buildingSettings;
    }
}
