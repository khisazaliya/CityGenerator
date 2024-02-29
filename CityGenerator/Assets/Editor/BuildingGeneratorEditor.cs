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
             buildingGenerator.buildingSettings[0].MaxNumberOfSouthBalconies,
             buildingGenerator.buildingSettings[0].randomSeedOfSouthBalconies,
             buildingGenerator.buildingSettings[0].minOffsetNorthWall,
             buildingGenerator.buildingSettings[0].maxOffsetNorthWall,
             buildingGenerator.buildingSettings[0].depthOffsetNorthWall,
             buildingGenerator.buildingSettings[0].northWallHeight,
             buildingGenerator.buildingSettings[0].minOffsetSouthWall,
             buildingGenerator.buildingSettings[0].maxOffsetSouthWall,
             buildingGenerator.buildingSettings[0].depthOffsetSouthWall,
             buildingGenerator.buildingSettings[0].southWallHeight)
        {
            x = buildingGenerator.buildingSettings[0].buildingSize.x,
            y = buildingGenerator.buildingSettings[0].buildingSize.y,
            buildingCount = buildingGenerator.buildingSettings[0].buildingCount,
            numberOfLevels = buildingGenerator.buildingSettings[0].numberOfLevels,
            type = buildingGenerator.buildingSettings[0].type,
            numberOfEntries = buildingGenerator.buildingSettings[0].numberOfEntries,
            MaxNumberOfSouthBalconies = buildingGenerator.buildingSettings[0].MaxNumberOfSouthBalconies,
            randomSeedOfSouthBalconies = buildingGenerator.buildingSettings[0].randomSeedOfSouthBalconies,
            minOffsetNorthWall = buildingGenerator.buildingSettings[0].minOffsetNorthWall,
            maxOffsetNorthWall = buildingGenerator.buildingSettings[0].maxOffsetNorthWall,
            depthOffsetNorthWall = buildingGenerator.buildingSettings[0].depthOffsetNorthWall,
            northWallHeight = buildingGenerator.buildingSettings[0].northWallHeight,
            minOffsetSouthWall = buildingGenerator.buildingSettings[0].minOffsetSouthWall,
            maxOffsetSouthWall = buildingGenerator.buildingSettings[0].maxOffsetSouthWall,
            depthOffsetSouthWall = buildingGenerator.buildingSettings[0].depthOffsetSouthWall,
            southWallHeight = buildingGenerator.buildingSettings[0].southWallHeight

        };

        EditorGUILayout.LabelField("Prefab Selection", EditorStyles.boldLabel);

        buildingRenderer.floorSouthPrefabIndex = EditorGUILayout.Popup("South Floor Prefab", buildingRenderer.floorSouthPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingRenderer.wallSouthPrefabIndex = EditorGUILayout.Popup("South Wall Prefab", buildingRenderer.wallSouthPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingRenderer.doorSouthPrefabIndex = EditorGUILayout.Popup("South Door Prefab", buildingRenderer.doorSouthPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingRenderer.roofSouthPrefabIndex = EditorGUILayout.Popup("South Roof Prefab", buildingRenderer.roofSouthPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingRenderer.stairSouthPrefabIndex = EditorGUILayout.Popup("South Stair Prefab", buildingRenderer.stairSouthPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingRenderer.balconySouthPrefabIndex = EditorGUILayout.Popup("South Balcony Prefab", buildingRenderer.balconySouthPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingRenderer.floorNorthPrefabIndex = EditorGUILayout.Popup("North Floor Prefab", buildingRenderer.floorNorthPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingRenderer.wallNorthPrefabIndex = EditorGUILayout.Popup("North Wall Prefab", buildingRenderer.wallNorthPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingRenderer.doorNorthPrefabIndex = EditorGUILayout.Popup("North Door Prefab", buildingRenderer.doorNorthPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingRenderer.roofNorthPrefabIndex = EditorGUILayout.Popup("North Roof Prefab", buildingRenderer.roofNorthPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingRenderer.stairNorthPrefabIndex = EditorGUILayout.Popup("North Stair Prefab", buildingRenderer.stairNorthPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingRenderer.balconyNorthPrefabIndex = EditorGUILayout.Popup("North Balcony Prefab", buildingRenderer.balconyNorthPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingRenderer.floorWestPrefabIndex = EditorGUILayout.Popup("West Floor Prefab", buildingRenderer.floorWestPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingRenderer.wallWestPrefabIndex = EditorGUILayout.Popup("West Wall Prefab", buildingRenderer.wallWestPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingRenderer.doorWestPrefabIndex = EditorGUILayout.Popup("West Door Prefab", buildingRenderer.doorWestPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingRenderer.roofWestPrefabIndex = EditorGUILayout.Popup("West Roof Prefab", buildingRenderer.roofWestPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingRenderer.stairWestPrefabIndex = EditorGUILayout.Popup("West Stair Prefab", buildingRenderer.stairWestPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingRenderer.balconyWestPrefabIndex = EditorGUILayout.Popup("West Balcony Prefab", buildingRenderer.balconyWestPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingRenderer.floorEastPrefabIndex = EditorGUILayout.Popup("East Floor Prefab", buildingRenderer.floorEastPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingRenderer.wallEastPrefabIndex = EditorGUILayout.Popup("East Wall Prefab", buildingRenderer.wallEastPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingRenderer.doorEastPrefabIndex = EditorGUILayout.Popup("East Door Prefab", buildingRenderer.doorEastPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingRenderer.roofEastPrefabIndex = EditorGUILayout.Popup("East Roof Prefab", buildingRenderer.roofEastPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingRenderer.stairEastPrefabIndex = EditorGUILayout.Popup("East Stair Prefab", buildingRenderer.stairEastPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingRenderer.balconyEastPrefabIndex = EditorGUILayout.Popup("East Balcony Prefab", buildingRenderer.balconyEastPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingRenderer.floorSouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Floor Prefab", buildingRenderer.floorSouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingRenderer.wallSouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Wall Prefab", buildingRenderer.wallSouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingRenderer.doorSouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Door Prefab", buildingRenderer.doorSouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingRenderer.roofSouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Roof Prefab", buildingRenderer.roofSouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingRenderer.stairSouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Stair Prefab", buildingRenderer.stairSouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingRenderer.balconySouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Balcony Prefab", buildingRenderer.balconySouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingRenderer.floorNorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Floor Prefab", buildingRenderer.floorNorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingRenderer.wallNorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Wall Prefab", buildingRenderer.wallNorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingRenderer.doorNorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Door Prefab", buildingRenderer.doorNorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingRenderer.roofNorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Roof Prefab", buildingRenderer.roofNorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingRenderer.stairNorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Stair Prefab", buildingRenderer.stairNorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingRenderer.balconyNorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Balcony Prefab", buildingRenderer.balconyNorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingRenderer.floorEastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Floor Prefab", buildingRenderer.floorEastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingRenderer.wallEastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Wall Prefab", buildingRenderer.wallEastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingRenderer.doorEastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Door Prefab", buildingRenderer.doorEastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingRenderer.roofEastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Roof Prefab", buildingRenderer.roofEastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingRenderer.stairEastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Stair Prefab", buildingRenderer.stairEastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingRenderer.balconyEastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Balcony Prefab", buildingRenderer.balconyEastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));
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
            && settings1.MaxNumberOfSouthBalconies == settings2.MaxNumberOfSouthBalconies
            && settings1.randomSeedOfSouthBalconies == settings2.randomSeedOfSouthBalconies;
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
