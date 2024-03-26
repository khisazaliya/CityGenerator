using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityMeshSimplifier;

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
    // ������� ������� ��� �������� LODGroup ��� ������� ������
    Dictionary<GameObject, LODGroup> buildingLODGroups = new Dictionary<GameObject, LODGroup>();

    public void PlaceStructuresAroundRoad(List<Vector3Int> roadPositions, BuildingGenerator buildingGenerator, Dictionary<Vector3Int, GameObject> roadDictionary, int roadWidth)
    {
        buildingGenerator.LoadField();
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions, roadWidth);

        HashSet<Collider> allColliders = new HashSet<Collider>();
        foreach (var building in structuresDictionary.Values)
        {
            var collider = building.GetComponentInChildren<Collider>();
            if (collider != null)
                allColliders.Add(collider);
        }
        foreach (var road in roadDictionary.Values)
        {
            var collider = road.GetComponentInChildren<BoxCollider>();
            if (collider != null)
                allColliders.Add(collider);
        }

        foreach (var buildingSetting in buildingGenerator.buildingSettings)
        {
            for (int j = 0; j < buildingSetting.buildingCount; j++)
            {
                bool intersects = true;
                int attempts = 0;

                while (intersects)
                {
                    if (attempts >= freeEstateSpots.Count)
                    {
                        Debug.LogError("���������� ���������� ��� ������: ������������ ���������� �����.");
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
                    }

                    var building = buildingGenerator.GenerateBuilding(position, rotation, buildingSetting);
                    GenerateLODs(building);
                    var childObject = building.transform.GetChild(0);
                    var buildingCollider = childObject.gameObject.AddComponent<BoxCollider>();

                    intersects = false;

                    foreach (var collider in allColliders)
                    {
                        if (collider != null && buildingCollider != null && collider.bounds.Intersects(buildingCollider.bounds))
                        {
                            intersects = true;
                            if (structuresDictionary.ContainsValue(collider.gameObject))
                                Debug.Log("Intersects with existing building");
                            else
                                Debug.Log("Intersects with road");
                            Destroy(building);
                            break;
                        }
                    }

                    if (randomNaturePlacement && !intersects)
                    {
                        var random = UnityEngine.Random.value;
                        if (random < randomNaturePlacementThreshold)
                        {
                            for (int l = 0; l < 2; l++)
                            {
                                var nature = SpawnPrefab(naturePrefabs[UnityEngine.Random.Range(0, naturePrefabs.Length)], position + new Vector3Int(10, 0, 10), rotation);
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

    private void GenerateLODs(GameObject building)
    {
        if (!buildingLODGroups.ContainsKey(building))
        {
            //GenerateLODs(building);
            LODLevel[] levels = new LODLevel[]
            {
                            new LODLevel(0.5f, 0.5f, 1f, true, true),
                            new LODLevel(0.01f, 0.5f, 0.4f, true, true)
            };

            bool autoCollectRenderers = true;
            SimplificationOptions simplificationOptions = new SimplificationOptions()
            {
                PreserveBorderEdges = true,
                PreserveUVSeamEdges = true,
                PreserveUVFoldoverEdges = false,
                PreserveSurfaceCurvature = false,
                EnableSmartLink = true,
                VertexLinkDistance = 0.00001,
                MaxIterationCount = 100,
                Agressiveness = 1.0,
                ManualUVComponentCount = false,
                UVComponentCount = 2
            };

            string saveAssetsPath = "Assets/Buildings";
            LODGroup lodGroup = LODGenerator.GenerateLODs(building, levels, autoCollectRenderers, simplificationOptions, saveAssetsPath);
            buildingLODGroups.Add(building, lodGroup);
        }
        else
        {
            // ���������� ��� ������������ LODs ��� ����� ������
            buildingLODGroups[building].gameObject.SetActive(true);
        }
      //  Destroy(GameObject.Find("Wing"));
    }

    private GameObject SpawnPrefab(GameObject prefab, Vector3Int position, Quaternion rotation)
    {
        var newStructure = Instantiate(prefab, position, rotation, transform);
        return newStructure;
    }
    private Dictionary<Vector3Int, Direction> FindFreeSpacesAroundRoad(List<Vector3Int> roadPositions, int roadWidth)
    {
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        foreach (var position in roadPositions)
        {
          //  var neighbourDirections = PlacementHelper.FindNeighbour(position, roadPositions, roadWidth);
            var neighbourDirections = PlacementHelper.FindNeighbour(position, roadPositions, roadWidth);
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (neighbourDirections.Contains(direction) == false)
                {
                    var newPosition = position + PlacementHelper.GetOffsetFromDirection(direction, roadWidth);
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
