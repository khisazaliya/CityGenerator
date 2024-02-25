using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingRenderer : MonoBehaviour
{
    public List<Transform> floorPrefabs;
    public List<Transform> wallPrefabs;
    public List<Transform> doorPrefabs;
    public List<Transform> roofPrefabs;
    public List<Transform> stairPrefabs;
    public List<Transform> balconyPrefabs;

    public int floorPrefabIndex = 0;
    public int wallPrefabIndex = 0;
    public int doorPrefabIndex = 0;
    public int roofPrefabIndex = 0;
    public int stairPrefabIndex = 0;
    public int balconyPrefabIndex = 0;

    public int savedSeedOfBalconies;
    public int savedNumberOfBalconies;
    public Vector3 floorSize;
    public System.Random rand = new System.Random();
    List<Tuple<int, int>> balconiesIndexes = new List<Tuple<int, int>>();
    Transform bldgFolder;
    public BuildingRenderer(List<Transform> floorPrefab, List<Transform> wallPrefab, List<Transform> doorPrefab, List<Transform> roofPrefab, List<Transform> stairPrefab)
    {
        this.floorPrefabs = floorPrefab;
        this.wallPrefabs = wallPrefab;
        this.doorPrefabs = doorPrefab;
        this.roofPrefabs = roofPrefab;
        this.stairPrefabs = stairPrefab;
    }

    public BuildingRenderer()
    {

    }
    public GameObject Render(Building bldg)
    {
        bldgFolder = new GameObject("Building").transform;
        MeshCombiner meshCombiner = bldgFolder.AddComponent<MeshCombiner>();
        foreach (Wing wing in bldg.Wings)
        {
            RenderWing(wing, bldg);
        }
        meshCombiner.CombineMeshes(bldgFolder);
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        string objectName = "Wing";
        foreach (GameObject obj in objects)
        {
            if (obj.name == objectName)
            {
               DestroyImmediate(obj);
            }
        }
        return bldgFolder.gameObject;
    }
    
    private void RenderWing(Wing wing, Building bldg)
    {
        Transform wingFolder = new GameObject("Wing").transform;
        wingFolder.SetParent(bldgFolder);
        foreach (Story story in wing.Stories)
        {
            RenderStory(story, wing, wingFolder, bldg);
        }
        RenderRoof(wing, wingFolder, bldg.level);
    }



    private void RenderStory(Story story, Wing wing, Transform wingFolder, Building bldg)
    {
        Transform storyFolder = new GameObject("Story ").transform;
        storyFolder.SetParent(wingFolder);
        List<int> entries = CalculateEntryIndex(wing, bldg.numberOfEntries);
        List<Tuple<int, int>> offsets = GenerateBuildingShape(bldg.Size.x);
        Debug.Log(offsets[0].Item1 + " " + offsets[0].Item2);
        for (int x = wing.Bounds.min.x; x < wing.Bounds.max.x; x++)
        {
            for (int y = wing.Bounds.min.y; y < wing.Bounds.max.y; y++)
            {
                for (int i = 0; i < bldg.level; i++)
                {
                    //south wall
                    if (y == wing.Bounds.min.y)
                    {
                        Transform wall = wallPrefabs[wallPrefabIndex];
                        if (i == 0) PlaceFloor(x, y-1, i, new int[3] { 0, -90, 0 }, storyFolder);
                        if (PlaceBalcony(bldg, i, x)) PlaceSouthWall(x, y, i, storyFolder, balconyPrefabs[balconyPrefabIndex]);

                        PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[wallPrefabIndex]);
                    }

                    //east wall
                    if (x == wing.Bounds.min.x + wing.Bounds.size.x - 1)
                    {
                        Transform wall = wallPrefabs[wallPrefabIndex];
                        if (i == 0)
                        {
                            if (entries.Contains(y))
                            {
                                PlaceEastWall(x, y, i, storyFolder, doorPrefabs[doorPrefabIndex]);
                                PlaceStair(x + 1, y - 1, i, storyFolder);
                            }
                            else
                            {
                                wall = wallPrefabs[wallPrefabIndex];
                                PlaceFloor(x + 1, y - 1, i, new int[3] { 0, 180, 0 }, storyFolder);
                            }
                        }
                        else 
                        {
                            if (PlaceBalcony(bldg, i, y)) PlaceEastWall(x, y, i, storyFolder, balconyPrefabs[balconyPrefabIndex]);
                            else
                            wall = wallPrefabs[wallPrefabIndex];
                        }
                        if (bldg.minOffsetNorthWall > 0 && bldg.minOffsetNorthWall < wing.Bounds.max.x &&
                            bldg.maxOffsetNorthWall > 0 && bldg.maxOffsetNorthWall < wing.Bounds.max.x)
                        {
                            if (y >= wing.Bounds.max.y - bldg.minOffsetNorthWall && y <= wing.Bounds.max.y + bldg.minOffsetNorthWall)
                                PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetNorthWall - 1), y + bldg.minOffsetNorthWall, i, storyFolder, wall);
                        }
                        PlaceEastWall(x, y, i, storyFolder, wall);
                    }

                    //north wall
                    if (y == wing.Bounds.min.y + wing.Bounds.size.y - 1)
                    {
                        Transform wall = wallPrefabs[wallPrefabIndex];
                        if (i == 0) PlaceFloor(x+1, y, i, new int[3] { 0, 90, 0 }, storyFolder);
                        if (PlaceBalcony(bldg, i, x)) PlaceNorthWall(x, y, i, storyFolder, balconyPrefabs[balconyPrefabIndex]);
                        if (bldg.minOffsetNorthWall > 0 && bldg.minOffsetNorthWall < wing.Bounds.max.x &&
                            bldg.maxOffsetNorthWall > 0 && bldg.maxOffsetNorthWall < wing.Bounds.max.x)
                        {
                            if (x >= bldg.minOffsetNorthWall && x <= bldg.maxOffsetNorthWall)
                                PlaceNorthWall(x, y + bldg.minOffsetNorthWall, i, storyFolder, wallPrefabs[wallPrefabIndex]);
                            else PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[wallPrefabIndex]);
                        }
                        else
                        PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[wallPrefabIndex]);

                    }

                    //west wall
                    if (x == wing.Bounds.min.x)
                    {
                        Transform wall = wallPrefabs[wallPrefabIndex];
                        if (i == 0) PlaceFloor(x, y, i, new int[3] { 0, 0, 0 }, storyFolder);
                        if (PlaceBalcony(bldg, i, y)) PlaceWestWall(x, y, i, storyFolder, balconyPrefabs[balconyPrefabIndex]);
                        PlaceWestWall(x, y, i, storyFolder, wallPrefabs[wallPrefabIndex]);
                        if (bldg.minOffsetNorthWall > 0 && bldg.minOffsetNorthWall < wing.Bounds.max.x &&
                             bldg.maxOffsetNorthWall > 0 && bldg.maxOffsetNorthWall < wing.Bounds.max.x)
                        {
                            if (y >= wing.Bounds.max.y - bldg.minOffsetNorthWall && y <= wing.Bounds.max.y + bldg.minOffsetNorthWall)
                                PlaceWestWall(x + bldg.minOffsetNorthWall, y + bldg.minOffsetNorthWall, i, storyFolder, wallPrefabs[wallPrefabIndex]);
                        }
                    }
                }
            }
        }
    }

    public List<Tuple<int, int>> GenerateBuildingShape(int max)
    {
        List<Tuple<int, int>> offset = new();
        offset.Add(new Tuple<int, int>(rand.Next(0, max), rand.Next(0, max)));
        var sortedTuples = offset.Select(t => Tuple.Create(Math.Min(t.Item1, t.Item2), Math.Max(t.Item1, t.Item2))).ToList();
        return sortedTuples;
    }

    public void RenderFirstLevel(Story story, Wing wing, Transform wingFolder, Building bldg)
    {
        Transform storyFolder = GameObject.Find("Story ").transform;
        GameObject objectToDelete = GameObject.Find("Entry ");
        if (objectToDelete != null)
        {
            Destroy(objectToDelete);
        }
        Transform wall;
        Transform entriesFolder = new GameObject("Entry ").transform;
        List<int> entries = CalculateEntryIndex(wing, bldg.numberOfEntries);
        for (int x = wing.Bounds.min.x; x < wing.Bounds.max.x; x++)
        {
            for (int y = wing.Bounds.min.y; y < wing.Bounds.max.y; y++)
            {
                if (entries.Contains(y))
                {
                    wall = doorPrefabs[doorPrefabIndex];
                    PlaceStair(x + 1, y - 1, 0, entriesFolder);
                }
                else
                {
                    wall = wallPrefabs[wallPrefabIndex];
                    PlaceFloor(x + 1, y - 1, 0, new int[3] { 0, 180, 0 }, entriesFolder);
                }
                PlaceEastWall(x, y, 0, storyFolder, wall);
            }
        }


    }
    public List<Tuple<int, int>> CalculateBalconiesIndex(Building bldg, int numberOfBalconies, int randomSeedOfBalconies)
    {
        if (randomSeedOfBalconies == savedSeedOfBalconies && numberOfBalconies == savedNumberOfBalconies) return balconiesIndexes;
        savedSeedOfBalconies = randomSeedOfBalconies;
        savedNumberOfBalconies = numberOfBalconies;
        balconiesIndexes.Clear();
        var balconySpacing = (int)Math.Ceiling(bldg.Size.y/ (double)numberOfBalconies);
        if (randomSeedOfBalconies == 0)
        {
            for (int i = 2; i < bldg.level; i++)
            {
                for (int j = 0; j < numberOfBalconies; j++)
                balconiesIndexes.Add(new Tuple<int, int>(i, j * balconySpacing));
            }
        }
        else
        {
            for (int i = 0; i < numberOfBalconies; i++)
            {
                balconiesIndexes.Add(new Tuple<int, int>(rand.Next(2, randomSeedOfBalconies), rand.Next(2, randomSeedOfBalconies)));
            }
        }
        return balconiesIndexes;
    }
    public List<int> CalculateEntryIndex(Wing wing, int numberOfEntries)
    {
        var doorSpacing = (int)Math.Ceiling((wing.Bounds.size.y - wing.Bounds.min.y) / ((double)numberOfEntries+1));
        List<int> entries = new();
        for (int i = 1; i <= numberOfEntries; i++)
        {
            int currentDoorPosition = wing.Bounds.min.y + doorSpacing * i - 1;
            entries.Add(currentDoorPosition);
        }
        return entries;
    }
    private void PlaceFloor(int x, int y, int level, int[] angles, Transform storyFolder)
    {
        floorSize = GetPrefabSize(floorPrefabs[floorPrefabIndex]);
        Transform f = Instantiate(floorPrefabs[floorPrefabIndex], storyFolder.TransformPoint(new Vector3(x * -3f, 0f + level * 2.5f, y * -3f -3f)), Quaternion.Euler(angles[0], angles[1], angles[2]));
        f.SetParent(storyFolder);
    }

    private bool PlaceBalcony(Building bldg, int level, int place)
    {
        List<Tuple<int, int>> balconiesIndexes = CalculateBalconiesIndex(bldg, bldg.numberOfBalconies, bldg.randomSeedOfBalconies);
        Tuple<int, int> pairToCheck = new Tuple<int, int>(2, 2);
        pairToCheck = new Tuple<int, int>(level, place);
        return balconiesIndexes.Contains(pairToCheck) ;
    }
    private void PlaceStair(int x, int y, int level, Transform storyFolder)
    {
        var stairSize = GetPrefabSize(stairPrefabs[stairPrefabIndex]);
        Transform f = Instantiate(stairPrefabs[stairPrefabIndex], storyFolder.TransformPoint(new Vector3(x * -3f, 0f + level * 2.5f, y * -3f - 3f)), Quaternion.identity);
        f.SetParent(storyFolder);
    }

    //боковая
    private void PlaceSouthWall(int x, int y, int level, Transform storyFolder, Transform wall)
    {
        float height;
        if (level == 0) height = floorSize.y;
        else height = level * 2+ floorSize.y;
        Transform w = Instantiate(
            wall,
            storyFolder.TransformPoint(
                new Vector3(
                    x * -3f,
                    height,
                    y * 3f
                    )
                ),
            Quaternion.Euler(0, 90, 0));
        w.SetParent(storyFolder);
    }

    private void PlaceEastWall(int x, int y, int level, Transform storyFolder, Transform wall)
    {

        float height;
        if (level == 0) height = floorSize.y;
        else height = level * 2 + floorSize.y;
        Transform w = Instantiate(
            wall,
            storyFolder.TransformPoint(
                new Vector3(
                    x * -3f - 3f,
                    height,
                    y * -3f
                    )
                ),
             Quaternion.identity);
        w.SetParent(storyFolder);
    }

    private void PlaceNorthWall(int x, int y, int level, Transform storyFolder, Transform wall)
    {
        float height;
        if (level == 0) height = floorSize.y;
        else height = level * 2 + floorSize.y;
        Transform w = Instantiate(
            wall,
            storyFolder.TransformPoint(
                new Vector3(
                    x * -3f - 3f,
                    height,
                    y * -3f - 3f
                    )
                ),
            Quaternion.Euler(0, -90, 0));
        w.SetParent(storyFolder);
    }

    private void PlaceWestWall(int x, int y, int level, Transform storyFolder, Transform wall)
    {
        float height;
        if (level == 0) height = floorSize.y;
        else height = level * 2  + floorSize.y;
        Transform w = Instantiate(
            wall,
            storyFolder.TransformPoint(
                new Vector3(
                    x * -3f,
                    height,
                    y * -3f -3f
                    )
                ),
            Quaternion.Euler(0, 180, 0));
        w.SetParent(storyFolder);
    }

    private void RenderRoof(Wing wing, Transform wingFolder, int level)
    {
        var direction = new RoofDirection();
        var prefabIndex = 0;
        for (int x = wing.Bounds.min.x; x < wing.Bounds.max.x; x++)
        {
            for (int y = wing.Bounds.min.y; y < wing.Bounds.max.y; y++)
            {
                if ((y == wing.Bounds.min.y) && (x == wing.Bounds.min.x))
                {
                    prefabIndex = 2;
                    direction = RoofDirection.South; 
                }
                else
                if ((y == wing.Bounds.min.y) && (x == wing.Bounds.max.x - 1))
                {
                    prefabIndex = 2;
                    direction = RoofDirection.East; 
                }
                else
                if ((y == wing.Bounds.max.y - 1) && (x == wing.Bounds.min.x))
                {
                    prefabIndex = 2;
                    direction = RoofDirection.West; 
                }
                else
                if ((y == wing.Bounds.max.y - 1) && (x == wing.Bounds.max.x - 1))
                {
                    prefabIndex = 2;
                    direction = RoofDirection.North; 
                }
                else
                if (y == wing.Bounds.min.y && !(y == wing.Bounds.max.y - 1 || x == wing.Bounds.min.x || x == wing.Bounds.max.x - 1))
                {
                    prefabIndex = 1;
                    direction = RoofDirection.East;
                }
                else
                if (y == wing.Bounds.max.y - 1 && !(y == wing.Bounds.min.y || x == wing.Bounds.min.x || x == wing.Bounds.max.x - 1))
                {
                    prefabIndex = 1;
                    direction = RoofDirection.West;
                }
                else
                if (x == wing.Bounds.min.x  && !(y == wing.Bounds.min.y || y == wing.Bounds.max.y - 1 || x == wing.Bounds.max.x - 1))
                {
                    prefabIndex = 1;
                    direction = RoofDirection.South;
                }
                else
                if (x == wing.Bounds.max.x - 1 && !(y == wing.Bounds.min.y || y == wing.Bounds.max.y - 1 || x == wing.Bounds.min.x))
                {
                    prefabIndex = 1;
                    direction = RoofDirection.North;
                }
                else
                {
                    prefabIndex = 0;
                    direction = RoofDirection.North;
                }

                PlaceRoof(x, y, level, prefabIndex, wingFolder, wing.GetRoof.Type, direction);
            }
        }
    }

    private void PlaceRoof(int x, int y, int level, int prefabIndex, Transform wingFolder, RoofType type, RoofDirection direction)
    {
        Transform r;
        r = Instantiate(
            roofPrefabs[prefabIndex],
            wingFolder.TransformPoint(
               new Vector3(
                       x * -3f + rotationOffset[(int)direction].x,
                        level * 2  + floorSize.y,
                        y * -3f + rotationOffset[(int)direction].z
                    )
                ),
            Quaternion.Euler(0f, rotationOffset[(int)direction].y, 0f)
            );
        r.SetParent(wingFolder);
    }

    Vector3[] rotationOffset = {
        new Vector3 (-3f, 270f, 0f),
        new Vector3 (0f, 0f, 0f),
        new Vector3 (0f, 90, -3f),
        new Vector3 (-3f, 180, -3f)
    };

    public Vector3 GetPrefabSize(Transform prefab)
    {
        Vector3 size = Vector3.zero;
        GameObject instance = Instantiate(prefab.gameObject, Vector3.zero, Quaternion.identity);
        var renderer = instance.GetComponentInChildren<MeshRenderer>();

        if (renderer != null)
        {
            size = renderer.bounds.size;
        }
        else
        {
            Debug.LogError("Prefab does not have a MeshRenderer component!");
        }
        DestroyImmediate(instance);

        return size;
    }

}
