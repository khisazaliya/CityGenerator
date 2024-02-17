using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingGenerator))]
public class BuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BuildingGenerator buildingGenerator = (BuildingGenerator)target;
        if (GUILayout.Button("Generate building"))
        {
            if (buildingGenerator.renderedBuilding != null)
            {
                DestroyImmediate(buildingGenerator.renderedBuilding);
            }

            buildingGenerator.GenerateBuilding(new Vector3(0, 0, 0), Quaternion.identity, buildingGenerator.buildingSettings[0]);
        }
    }
}
