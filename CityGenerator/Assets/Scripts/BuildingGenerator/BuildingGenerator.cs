using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public List<BuildingSettings> buildingSettings;
    public List<PrefabIndexes> prefabIndexesList; // Список для хранения индексов префабов

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
        buildingSettings = new List<BuildingSettings>();
        prefabIndexesList = new List<PrefabIndexes>();
    }
  
    [ContextMenu("Load")]
    public void LoadField()
    {
        string jsonText = File.ReadAllText(Application.streamingAssetsPath + "/settings.json");
        BuildingDataWrapper wrapper = JsonUtility.FromJson<BuildingDataWrapper>(jsonText);

        // Очищаем текущий список buildingSettings перед добавлением новых данных
        buildingSettings.Clear();

        // Добавляем данные из wrapper.buildingDataList в список buildingSettings
        foreach (BuildingFileData data in wrapper.buildingDataList)
        {
            buildingSettings.AddRange(data.buildingSettings);
            buildingRenderer.floorSouthPrefabIndex = data.prefabIndexes.floorSouthPrefabIndex;
            buildingRenderer.wall0SouthPrefabIndex = data.prefabIndexes.wall0SouthPrefabIndex;
            buildingRenderer.wall1SouthPrefabIndex = data.prefabIndexes.wall1SouthPrefabIndex;
            buildingRenderer.wall2SouthPrefabIndex = data.prefabIndexes.wall2SouthPrefabIndex;
            buildingRenderer.doorSouthPrefabIndex = data.prefabIndexes.doorSouthPrefabIndex;
            buildingRenderer.roofPrefabIndex = data.prefabIndexes.roofPrefabIndex;
            buildingRenderer.roofBoundPrefabIndex = data.prefabIndexes.roofBoundPrefabIndex;
            buildingRenderer.roofCornerPrefabIndex = data.prefabIndexes.roofCornerPrefabIndex;
            buildingRenderer.stairSouthPrefabIndex = data.prefabIndexes.stairSouthPrefabIndex;
            buildingRenderer.balconySouthPrefabIndex = data.prefabIndexes.balconySouthPrefabIndex;

            buildingRenderer.savedSeedOfSouthBalconies = data.prefabIndexes.savedSeedOfSouthBalconies;
            buildingRenderer.savedNumberOfSouthBalconies = data.prefabIndexes.savedNumberOfSouthBalconies;

            buildingRenderer.floorNorthPrefabIndex = data.prefabIndexes.floorNorthPrefabIndex;
            buildingRenderer.wall0NorthPrefabIndex = data.prefabIndexes.wall0NorthPrefabIndex;
            buildingRenderer.wall1NorthPrefabIndex = data.prefabIndexes.wall1NorthPrefabIndex;
            buildingRenderer.wall2NorthPrefabIndex = data.prefabIndexes.wall2NorthPrefabIndex;
            buildingRenderer.doorNorthPrefabIndex = data.prefabIndexes.doorNorthPrefabIndex;
            buildingRenderer.stairNorthPrefabIndex = data.prefabIndexes.stairNorthPrefabIndex;
            buildingRenderer.balconyNorthPrefabIndex = data.prefabIndexes.balconyNorthPrefabIndex;

            buildingRenderer.savedSeedOfNorthBalconies = data.prefabIndexes.savedSeedOfNorthBalconies;
            buildingRenderer.savedNumberOfNorthBalconies = data.prefabIndexes.savedNumberOfNorthBalconies;

            buildingRenderer.floorEastPrefabIndex = data.prefabIndexes.floorEastPrefabIndex;
            buildingRenderer.wall0EastPrefabIndex = data.prefabIndexes.wall0EastPrefabIndex;
            buildingRenderer.wall1EastPrefabIndex = data.prefabIndexes.wall1EastPrefabIndex;
            buildingRenderer.wall2EastPrefabIndex = data.prefabIndexes.wall2EastPrefabIndex;
            buildingRenderer.wallDoorPrefabIndex = data.prefabIndexes.wallDoorPrefabIndex;
            buildingRenderer.doorEastPrefabIndex = data.prefabIndexes.doorEastPrefabIndex;
            buildingRenderer.stairEastPrefabIndex = data.prefabIndexes.stairEastPrefabIndex;
            buildingRenderer.balconyEastPrefabIndex = data.prefabIndexes.balconyEastPrefabIndex;

            buildingRenderer.savedSeedOfEastBalconies = data.prefabIndexes.savedSeedOfEastBalconies;
            buildingRenderer.savedNumberOfEastBalconies = data.prefabIndexes.savedNumberOfEastBalconies;

            buildingRenderer.floorWestPrefabIndex = data.prefabIndexes.floorWestPrefabIndex;
            buildingRenderer.wall0WestPrefabIndex = data.prefabIndexes.wall0WestPrefabIndex;
            buildingRenderer.wall1WestPrefabIndex = data.prefabIndexes.wall1WestPrefabIndex;
            buildingRenderer.wall2WestPrefabIndex = data.prefabIndexes.wall2WestPrefabIndex;
            buildingRenderer.doorWestPrefabIndex = data.prefabIndexes.doorWestPrefabIndex;
            buildingRenderer.stairWestPrefabIndex = data.prefabIndexes.stairWestPrefabIndex;
            buildingRenderer.balconyWestPrefabIndex = data.prefabIndexes.balconyWestPrefabIndex;

            buildingRenderer.savedSeedOfWestBalconies = data.prefabIndexes.savedSeedOfWestBalconies;
            buildingRenderer.savedNumberOfWestBalconies = data.prefabIndexes.savedNumberOfWestBalconies;

            buildingRenderer.floorSouthOffsetPrefabIndex = data.prefabIndexes.floorSouthOffsetPrefabIndex;
            buildingRenderer.wall0SouthOffsetPrefabIndex = data.prefabIndexes.wall0SouthOffsetPrefabIndex;
            buildingRenderer.wall1SouthOffsetPrefabIndex = data.prefabIndexes.wall1SouthOffsetPrefabIndex;
            buildingRenderer.wall2SouthOffsetPrefabIndex = data.prefabIndexes.wall2SouthOffsetPrefabIndex;
            buildingRenderer.doorSouthOffsetPrefabIndex = data.prefabIndexes.doorSouthOffsetPrefabIndex;
            buildingRenderer.roofSouthOffsetPrefabIndex = data.prefabIndexes.roofSouthOffsetPrefabIndex;
            buildingRenderer.roofSouthOffsetBoundPrefabIndex = data.prefabIndexes.roofSouthOffsetBoundPrefabIndex;
            buildingRenderer.roofSouthOffsetCornerPrefabIndex = data.prefabIndexes.roofSouthOffsetCornerPrefabIndex;
            buildingRenderer.stairSouthOffsetPrefabIndex = data.prefabIndexes.stairSouthOffsetPrefabIndex;
            buildingRenderer.balconySouthOffsetPrefabIndex = data.prefabIndexes.balconySouthOffsetPrefabIndex;


            buildingRenderer.floorNorthOffsetPrefabIndex = data.prefabIndexes.floorNorthOffsetPrefabIndex;
            buildingRenderer.wall0NorthOffsetPrefabIndex = data.prefabIndexes.wall0NorthOffsetPrefabIndex;
            buildingRenderer.wall1NorthOffsetPrefabIndex = data.prefabIndexes.wall1NorthOffsetPrefabIndex;
            buildingRenderer.wall2NorthOffsetPrefabIndex = data.prefabIndexes.wall2NorthOffsetPrefabIndex;
            buildingRenderer.doorNorthOffsetPrefabIndex = data.prefabIndexes.doorNorthOffsetPrefabIndex;
            buildingRenderer.roofNorthOffsetPrefabIndex = data.prefabIndexes.roofNorthOffsetPrefabIndex;
            buildingRenderer.roofNorthOffsetBoundPrefabIndex = data.prefabIndexes.roofNorthOffsetBoundPrefabIndex;
            buildingRenderer.roofNorthOffsetCornerPrefabIndex = data.prefabIndexes.roofNorthOffsetCornerPrefabIndex;
            buildingRenderer.stairNorthOffsetPrefabIndex = data.prefabIndexes.stairNorthOffsetPrefabIndex;
            buildingRenderer.balconyNorthOffsetPrefabIndex = data.prefabIndexes.balconyNorthOffsetPrefabIndex;

            buildingRenderer.floorEastOffsetPrefabIndex = data.prefabIndexes.floorEastOffsetPrefabIndex;
            buildingRenderer.wall0EastOffsetPrefabIndex = data.prefabIndexes.wall0EastOffsetPrefabIndex;
            buildingRenderer.wall1EastOffsetPrefabIndex = data.prefabIndexes.wall1EastOffsetPrefabIndex;
            buildingRenderer.wall2EastOffsetPrefabIndex = data.prefabIndexes.wall2EastOffsetPrefabIndex;
            buildingRenderer.doorEastOffsetPrefabIndex = data.prefabIndexes.doorEastOffsetPrefabIndex;
            buildingRenderer.roofEastOffsetPrefabIndex = data.prefabIndexes.roofEastOffsetPrefabIndex;
            buildingRenderer.roofEastOffsetBoundPrefabIndex = data.prefabIndexes.roofEastOffsetBoundPrefabIndex;
            buildingRenderer.roofEastOffsetCornerPrefabIndex = data.prefabIndexes.roofEastOffsetCornerPrefabIndex;
            buildingRenderer.stairEastOffsetPrefabIndex = data.prefabIndexes.stairEastOffsetPrefabIndex;
            buildingRenderer.balconyEastOffsetPrefabIndex = data.prefabIndexes.balconyEastOffsetPrefabIndex;
        }
       

        for (int i = 0; i < buildingSettings.Count; i++)
        {
            if (renderedBuilding != null)
            {
                DestroyImmediate(renderedBuilding);
            }
            GenerateBuilding(Vector3.zero, Quaternion.identity, buildingSettings[i]);
        }
}

    [ContextMenu("Save")]
    public void SaveField()
    {
        string filePath = Application.streamingAssetsPath + "/settings.json";

        // Создаем новый объект BuildingDataWrapper для хранения данных
        BuildingDataWrapper wrapper = new BuildingDataWrapper();

        // Заполняем данные объекта BuildingFileData
        BuildingFileData data = new BuildingFileData
        {
            prefabIndexes = new PrefabIndexes
            {
                floorSouthPrefabIndex = buildingRenderer.floorSouthPrefabIndex,
                wall0SouthPrefabIndex = buildingRenderer.wall0SouthPrefabIndex,
                wall1SouthPrefabIndex = buildingRenderer.wall1SouthPrefabIndex,
                wall2SouthPrefabIndex = buildingRenderer.wall2SouthPrefabIndex,
                doorSouthPrefabIndex = buildingRenderer.doorSouthPrefabIndex,
                roofPrefabIndex = buildingRenderer.roofPrefabIndex,
                roofBoundPrefabIndex = buildingRenderer.roofBoundPrefabIndex,
                roofCornerPrefabIndex = buildingRenderer.roofCornerPrefabIndex,
                stairSouthPrefabIndex = buildingRenderer.stairSouthPrefabIndex,
                balconySouthPrefabIndex = buildingRenderer.balconySouthPrefabIndex,

                savedSeedOfSouthBalconies = buildingRenderer.savedSeedOfSouthBalconies,
                savedNumberOfSouthBalconies = buildingRenderer.savedNumberOfSouthBalconies,

                floorNorthPrefabIndex = buildingRenderer.floorNorthPrefabIndex,
                wall0NorthPrefabIndex = buildingRenderer.wall0NorthPrefabIndex,
                wall1NorthPrefabIndex = buildingRenderer.wall1NorthPrefabIndex,
                wall2NorthPrefabIndex = buildingRenderer.wall2NorthPrefabIndex,
                doorNorthPrefabIndex = buildingRenderer.doorNorthPrefabIndex,
                stairNorthPrefabIndex = buildingRenderer.stairNorthPrefabIndex,
                balconyNorthPrefabIndex = buildingRenderer.balconyNorthPrefabIndex,

                savedSeedOfNorthBalconies = buildingRenderer.savedSeedOfNorthBalconies,
                savedNumberOfNorthBalconies = buildingRenderer.savedNumberOfNorthBalconies,

                floorEastPrefabIndex = buildingRenderer.floorEastPrefabIndex,
                wall0EastPrefabIndex = buildingRenderer.wall0EastPrefabIndex,
                wall1EastPrefabIndex = buildingRenderer.wall1EastPrefabIndex,
                wall2EastPrefabIndex = buildingRenderer.wall2EastPrefabIndex,
                wallDoorPrefabIndex = buildingRenderer.wallDoorPrefabIndex,
                doorEastPrefabIndex = buildingRenderer.doorEastPrefabIndex,
                stairEastPrefabIndex = buildingRenderer.stairEastPrefabIndex,
                balconyEastPrefabIndex = buildingRenderer.balconyEastPrefabIndex,

                savedSeedOfEastBalconies = buildingRenderer.savedSeedOfEastBalconies,
                savedNumberOfEastBalconies = buildingRenderer.savedNumberOfEastBalconies,

                floorWestPrefabIndex = buildingRenderer.floorWestPrefabIndex,
                wall0WestPrefabIndex = buildingRenderer.wall0WestPrefabIndex,
                wall1WestPrefabIndex = buildingRenderer.wall1WestPrefabIndex,
                wall2WestPrefabIndex = buildingRenderer.wall2WestPrefabIndex,
                doorWestPrefabIndex = buildingRenderer.doorWestPrefabIndex,
                stairWestPrefabIndex = buildingRenderer.stairWestPrefabIndex,
                balconyWestPrefabIndex = buildingRenderer.balconyWestPrefabIndex,

                savedSeedOfWestBalconies = buildingRenderer.savedSeedOfWestBalconies,
                savedNumberOfWestBalconies = buildingRenderer.savedNumberOfWestBalconies,

                floorSouthOffsetPrefabIndex = buildingRenderer.floorSouthOffsetPrefabIndex,
                wall0SouthOffsetPrefabIndex = buildingRenderer.wall0SouthOffsetPrefabIndex,
                wall1SouthOffsetPrefabIndex = buildingRenderer.wall1SouthOffsetPrefabIndex,
                wall2SouthOffsetPrefabIndex = buildingRenderer.wall2SouthOffsetPrefabIndex,
                doorSouthOffsetPrefabIndex = buildingRenderer.doorSouthOffsetPrefabIndex,
                roofSouthOffsetPrefabIndex = buildingRenderer.roofSouthOffsetPrefabIndex,
                roofSouthOffsetBoundPrefabIndex = buildingRenderer.roofSouthOffsetBoundPrefabIndex,
                roofSouthOffsetCornerPrefabIndex = buildingRenderer.roofSouthOffsetCornerPrefabIndex,
                stairSouthOffsetPrefabIndex = buildingRenderer.stairSouthOffsetPrefabIndex,
                balconySouthOffsetPrefabIndex = buildingRenderer.balconySouthOffsetPrefabIndex,


                floorNorthOffsetPrefabIndex = buildingRenderer.floorNorthOffsetPrefabIndex,
                wall0NorthOffsetPrefabIndex = buildingRenderer.wall0NorthOffsetPrefabIndex,
                wall1NorthOffsetPrefabIndex = buildingRenderer.wall1NorthOffsetPrefabIndex,
                wall2NorthOffsetPrefabIndex = buildingRenderer.wall2NorthOffsetPrefabIndex,
                doorNorthOffsetPrefabIndex = buildingRenderer.doorNorthOffsetPrefabIndex,
                roofNorthOffsetPrefabIndex = buildingRenderer.roofNorthOffsetPrefabIndex,
                roofNorthOffsetBoundPrefabIndex = buildingRenderer.roofNorthOffsetBoundPrefabIndex,
                roofNorthOffsetCornerPrefabIndex = buildingRenderer.roofNorthOffsetCornerPrefabIndex,
                stairNorthOffsetPrefabIndex = buildingRenderer.stairNorthOffsetPrefabIndex,
                balconyNorthOffsetPrefabIndex = buildingRenderer.balconyNorthOffsetPrefabIndex,

                floorEastOffsetPrefabIndex = buildingRenderer.floorEastOffsetPrefabIndex,
                wall0EastOffsetPrefabIndex = buildingRenderer.wall0EastOffsetPrefabIndex,
                wall1EastOffsetPrefabIndex = buildingRenderer.wall1EastOffsetPrefabIndex,
                wall2EastOffsetPrefabIndex = buildingRenderer.wall2EastOffsetPrefabIndex,
                doorEastOffsetPrefabIndex = buildingRenderer.doorEastOffsetPrefabIndex,
                roofEastOffsetPrefabIndex = buildingRenderer.roofEastOffsetPrefabIndex,
                roofEastOffsetBoundPrefabIndex = buildingRenderer.roofEastOffsetBoundPrefabIndex,
                roofEastOffsetCornerPrefabIndex = buildingRenderer.roofEastOffsetCornerPrefabIndex,
                stairEastOffsetPrefabIndex = buildingRenderer.stairEastOffsetPrefabIndex,
                balconyEastOffsetPrefabIndex = buildingRenderer.balconyEastOffsetPrefabIndex
            },
            buildingSettings = buildingSettings
        };

        // Добавляем BuildingFileData в список
        wrapper.buildingDataList.Add(data);

        // Преобразуем объект BuildingDataWrapper в JSON и записываем в файл
        string json = JsonUtility.ToJson(wrapper, true);
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
    public class BuildingDataWrapper
    {
        public List<BuildingFileData> buildingDataList;

        public BuildingDataWrapper()
        {
            buildingDataList = new List<BuildingFileData>();
        }
    }
}