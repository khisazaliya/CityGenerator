#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(BuildingGenerator))]
[CanEditMultipleObjects]
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
            //  buildingGenerator.SaveField();
            GenerateNewBuilding();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Add buildings settings to file"))
        {
            buildingGenerator.AddBuildingSettingsToFile();
        }
        if (GUILayout.Button("Save settings as a new file"))
        {
            buildingGenerator.SaveAsNewFile();
        }
        if (GUILayout.Button("Select File for loading"))
        {
            string loadFilePath = EditorUtility.OpenFilePanel("Select File", "", "");
            buildingGenerator.SetLoadFilePath(loadFilePath);
        }
        previousSettings = new BuildingSettings(buildingGenerator.buildingSettings[0].buildingSize,
            buildingGenerator.buildingSettings[0].buildingCount,
            buildingGenerator.buildingSettings[0].numberOfLevels,
            buildingGenerator.buildingSettings[0].type,
            buildingGenerator.buildingSettings[0].numberOfEntries,
            buildingGenerator.buildingSettings[0].offsetOfEntries,
             buildingGenerator.buildingSettings[0].MaxNumberOfSouthBalconies,
             buildingGenerator.buildingSettings[0].randomSeedOfSouthBalconies,
             buildingGenerator.buildingSettings[0].randomSeedOfRoofElements,
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
            offsetOfEntries = buildingGenerator.buildingSettings[0].offsetOfEntries,
            MaxNumberOfSouthBalconies = buildingGenerator.buildingSettings[0].MaxNumberOfSouthBalconies,
            randomSeedOfSouthBalconies = buildingGenerator.buildingSettings[0].randomSeedOfSouthBalconies,
            randomSeedOfRoofElements = buildingGenerator.buildingSettings[0].randomSeedOfRoofElements,
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

        buildingGenerator.buildingSettings[0].floorSouthPrefabIndex = EditorGUILayout.Popup("South Floor Prefab", buildingGenerator.buildingSettings[0].floorSouthPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingGenerator.buildingSettings[0].wall0SouthPrefabIndex = EditorGUILayout.Popup("South Wall 0 Prefab", buildingGenerator.buildingSettings[0].wall0SouthPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall1SouthPrefabIndex = EditorGUILayout.Popup("South Wall 1 Prefab", buildingGenerator.buildingSettings[0].wall1SouthPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall2SouthPrefabIndex = EditorGUILayout.Popup("South Wall 2 Prefab", buildingGenerator.buildingSettings[0].wall2SouthPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].doorSouthPrefabIndex = EditorGUILayout.Popup("South Door Prefab", buildingGenerator.buildingSettings[0].doorSouthPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingGenerator.buildingSettings[0].roofPrefabIndex = EditorGUILayout.Popup("Roof Prefab", buildingGenerator.buildingSettings[0].roofPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingGenerator.buildingSettings[0].roofBoundPrefabIndex = EditorGUILayout.Popup("Roof Bound Prefab", buildingGenerator.buildingSettings[0].roofBoundPrefabIndex, GetPrefabNames(buildingRenderer.roofBoundPrefabs));
        buildingGenerator.buildingSettings[0].roofCornerPrefabIndex = EditorGUILayout.Popup("Roof Corner Prefab", buildingGenerator.buildingSettings[0].roofCornerPrefabIndex, GetPrefabNames(buildingRenderer.roofCornerPrefabs));
        buildingGenerator.buildingSettings[0].roofElementPrefabIndex = EditorGUILayout.Popup("Roof Element Prefab", buildingGenerator.buildingSettings[0].roofElementPrefabIndex, GetPrefabNames(buildingRenderer.roofElementPrefabs));
        buildingGenerator.buildingSettings[0].stairSouthPrefabIndex = EditorGUILayout.Popup("South Stair Prefab", buildingGenerator.buildingSettings[0].stairSouthPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingGenerator.buildingSettings[0].balconySouthPrefabIndex = EditorGUILayout.Popup("South Balcony Prefab", buildingGenerator.buildingSettings[0].balconySouthPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingGenerator.buildingSettings[0].floorNorthPrefabIndex = EditorGUILayout.Popup("North Floor Prefab", buildingGenerator.buildingSettings[0].floorNorthPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingGenerator.buildingSettings[0].wall0NorthPrefabIndex = EditorGUILayout.Popup("North Wall 0 Prefab", buildingGenerator.buildingSettings[0].wall0NorthPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall1NorthPrefabIndex = EditorGUILayout.Popup("North Wall 1 Prefab", buildingGenerator.buildingSettings[0].wall1NorthPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall2NorthPrefabIndex = EditorGUILayout.Popup("North Wall 2 Prefab", buildingGenerator.buildingSettings[0].wall2NorthPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].doorNorthPrefabIndex = EditorGUILayout.Popup("North Door Prefab", buildingGenerator.buildingSettings[0].doorNorthPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingGenerator.buildingSettings[0].stairNorthPrefabIndex = EditorGUILayout.Popup("North Stair Prefab", buildingGenerator.buildingSettings[0].stairNorthPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingGenerator.buildingSettings[0].balconyNorthPrefabIndex = EditorGUILayout.Popup("North Balcony Prefab", buildingGenerator.buildingSettings[0].balconyNorthPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingGenerator.buildingSettings[0].floorWestPrefabIndex = EditorGUILayout.Popup("West Floor Prefab", buildingGenerator.buildingSettings[0].floorWestPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingGenerator.buildingSettings[0].wall0WestPrefabIndex = EditorGUILayout.Popup("West Wall 0 Prefab", buildingGenerator.buildingSettings[0].wall0WestPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall1WestPrefabIndex = EditorGUILayout.Popup("West Wall 1 Prefab", buildingGenerator.buildingSettings[0].wall1WestPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall2WestPrefabIndex = EditorGUILayout.Popup("West Wall 2 Prefab", buildingGenerator.buildingSettings[0].wall2WestPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].doorWestPrefabIndex = EditorGUILayout.Popup("West Door Prefab", buildingGenerator.buildingSettings[0].doorWestPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingGenerator.buildingSettings[0].stairWestPrefabIndex = EditorGUILayout.Popup("West Stair Prefab", buildingGenerator.buildingSettings[0].stairWestPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingGenerator.buildingSettings[0].balconyWestPrefabIndex = EditorGUILayout.Popup("West Balcony Prefab", buildingGenerator.buildingSettings[0].balconyWestPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingGenerator.buildingSettings[0].floorEastPrefabIndex = EditorGUILayout.Popup("East Floor Prefab", buildingGenerator.buildingSettings[0].floorEastPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingGenerator.buildingSettings[0].wall0EastPrefabIndex = EditorGUILayout.Popup("East Wall 0 Prefab", buildingGenerator.buildingSettings[0].wall0EastPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall1EastPrefabIndex = EditorGUILayout.Popup("East Wall 1 Prefab", buildingGenerator.buildingSettings[0].wall1EastPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall2EastPrefabIndex = EditorGUILayout.Popup("East Wall 2 Prefab", buildingGenerator.buildingSettings[0].wall2EastPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wallDoorPrefabIndex = EditorGUILayout.Popup("Wall door Prefab", buildingGenerator.buildingSettings[0].wallDoorPrefabIndex, GetPrefabNames(buildingRenderer.wallDoorsPrefabs));
        buildingGenerator.buildingSettings[0].doorEastPrefabIndex = EditorGUILayout.Popup("East Door Prefab", buildingGenerator.buildingSettings[0].doorEastPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingGenerator.buildingSettings[0].stairEastPrefabIndex = EditorGUILayout.Popup("East Stair Prefab", buildingGenerator.buildingSettings[0].stairEastPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingGenerator.buildingSettings[0].balconyEastPrefabIndex = EditorGUILayout.Popup("East Balcony Prefab", buildingGenerator.buildingSettings[0].balconyEastPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingGenerator.buildingSettings[0].floorSouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Floor Prefab", buildingGenerator.buildingSettings[0].floorSouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingGenerator.buildingSettings[0].wall0SouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Wall 0 Prefab", buildingGenerator.buildingSettings[0].wall0SouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall1SouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Wall 1 Prefab", buildingGenerator.buildingSettings[0].wall0SouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall2SouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Wall 2 Prefab", buildingGenerator.buildingSettings[0].wall0SouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].doorSouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Door Prefab", buildingGenerator.buildingSettings[0].doorSouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingGenerator.buildingSettings[0].roofSouthOffsetPrefabIndex = EditorGUILayout.Popup("Roof SouthOffset Prefab", buildingGenerator.buildingSettings[0].roofSouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingGenerator.buildingSettings[0].roofSouthOffsetBoundPrefabIndex = EditorGUILayout.Popup("Roof SouthOffset Bound Prefab", buildingGenerator.buildingSettings[0].roofSouthOffsetBoundPrefabIndex, GetPrefabNames(buildingRenderer.roofBoundPrefabs));
        buildingGenerator.buildingSettings[0].roofSouthOffsetCornerPrefabIndex = EditorGUILayout.Popup("Roof SouthOffset Corner Prefab", buildingGenerator.buildingSettings[0].roofSouthOffsetCornerPrefabIndex, GetPrefabNames(buildingRenderer.roofCornerPrefabs));
        buildingGenerator.buildingSettings[0].stairSouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Stair Prefab", buildingGenerator.buildingSettings[0].stairSouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingGenerator.buildingSettings[0].balconySouthOffsetPrefabIndex = EditorGUILayout.Popup("South Offset Balcony Prefab", buildingGenerator.buildingSettings[0].balconySouthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingGenerator.buildingSettings[0].floorNorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Floor Prefab", buildingGenerator.buildingSettings[0].floorNorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingGenerator.buildingSettings[0].wall0NorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Wall 0 Prefab", buildingGenerator.buildingSettings[0].wall0NorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall1NorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Wall 1 Prefab", buildingGenerator.buildingSettings[0].wall1NorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall2NorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Wall 2 Prefab", buildingGenerator.buildingSettings[0].wall2NorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].doorNorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Door Prefab", buildingGenerator.buildingSettings[0].doorNorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingGenerator.buildingSettings[0].roofNorthOffsetPrefabIndex = EditorGUILayout.Popup("Roof NorthOffset Prefab", buildingGenerator.buildingSettings[0].roofNorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingGenerator.buildingSettings[0].roofNorthOffsetBoundPrefabIndex = EditorGUILayout.Popup("Roof NorthOffset Bound Prefab", buildingGenerator.buildingSettings[0].roofNorthOffsetBoundPrefabIndex, GetPrefabNames(buildingRenderer.roofBoundPrefabs));
        buildingGenerator.buildingSettings[0].roofNorthOffsetCornerPrefabIndex = EditorGUILayout.Popup("Roof NorthOffset Corner Prefab", buildingGenerator.buildingSettings[0].roofNorthOffsetCornerPrefabIndex, GetPrefabNames(buildingRenderer.roofCornerPrefabs));
        buildingGenerator.buildingSettings[0].stairNorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Stair Prefab", buildingGenerator.buildingSettings[0].stairNorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingGenerator.buildingSettings[0].balconyNorthOffsetPrefabIndex = EditorGUILayout.Popup("North Offset Balcony Prefab", buildingGenerator.buildingSettings[0].balconyNorthOffsetPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));

        buildingGenerator.buildingSettings[0].floorEastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Floor Prefab", buildingGenerator.buildingSettings[0].floorEastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.floorPrefabs));
        buildingGenerator.buildingSettings[0].wall0EastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Wall 0 Prefab", buildingGenerator.buildingSettings[0].wall0EastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall1EastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Wall 1 Prefab", buildingGenerator.buildingSettings[0].wall1EastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].wall2EastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Wall 2 Prefab", buildingGenerator.buildingSettings[0].wall2EastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.wallPrefabs));
        buildingGenerator.buildingSettings[0].doorEastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Door Prefab", buildingGenerator.buildingSettings[0].doorEastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.doorPrefabs));
        buildingGenerator.buildingSettings[0].roofEastOffsetPrefabIndex = EditorGUILayout.Popup("Roof EastOffset Prefab", buildingGenerator.buildingSettings[0].roofEastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.roofPrefabs));
        buildingGenerator.buildingSettings[0].roofEastOffsetBoundPrefabIndex = EditorGUILayout.Popup("Roof EastOffset Bound Prefab", buildingGenerator.buildingSettings[0].roofEastOffsetBoundPrefabIndex, GetPrefabNames(buildingRenderer.roofBoundPrefabs));
        buildingGenerator.buildingSettings[0].roofEastOffsetCornerPrefabIndex = EditorGUILayout.Popup("Roof EastOffset Corner Prefab", buildingGenerator.buildingSettings[0].roofEastOffsetCornerPrefabIndex, GetPrefabNames(buildingRenderer.roofCornerPrefabs));
        buildingGenerator.buildingSettings[0].stairEastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Stair Prefab", buildingGenerator.buildingSettings[0].stairEastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.stairPrefabs));
        buildingGenerator.buildingSettings[0].balconyEastOffsetPrefabIndex = EditorGUILayout.Popup("East Offset Balcony Prefab", buildingGenerator.buildingSettings[0].balconyEastOffsetPrefabIndex, GetPrefabNames(buildingRenderer.balconyPrefabs));
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
#endif