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
        // Загружаем данные из файла settings.json
        string jsonText = File.ReadAllText(Application.streamingAssetsPath + "/settings.json");

        // Создаем обертку для массива
        var wrapper = JsonUtility.FromJson<BuildingSettingsWrapper>(jsonText);

        // Присваиваем массив из обертки переменной buildingSettings
        buildingSettings = wrapper.buildingSettings;
    }

    // Обертка для массива объектов BuildingSettings
    [System.Serializable]
    private class BuildingSettingsWrapper
    {
        public BuildingSettings[] buildingSettings;
    }

    [ContextMenu("Save")]
    public void SaveField()
    {
        // Создаем список для хранения строк JSON каждого объекта BuildingSettings
        List<string> jsonList = new List<string>();

        // Преобразуем каждый объект BuildingSettings в строку JSON и добавляем в список
        foreach (BuildingSettings settings in buildingSettings)
        {
            string json = JsonUtility.ToJson(settings);
            jsonList.Add(json);
        }

        // Объединяем все строки JSON в одну, разделяя их символом новой строки
        string combinedJson = string.Join("\n", jsonList.ToArray());

        // Сохраняем объединенную строку JSON в файл settings.json
        File.WriteAllText(Application.streamingAssetsPath + "/settings.json", combinedJson);
    }
}
