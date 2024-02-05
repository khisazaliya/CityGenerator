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
       // var building = buildingDemo.GenerateBuilding(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);

        for (int i = 0; i < buildingDemo.buildingSettings.Length; i++)
        {
            for (int j = 0; j < buildingDemo.buildingSettings[i].buildingCount; j++)
            {
                List<Vector3Int> tempPositionsBlocked = new List<Vector3Int>();
                var halfSize = Mathf.CeilToInt(buildingDemo.buildingSettings[i].buildingSize.x * buildingDemo.buildingSettings[i].buildingSize.y / 3f);
                {
                    // ������� ������ ������ � ������ �������� ��������
                    var position = freeEstateSpots.Last().Key;
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
                    if (tempPositionsBlocked.Contains(position)) freeEstateSpots.Remove(position);
                    var building = buildingDemo.GenerateBuilding(freeEstateSpots.Last().Key, rotation, buildingDemo.buildingSettings[i]);
                    freeEstateSpots.Remove(position);
                    // ��������� ������ � ������� ������ ���� ����� ���
                    if (!structuresDictionary.ContainsKey(position))
                    {
                        // ���������, ��� ������� � tempPositionsBlocked �� ������
                        if (tempPositionsBlocked.All(pos => !structuresDictionary.ContainsKey(pos)))
                        {
                            // ��������� ������ � �������, ��������� ������� tempPositionsBlocked
                            foreach (var pos in tempPositionsBlocked)
                            {
                                structuresDictionary.Add(pos, building);
                            }
                        }
                    }
                }
            }
        }





/*
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

            for (int i = 0; i < buildingDemo.buildingSettings.Length; i++)
            {
                for (int j = 0; j < buildingDemo.buildingSettings[i].buildingCount; j++)
                {
                    List<Vector3Int> tempPositionsBlocked = new List<Vector3Int>();
                    var halfSize = Mathf.CeilToInt(buildingDemo.buildingSettings[i].buildingSize.x * buildingDemo.buildingSettings[i].buildingSize.y / 3f);
                    {
                        // ������� ������ ������ � ������ �������� ��������
                        building = buildingDemo.GenerateBuilding(freeSpot.Key, rotation);
                        buildingCount++;
                        // ��������� ������ � ������� ������ ���� ����� ���
                        if (!structuresDictionary.ContainsKey(freeSpot.Key))
                        {
                            // ���������, ��� ������� � tempPositionsBlocked �� ������
                            if (tempPositionsBlocked.All(pos => !structuresDictionary.ContainsKey(pos)))
                            {
                                // ��������� ������ � �������, ��������� ������� tempPositionsBlocked
                                foreach (var pos in tempPositionsBlocked)
                                {
                                    structuresDictionary.Add(pos, building);
                                }

                                // ����� �� �����, ��� ��� ������ ��� ���������
                                break;
                            }
                        }
                    }
                }
            }
        }*/
    }

    private bool VerifyIfBuildingFits(int halfSize, Dictionary<Vector3Int, Direction> freeEstateSpots, KeyValuePair<Vector3Int, Direction> freeSpot, ref List<Vector3Int> tempPositionsBlocked)
    {
        Vector3Int direction = (freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up) ? Vector3Int.right : new Vector3Int(0, 0, 1);

        for (int i = -halfSize + 1; i < halfSize; i++)
        {
            var pos = freeSpot.Key + direction * i;

            // ���������, ��� ������� ��������� � �������� ������� freeEstateSpots � �� ������
            if (!freeEstateSpots.ContainsKey(pos) || structuresDictionary.ContainsKey(pos))
            {
                // ������� ��������� ������� � ���������� false, ���� �������� �� ������
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
