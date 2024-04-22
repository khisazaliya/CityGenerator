#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CityGenerator))]
public class CityGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        CityGenerator cityGenerator = (CityGenerator)target;
        base.OnInspectorGUI();

      //  BuildingGenerator buildingGenerator = new BuildingGenerator(); // Создаем экземпляр BuildingGenerator здесь

        if (GUILayout.Button("Generate road"))
        {
            cityGenerator.waveFunctionCollapse.InitializeWaveFunction();
        }
        if (GUILayout.Button("Destroy roads"))
        {
            cityGenerator.waveFunctionCollapse.ClearAll();
        }
        if (GUILayout.Button("Select File for loading buildings"))
        {
            string loadFilePath = EditorUtility.OpenFilePanel("Select File", "", "");
            cityGenerator.repository.SetLoadFilePath(loadFilePath);
        }
        if (GUILayout.Button("Place buildings"))
        {
            cityGenerator.PlaceBuildings(cityGenerator.waveFunctionCollapse.places, cityGenerator.waveFunctionCollapse.oldBuildingsPlaces);
        }
        if (GUILayout.Button("Randomize buildings positions"))
        {
            cityGenerator.RandomizeBuildingPositions(cityGenerator.waveFunctionCollapse.oldBuildingsPlaces);
        }
        if (GUILayout.Button("Destroy buildings"))
        {
            cityGenerator.DestroyBuildings(cityGenerator.waveFunctionCollapse.oldBuildingsPlaces);
        }
        if (GUILayout.Button("Place large prefabs"))
        {
            cityGenerator.PlaceNature(cityGenerator.waveFunctionCollapse.places, cityGenerator.waveFunctionCollapse.oldNaturesPlaces);
        }
        if (GUILayout.Button("Randomize large prefabs positions"))
        {
            cityGenerator.RandomizeNaturePositions(cityGenerator.waveFunctionCollapse.oldNaturesPlaces);
        }
        if (GUILayout.Button("Destroy large prefabs"))
        {
            cityGenerator.DestroyNature();
        }
        if (GUILayout.Button("Place street elements"))
        {
            cityGenerator.PlaceStreetElements(cityGenerator.waveFunctionCollapse.gridOffset, cityGenerator.waveFunctionCollapse.streetElementsPlaces, cityGenerator.waveFunctionCollapse.oldStreetElementsPlaces);
        }
        if (GUILayout.Button("Randomize street elements positions"))
        {
            cityGenerator.RandomizeStreetElementsPositions(cityGenerator.waveFunctionCollapse.gridOffset, cityGenerator.waveFunctionCollapse.oldStreetElementsPlaces, cityGenerator.waveFunctionCollapse.streetElementsPlaces);
        }
        if (GUILayout.Button("Destroy street elements"))
        {
            cityGenerator.DestroyStreetElements();
        }
        if (GUILayout.Button("Generate LODs"))
        {
            cityGenerator.LODGeneratorService.GenerateLODs(cityGenerator.buildings);
        }
        if (GUILayout.Button("Destroy LODs"))
        {
            cityGenerator.LODGeneratorService.DestroyLODs(cityGenerator.buildings);
        }
    }
}
#endif