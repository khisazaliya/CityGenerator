using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StructureHelper : MonoBehaviour
{
    public GameObject[] naturePrefabs;
    public bool randomNaturePlacement = false;
    [Range(0, 1)]
    public float randomNaturePlacementThreshold = 0.3f;
    public Dictionary<Vector3Int, GameObject> structuresDictionary = new Dictionary<Vector3Int, GameObject>();
    public Dictionary<Vector3Int, GameObject> natureDictionary = new Dictionary<Vector3Int, GameObject>();
    public float animationTime = 0.01f;


    public System.Random rand = new System.Random();
    public void PlaceStructuresAroundRoad(List<Vector3Int> roadPositions, BuildingGenerator buildingGenerator, Dictionary<Vector3Int, GameObject> roadDictionary)
    {
        buildingGenerator.LoadField();
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);

        for (int i = 0; i < buildingGenerator.buildingSettings.Count; i++)
        {
            for (int j = 0; j < buildingGenerator.buildingSettings[i].buildingCount; j++)
            {
                bool intersects = true;
                int attempts = 0;

                while (intersects)
                {
                    if (attempts >= freeEstateSpots.Count)
                    {
                        Debug.LogError("Невозможно разместить все здания: недостаточно свободного места.");
                        return;
                    }

                    var position = freeEstateSpots.ElementAt(UnityEngine.Random.Range(0, freeEstateSpots.Count)).Key;
                    var rotation = Quaternion.identity;

                    switch (freeEstateSpots[position])
                    {
                        case Direction.Up:
                            rotation = Quaternion.Euler(0, 90, 0);
                            break;
                        case Direction.Down:
                            rotation = Quaternion.Euler(0, -90, 0);
                            break;
                        case Direction.Right:
                            rotation = Quaternion.Euler(0, 180, 0);
                            break;
                        default:
                            break;
                    }

                    var building = buildingGenerator.GenerateBuilding(position, rotation, buildingGenerator.buildingSettings[i]);
                    //  var buildingCollider = building.GetComponentInChildren<Collider>();
                    var childObject = building.transform.GetChild(0);
                    var buildingCollider = childObject.gameObject.AddComponent<BoxCollider>();

                    intersects = false;

                    foreach (var existingBuilding in structuresDictionary.Values)
                    {
                        var existingCollider = existingBuilding.GetComponentInChildren<Collider>();
                        if (existingCollider != null && buildingCollider != null && existingBuilding != building)
                        {
                            if (existingCollider.bounds.Intersects(buildingCollider.bounds))
                            {
                                intersects = true;
                                Debug.Log("Intersects with existing building");
                                Destroy(building);
                                break;
                            }
                        }
                    }
                    foreach (var road in roadDictionary.Values)
                    {
                        var existingCollider = road.GetComponentInChildren<BoxCollider>();
                        if (existingCollider != null && buildingCollider != null)
                        {
                            if (existingCollider.bounds.Intersects(buildingCollider.bounds))
                            {
                                intersects = true;
                                Debug.Log("Intersects with road");
                                Destroy(building);
                                break;
                            }
                        }
                    }

                    if (randomNaturePlacement)
                    {
                        var random = UnityEngine.Random.value;
                        if (random < randomNaturePlacementThreshold)
                        {
                            var nature = SpawnPrefab(naturePrefabs[UnityEngine.Random.Range(0, naturePrefabs.Length)], position, rotation);
                            if (!structuresDictionary.ContainsKey(position))
                                structuresDictionary.Add(position, nature);
                            break;
                        }
                    }
                    if (!intersects)
                    {
                        freeEstateSpots.Remove(position);
                        if (!structuresDictionary.ContainsKey(position))
                            structuresDictionary.Add(position, building);
                    }

                    attempts++;
                }
            }
        }
    }


    private GameObject SpawnPrefab(GameObject prefab, Vector3Int position, Quaternion rotation)
    {
        var newStructure = Instantiate(prefab, position, rotation, transform);
        return newStructure;
    }
    private Dictionary<Vector3Int, Direction> FindFreeSpacesAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        foreach (var position in roadPositions)
        {
            var neighbourDirections = PlacementHelper.FindNeighbour(position, roadPositions, new Vector3Int(1, 1));
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (neighbourDirections.Contains(direction) == false)
                {
                    var newPosition = position + PlacementHelper.GetOffsetFromDirection(direction);
                    if (freeSpaces.ContainsKey(newPosition))
                    {
                        continue;
                    }
                    freeSpaces.Add(newPosition, PlacementHelper.GetReverseDirection(direction));
                }
            }
        }
        return freeSpaces;
    }

    public void Reset()
    {
        structuresDictionary.Clear();
    }
}
