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
    public int buildingCount = 0;

    public System.Random rand = new System.Random();
    public void PlaceStructuresAroundRoad(List<Vector3Int> roadPositions, BuildingDemo buildingDemo)
    {
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);

        foreach (var freeSpot in freeEstateSpots)
        {
            var rotation = Quaternion.identity;

            switch (freeSpot.Value)
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

            for (int i = 0; i < buildingDemo.settings.Length; i++)
            {
                if (buildingCount < buildingDemo.settings[i].Count)
                {
                    List<Vector3Int> tempPositionsBlocked = new List<Vector3Int>();
                    var halfSize = Mathf.CeilToInt(buildingDemo.settings[i].buildingSize.x * buildingDemo.settings[i].buildingSize.y / 3f);

                    if (VerifyIfBuildingFits(halfSize, freeEstateSpots, freeSpot, ref tempPositionsBlocked))
                    {
                        // —оздаем здание только в случае успешной проверки
                        var building = buildingDemo.GenerateBuilding(freeSpot.Key, rotation);
                        buildingCount++;
                        // ƒобавл€ем здание в словарь только если ключа нет
                        if (!structuresDictionary.ContainsKey(freeSpot.Key))
                        {
                            // ѕровер€ем, что позиции в tempPositionsBlocked не зан€ты
                            if (tempPositionsBlocked.All(pos => !structuresDictionary.ContainsKey(pos)))
                            {
                                // ƒобавл€ем здание в словарь, использу€ позиции tempPositionsBlocked
                                foreach (var pos in tempPositionsBlocked)
                                {
                                    structuresDictionary.Add(pos, building);
                                }

                                // ¬ыход из цикла, так как здание уже размещено
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private bool VerifyIfBuildingFits(int halfSize, Dictionary<Vector3Int, Direction> freeEstateSpots, KeyValuePair<Vector3Int, Direction> freeSpot, ref List<Vector3Int> tempPositionsBlocked)
    {
        Vector3Int direction = (freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up) ? Vector3Int.right : new Vector3Int(0, 0, 1);

        for (int i = -halfSize + 1; i < halfSize; i++)
        {
            var pos = freeSpot.Key + direction * i;

            // ѕровер€ем, что позици€ находитс€ в пределах словар€ freeEstateSpots и не зан€та
            if (!freeEstateSpots.ContainsKey(pos) || structuresDictionary.ContainsKey(pos))
            {
                // ќчищаем временные позиции и возвращаем false, если проверка не прошла
                tempPositionsBlocked.Clear();
                return false;
            }

            tempPositionsBlocked.Add(pos);
        }

        return true;
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
