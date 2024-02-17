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
    public void PlaceStructuresAroundRoad(List<Vector3Int> roadPositions, BuildingGenerator buildingGenerator)
    {
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);

        for (int i = 0; i < buildingGenerator.buildingSettings.Length; i++)
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
                    MeshRenderer buildingRenderer = building.GetComponentInChildren<MeshRenderer>();
                    GameObject combinedMesh = GameObject.Find("CombinedMesh");
                    Vector3 combinedMeshSize = new();
                    if (combinedMesh != null)
                    {
                        MeshRenderer renderer = combinedMesh.GetComponent<MeshRenderer>();
                        combinedMeshSize = renderer.bounds.size;

                    }
                    else
                    {
                        Debug.LogError("CombinedMesh object not found!");
                    }
                    var collider = combinedMesh.AddComponent<BoxCollider>();
                    collider.size = combinedMeshSize;

                    intersects = false; 

                    foreach (var existingBuilding in structuresDictionary.Values)
                    {
                        Collider existingCollider = existingBuilding.GetComponent<BoxCollider>();
                        if (existingCollider != null && collider != null)
                        {
                            if (existingCollider.bounds.Intersects(collider.bounds))
                            {
                                intersects = true;
                                Debug.Log("intersects");
                                Destroy(building);
                                break;
                            }
                        }
                    }

                    if (!intersects)
                    {
                        freeEstateSpots.Remove(position);
                        structuresDictionary.Add(position, building);
                    }

                    attempts++; 
                }
            }
        }
    }


    private Dictionary<Vector3Int, Direction> FindFreeSpacesAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        foreach (var position in roadPositions)
        {
            var neighbourDirections = PlacementHelper.FindNeighbour(position, roadPositions);
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
