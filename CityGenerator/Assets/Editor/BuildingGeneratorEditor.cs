using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingGenerator))]
public class BuildingGeneratorEditor : Editor
{
    private BuildingGenerator buildingGenerator;
    private BuildingSettings previousSettings;
   // private BuildingRenderer previousRenderer;
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
             buildingGenerator.buildingSettings[0].numberOfBalconies)
        {
            x = buildingGenerator.buildingSettings[0].buildingSize.x,
            y = buildingGenerator.buildingSettings[0].buildingSize.y,
            buildingCount = buildingGenerator.buildingSettings[0].buildingCount,
            numberOfLevels = buildingGenerator.buildingSettings[0].numberOfLevels,
            type = buildingGenerator.buildingSettings[0].type,
            numberOfEntries = buildingGenerator.buildingSettings[0].numberOfEntries,
            numberOfBalconies = buildingGenerator.buildingSettings[0].numberOfBalconies
        };

       /* if (tempObject != null)
        {
            DestroyImmediate(tempObject);
        }

        tempObject = new GameObject("TempBuildingRenderer");

        previousRenderer = tempObject.AddComponent<BuildingRenderer>();

        if (buildingGenerator.buildingRenderer != null)
        {
            previousRenderer.floorPrefab = buildingGenerator.buildingRenderer.floorPrefab;
            previousRenderer.wallPrefab = buildingGenerator.buildingRenderer.wallPrefab;
            previousRenderer.doorPrefab = buildingGenerator.buildingRenderer.doorPrefab;
            previousRenderer.roofPrefab = buildingGenerator.buildingRenderer.roofPrefab;
            previousRenderer.stairPrefab = buildingGenerator.buildingRenderer.stairPrefab;
        }*/
    }

    private bool AreSettingsEqual(BuildingSettings settings1, BuildingSettings settings2)
    {
        return settings1.buildingSize.Equals(settings2.buildingSize)
            && settings1.buildingCount == settings2.buildingCount
            && settings1.numberOfLevels == settings2.numberOfLevels
            && settings1.type == settings2.type
            && settings1.numberOfEntries == settings2.numberOfEntries
            && settings1.numberOfBalconies == settings2.numberOfBalconies;
    }

    private bool ArePrefabsEqual(BuildingRenderer render1, BuildingRenderer render2)
    {
        return render1.stairPrefab.Equals(render2.stairPrefab)
            && render1.floorPrefab.Equals(render2.floorPrefab)
            && render1.doorPrefab.Equals(render2.doorPrefab)
            && render1.wallPrefab.Equals(render2.wallPrefab)
            && render1.roofPrefab.Equals(render2.roofPrefab);
    }

    private void GenerateNewBuilding()
    {
        if (buildingGenerator.renderedBuilding != null)
        {
            DestroyImmediate(buildingGenerator.renderedBuilding);
        }
        buildingGenerator.GenerateBuilding(Vector3.zero, Quaternion.identity, buildingGenerator.buildingSettings[0]);
    }
/*
    private void OnDisable()
    {
        // ”ничтожаем временный объект в методе OnDisable()
        if (tempObject != null)
        {
            DestroyImmediate(tempObject);
        }
    }*/
}
