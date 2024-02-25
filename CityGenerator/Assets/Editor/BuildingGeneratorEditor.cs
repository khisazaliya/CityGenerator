using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(BuildingGenerator))]
public class BuildingGeneratorEditor : Editor
{
    private BuildingGenerator buildingGenerator;
    private BuildingSettings previousSettings;
    private GameObject tempObject;

    private void OnEnable()
    {
        buildingGenerator = (BuildingGenerator)target;
        previousSettings = null;
       // previousRenderer = null;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BuildingRenderer buildingRenderer = buildingGenerator.buildingRenderer;
        if (previousSettings == null || !AreSettingsEqual(buildingGenerator.buildingSettings[0], previousSettings))
        {
            GenerateNewBuilding();
            EditorUtility.SetDirty(target);
        }

        previousSettings = new BuildingSettings(buildingGenerator.buildingSettings[0].buildingSize,
            buildingGenerator.buildingSettings[0].buildingCount,
            buildingGenerator.buildingSettings[0].numberOfLevels,
            buildingGenerator.buildingSettings[0].type,
            buildingGenerator.buildingSettings[0].numberOfEntries,
             buildingGenerator.buildingSettings[0].MaxNumberOfBalconies,
             buildingGenerator.buildingSettings[0].randomSeedOfBalconies,
             buildingGenerator.buildingSettings[0].randomOffsetNorthWall)
        {
            x = buildingGenerator.buildingSettings[0].buildingSize.x,
            y = buildingGenerator.buildingSettings[0].buildingSize.y,
            buildingCount = buildingGenerator.buildingSettings[0].buildingCount,
            numberOfLevels = buildingGenerator.buildingSettings[0].numberOfLevels,
            type = buildingGenerator.buildingSettings[0].type,
            numberOfEntries = buildingGenerator.buildingSettings[0].numberOfEntries,
            MaxNumberOfBalconies = buildingGenerator.buildingSettings[0].MaxNumberOfBalconies,
            randomSeedOfBalconies = buildingGenerator.buildingSettings[0].randomSeedOfBalconies,
            randomOffsetNorthWall = buildingGenerator.buildingSettings[0].randomOffsetNorthWall

        };

        EditorGUILayout.LabelField("Prefab Selection", EditorStyles.boldLabel);

        buildingRenderer.floorPrefabIndex = EditorGUILayout.Popup("Floor Prefab", buildingRenderer.floorPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingRenderer.wallPrefabIndex = EditorGUILayout.Popup("Wall Prefab", buildingRenderer.wallPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingRenderer.doorPrefabIndex = EditorGUILayout.Popup("Door Prefab", buildingRenderer.doorPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingRenderer.roofPrefabIndex = EditorGUILayout.Popup("Roof Prefab", buildingRenderer.roofPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingRenderer.stairPrefabIndex = EditorGUILayout.Popup("Stair Prefab", buildingRenderer.stairPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingRenderer.balconyPrefabIndex = EditorGUILayout.Popup("Balcony Prefab", buildingRenderer.balconyPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));
        if (GUI.changed)
        {
            GenerateNewBuilding();
        }
    }

    private string[] GetPrefabNames(List<Transform> prefabList)
    {
        string[] prefabNames = new string[prefabList.Count];
        for (int i = 0; i < prefabList.Count; i++)
        {
            prefabNames[i] = $"{prefabList[i].name} ({i})";
        }
        return prefabNames;
    }

    private bool AreSettingsEqual(BuildingSettings settings1, BuildingSettings settings2)
    {
        return settings1.buildingSize.Equals(settings2.buildingSize)
            && settings1.buildingCount == settings2.buildingCount
            && settings1.numberOfLevels == settings2.numberOfLevels
            && settings1.type == settings2.type
            && settings1.numberOfEntries == settings2.numberOfEntries
            && settings1.MaxNumberOfBalconies == settings2.MaxNumberOfBalconies
            && settings1.randomSeedOfBalconies == settings2.randomSeedOfBalconies;
    }

    private void GenerateNewBuilding()
    {
        if (buildingGenerator.renderedBuilding != null)
        {
            DestroyImmediate(buildingGenerator.renderedBuilding);
        }
        buildingGenerator.GenerateBuilding(Vector3.zero, Quaternion.identity, buildingGenerator.buildingSettings[0]);
    }
}
