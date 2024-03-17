using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(WaveFunctionCollapse))]
public class WFCEditor : Editor
{

    public override void OnInspectorGUI()
    {
        WaveFunctionCollapse waveFunctionCollapse = (WaveFunctionCollapse)target;
        base.OnInspectorGUI();

      //  BuildingGenerator buildingGenerator = new BuildingGenerator(); // Создаем экземпляр BuildingGenerator здесь

        if (GUILayout.Button("Generate road"))
        {
            waveFunctionCollapse.InitializeWaveFunction();
        }
        if (GUILayout.Button("Destroy roads"))
        {
            waveFunctionCollapse.ClearAll();
        }
        if (GUILayout.Button("Select File for loading buildings"))
        {
            string loadFilePath = EditorUtility.OpenFilePanel("Select File", "", "");
            waveFunctionCollapse.buildingGenerator.SetLoadFilePath(loadFilePath);
        }
        if (GUILayout.Button("Place buildings"))
        {
            waveFunctionCollapse.PlaceBuildings();
        }
        if (GUILayout.Button("Randomize buildings positions"))
        {
            waveFunctionCollapse.RandomizeBuildingPositions();
        }
        if (GUILayout.Button("Destroy buildings"))
        {
            waveFunctionCollapse.DestroyBuildings();
        }
    }
}
