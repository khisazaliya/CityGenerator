#if UNITY_EDITOR
using Assets.Scripts.BuildingGenerator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


public class CityGenerator : MonoBehaviour
{
    public BuildingGenerator buildingGenerator;
    public List<GameObject> largePrefabs = new List<GameObject>();
    public int largePrefabsCount;
    public int streetElementsCount;
    public List<GameObject> streetPrefabs = new List<GameObject>();
    public bool withoutRoad = false;
    public bool BuildingFromList = false;
    public LODGeneratorService LODGeneratorService = new();
    [HideInInspector] public System.Random rand = new();
    public Terrain terrain;
    public List<GameObject> buildingPrefabs;
    [HideInInspector] public List<GameObject> buildings = new List<GameObject>();
    [HideInInspector] public List<GameObject> natures = new List<GameObject>();
    [HideInInspector] public List<GameObject> streetElements = new List<GameObject>();

    public Repository repository = new();
    public WaveFunctionCollapse waveFunctionCollapse = new();

    public void PlaceBuildings(List<Transform> places, List<Vector3> oldBuildingsPlaces)
    {
        float[] rotationOffset = {
        0f,
        90f,
        180f,
        270f
    };

        Vector3[] rotationOffset2 = {
        new Vector3 (-5f, 270f, 1f),
        new Vector3 (0f, 0f, 0f),
        new Vector3 (1f, 90, -5f),
        new Vector3 (-5f, 180, -5f)
    };
        if (places.Count == 0)
        {
            Debug.LogWarning("List of free places is empty");
            return;
        }
        if (!BuildingFromList)
        {
            buildingGenerator.buildingSettings = repository.LoadField();

            foreach (var buildingSetting in buildingGenerator.buildingSettings)
            {
                for (int j = 0; j < buildingSetting.buildingCount; j++)
                {
                    if (places.Count > 0)
                    {
                        int index = rand.Next(0, places.Count);
                        int rotationIndex = rand.Next(0, 4);
                        var building = buildingGenerator.GenerateBuilding(new Vector3(0, 0, 0), Quaternion.identity, buildingSetting);
                        Vector3 position;
                        if (withoutRoad)
                            position = places[index].position - new Vector3(0.5f, 1f, 0);
                        else
                            position = places[index].position - new Vector3(building.transform.localScale.x * -5f, 0f, building.transform.localScale.z * -5f);
                        building.transform.position = position;
                        var angle = places[index].rotation;


                        building.transform.Rotate(new Vector3(places[index].transform.rotation.x, 0, 0), Space.Self);



                        Vector3 spawnPosition = new Vector3(places[index].position.x, 0f, places[index].position.z);


                        RaycastHit hit;
                        if (Physics.Raycast(spawnPosition, Vector3.down, out hit))
                        {

                            building.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                        }

                        oldBuildingsPlaces.Add(position);
                        buildings.Add(building);
                        places.RemoveAt(index);
                    }
                }
            }
        }
        else
        {
            foreach (var buildingPref in buildingPrefabs)
            {
                if (places.Count > 0)
                {
                    int index = rand.Next(0, places.Count);
                    int rotationIndex = rand.Next(0, 4);
                    Vector3 position;
                    GameObject building = Instantiate(buildingPref);
                    building.name = "Building";
                    if (withoutRoad)
                        position = places[index].position - new Vector3(0.5f, 1f, 0);
                    else
                        position = places[index].position + new Vector3(0f, 3f, 0);
                    building.transform.position = position;
                    var angle = places[index].rotation;


                    building.transform.Rotate(new Vector3(places[index].transform.rotation.x, 0, 0), Space.Self);



                    Vector3 spawnPosition = new Vector3(places[index].position.x, 0f, places[index].position.z);


                    RaycastHit hit;
                    if (Physics.Raycast(spawnPosition, Vector3.down, out hit))
                    {

                        building.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    }

                    oldBuildingsPlaces.Add(position);
                    buildings.Add(building);
                    places.RemoveAt(index);
                }
            }
        }
    }

    public void PlaceNature(List<Transform> places,  List<Vector3> oldNaturesPlaces)
    {
        if (places.Count == 0)
        {
            Debug.LogWarning("List of free places is empty");
            return;
        }
        if (places.Count > 0)
        {
            for (int i = 0; i < largePrefabsCount; i++)
            {
                int natureIndex = rand.Next(0, largePrefabs.Count);
                int index = rand.Next(0, places.Count);
                GameObject nature = (GameObject)PrefabUtility.InstantiatePrefab(largePrefabs[natureIndex] as GameObject);
                natures.Add(nature);
                nature.name = "Nature";
                nature.transform.position = places[index].position + new Vector3(0, 0, 5);

                Vector3 spawnPosition = new Vector3(places[index].position.x, 0f, places[index].position.z);

                RaycastHit hit;
                if (Physics.Raycast(spawnPosition, Vector3.down, out hit))
                {
                    nature.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
                oldNaturesPlaces.Add(places[index].position);
                places.RemoveAt(index);
            }
        }

    }

    public void RandomizeBuildingPositions(List<Vector3> oldBuildingsPlaces)
    {
        List<Vector3> newPositions = new List<Vector3>(oldBuildingsPlaces);
        foreach (var building in buildings)
        {
            int index = UnityEngine.Random.Range(0, newPositions.Count);
            if (index < newPositions.Count)
            {
                building.transform.position = newPositions[index];
                Vector3 spawnPosition = new Vector3(newPositions[index].x, 0f, newPositions[index].z);
                RaycastHit hit;
                if (Physics.Raycast(spawnPosition, Vector3.down, out hit))
                {
                    building.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
                newPositions.RemoveAt(index);
            }
        }
    }

    public void RandomizeNaturePositions(List<Vector3> oldNaturesPlaces)
    {
        List<Vector3> newPositions = new List<Vector3>(oldNaturesPlaces);
        foreach (var nature in natures)
        {
            int index = UnityEngine.Random.Range(0, newPositions.Count);
            if (index < newPositions.Count)
            {
                nature.transform.position = newPositions[index] + new Vector3(0, 0, 5);
                newPositions.RemoveAt(index);
            }
        }
    }

    public void RandomizeStreetElementsPositions(float gridOffset, List<Vector3> oldStreetElementsPlaces, List<Transform> streetElementsPlaces)
    {
        List<Vector3> newPositions = new List<Vector3>(oldStreetElementsPlaces);
        foreach (var streetElement in streetElements)
        {
            int index = UnityEngine.Random.Range(0, newPositions.Count);
            if (index < newPositions.Count)
            {
                streetElement.transform.position = streetElementsPlaces[index].position + new Vector3(gridOffset / 2, 0, gridOffset / 2);
                newPositions.RemoveAt(index);
            }
        }
    }

    public void PlaceStreetElements(float gridOffset, List<Transform> streetElementsPlaces, List<Vector3> oldStreetElementsPlaces)
    {
        if (streetElementsPlaces.Count == 0)
        {
            Debug.LogWarning("List of free places is empty");
            return;
        }
        if (streetElementsPlaces.Count > 0)
        {
            for (int i = 0; i < streetElementsCount; i++)
            {
                int streetElementIndex = rand.Next(0, streetPrefabs.Count);
                int index = rand.Next(0, streetElementsPlaces.Count);
                GameObject streetElement = (GameObject)PrefabUtility.InstantiatePrefab(streetPrefabs[streetElementIndex] as GameObject);
                streetElements.Add(streetElement);
                streetElement.name = "StreetElement";
                streetElement.transform.position = streetElementsPlaces[index].position + new Vector3(gridOffset / 2, 0, gridOffset / 2);
                oldStreetElementsPlaces.Add(streetElement.transform.position);
                streetElementsPlaces.RemoveAt(index);
            }
        }

    }
    public void DestroyBuildings(List<Vector3> oldBuildingsPlaces)
    {
        buildings.Clear();
        oldBuildingsPlaces.Clear();
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        string objectName = "Building";
        foreach (GameObject obj in objects)
        {
            if (obj.name == objectName)
            {
                DestroyImmediate(obj);
            }
        }
    }

    public void DestroyNature()
    {
        natures.Clear();
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        string objectName = "Nature";
        foreach (GameObject obj in objects)
        {
            if (obj.name == objectName)
            {
                DestroyImmediate(obj);
            }
        }
    }



    public void DestroyStreetElements()
    {
        natures.Clear();
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        string objectName = "StreetElement";
        foreach (GameObject obj in objects)
        {
            if (obj.name == objectName)
            {
                DestroyImmediate(obj);
            }
        }
    }

    public void ClearAll()
    {
        buildings.Clear();
        natures.Clear();
        streetElements.Clear();
    }



}

#endif