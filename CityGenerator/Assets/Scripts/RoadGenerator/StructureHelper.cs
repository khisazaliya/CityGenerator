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
    public void PlaceStructuresAroundRoad(List<Vector3Int> roadPositions, BuildingDemo buildingDemo)
    {
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);

        for (int i = 0; i < buildingDemo.buildingSettings.Length; i++)
        {
            for (int j = 0; j < buildingDemo.buildingSettings[i].buildingCount; j++)
            {
                bool intersects = true;
                int attempts = 0; // Счетчик попыток размещения здания

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

                    var building = buildingDemo.GenerateBuilding(position, rotation, buildingDemo.buildingSettings[i]);
                    var collider = building.AddComponent<BoxCollider>();
                    Vector3 buildingSize = building.transform.localScale * 20;
                    collider.size = buildingSize;

                    intersects = false; // Предполагаем, что здание не пересекается с другими

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
                        // Успешно размещено, выходим из цикла
                        freeEstateSpots.Remove(position);
                        structuresDictionary.Add(position, building);
                    }

                    attempts++; // Увеличиваем счетчик попыток
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
        /* foreach (var item in structuresDictionary.Values)
         {
             Destroy(item);
         }*/
        structuresDictionary.Clear();
    }
}
