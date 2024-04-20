#if UNITY_EDITOR
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
        if (GUILayout.Button("Place large prefabs"))
        {
            waveFunctionCollapse.PlaceNature();
        }
        if (GUILayout.Button("Randomize large prefabs positions"))
        {
            waveFunctionCollapse.RandomizeNaturePositions();
        }
        if (GUILayout.Button("Destroy large prefabs"))
        {
            waveFunctionCollapse.DestroyNature();
        }
        if (GUILayout.Button("Place street elements"))
        {
            waveFunctionCollapse.PlaceStreetElements();
        }
        if (GUILayout.Button("Randomize street elements positions"))
        {
            waveFunctionCollapse.RandomizeStreetElementsPositions();
        }
        if (GUILayout.Button("Destroy street elements"))
        {
            waveFunctionCollapse.DestroyStreetElements();
        }
        if (GUILayout.Button("Generate LODs"))
        {
            waveFunctionCollapse.LODGeneratorService.GenerateLODs(waveFunctionCollapse.buildings);
        }
        if (GUILayout.Button("Destroy LODs"))
        {
            waveFunctionCollapse.LODGeneratorService.DestroyLODs(waveFunctionCollapse.buildings);
        }
    }
}
#endif