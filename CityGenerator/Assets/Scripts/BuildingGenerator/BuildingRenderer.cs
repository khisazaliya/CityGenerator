using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingRenderer : MonoBehaviour
{
    public List<Transform> floorPrefab;
    public List<Transform> wallPrefab;
    public List<Transform> doorPrefab;
    public List<Transform> roofPrefab;
    public List<Transform> stairPrefab;
    public List<Transform> balconyPrefab;
    public Vector3 floorSize;
    public System.Random rand = new System.Random();
    Transform bldgFolder;
    public BuildingRenderer(List<Transform> floorPrefab, List<Transform> wallPrefab, List<Transform> doorPrefab, List<Transform> roofPrefab, List<Transform> stairPrefab)
    {
        this.floorPrefab = floorPrefab;
        this.wallPrefab = wallPrefab;
        this.doorPrefab = doorPrefab;
        this.roofPrefab = roofPrefab;
        this.stairPrefab = stairPrefab;
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
        List<int> balconies = CalculateBalconiesIndex(bldg.Size.y, bldg.numberOfBalconies);
        List<int> entries = CalculateEntryIndex(wing, bldg.numberOfEntries);
        for (int x = wing.Bounds.min.x; x < wing.Bounds.max.x; x++)
        {
            for (int y = wing.Bounds.min.y; y < wing.Bounds.max.y; y++)
            {
                for (int i = 0; i < bldg.level; i++)
                {

                    //south wall
                    if (y == wing.Bounds.min.y)
                    {
                        Transform wall = wallPrefab[0];
                        if (i == 0) PlaceFloor(x, y-1, i, new int[3] { 0, -90, 0 }, storyFolder);
                        if (i>0 && balconies.Contains(x))
                        {
                            PlaceSouthWall(x, y, i, storyFolder, balconyPrefab[0]);
                        }
                        PlaceSouthWall(x, y, i, storyFolder, wallPrefab[0]);
                    }

                    //east wall
                    if (x == wing.Bounds.min.x + wing.Bounds.size.x - 1)
                    {
                        Transform wall =wallPrefab[0];
                        if (i == 0)
                        {
                            if (entries.Contains(y))
                            {
                                wall = doorPrefab[0];
                                PlaceStair(x + 1, y - 1, i, storyFolder);
                            }
                            else
                            {
                                wall = wallPrefab[0];
                                PlaceFloor(x + 1, y - 1, i, new int[3] { 0, 180, 0 }, storyFolder);
                            }
                        }
                        else 
                        {
                            if (balconies.Contains(y))
                            {
                                PlaceEastWall(x, y, i, storyFolder, balconyPrefab[0]);
                            }
                            else
                            wall = wallPrefab[0];
                        }
                        PlaceEastWall(x, y, i, storyFolder, wall);
                    }

                    //north wall
                    if (y == wing.Bounds.min.y + wing.Bounds.size.y - 1)
                    {
                        Transform wall = wallPrefab[0];
                        if (i == 0) PlaceFloor(x+1, y, i, new int[3] { 0, 90, 0 }, storyFolder);
                        if (i > 0 && balconies.Contains(x))
                        {
                            PlaceNorthWall(x, y, i, storyFolder, balconyPrefab[0]);
                        }
                        PlaceNorthWall(x, y, i, storyFolder, wallPrefab[0]);
                    }

                    //west wall
                    if (x == wing.Bounds.min.x)
                    {
                        Transform wall = wallPrefab[0];
                        if (i == 0) PlaceFloor(x, y, i, new int[3] { 0, 0, 0 }, storyFolder);
                        if (i > 0 && balconies.Contains(y))
                        {
                            PlaceWestWall(x, y, i, storyFolder, balconyPrefab[0]); 
                        }
                        PlaceWestWall(x, y, i, storyFolder, wallPrefab[0]);
                    }

                }
            }
        }
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
                    wall = doorPrefab[0];
                    PlaceStair(x + 1, y - 1, 0, entriesFolder);
                }
                else
                {
                    wall = wallPrefab[0];
                    PlaceFloor(x + 1, y - 1, 0, new int[3] { 0, 180, 0 }, entriesFolder);
                }
                PlaceEastWall(x, y, 0, storyFolder, wall);
            }
        }


    }
    public List<int> CalculateBalconiesIndex(int bldgSize, int numberOfBalconies)
    {
        var balconySpacing = (int)Math.Ceiling(bldgSize / (double)numberOfBalconies);
        List<int> balconiesIndexes = new List<int>();
        for (int i = 0; i < numberOfBalconies; i++)
        {
            balconiesIndexes.Add(i* balconySpacing);
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
        floorSize = GetPrefabSize(floorPrefab[0]);
        Transform f = Instantiate(floorPrefab[0], storyFolder.TransformPoint(new Vector3(x * -3f, 0f + level * 2.5f, y * -3f -3f)), Quaternion.Euler(angles[0], angles[1], angles[2]));
        f.SetParent(storyFolder);
    }

    private void PlaceBalcony(int x, int y, int level, Transform storyFolder)
    {
        Transform b = Instantiate(balconyPrefab[0], storyFolder.TransformPoint(new Vector3(x * -3f, 0f + level * 2.5f, y * -3f - 3f)), Quaternion.identity);
        b.SetParent(storyFolder);
    }
    private void PlaceStair(int x, int y, int level, Transform storyFolder)
    {
        var stairSize = GetPrefabSize(stairPrefab[0]);
        Transform f = Instantiate(stairPrefab[0], storyFolder.TransformPoint(new Vector3(x * -3f, 0f + level * 2.5f, y * -3f - 3f)), Quaternion.identity);
        f.SetParent(storyFolder);
    }

    //боковая
    private void PlaceSouthWall(int x, int y, int level, Transform storyFolder, Transform wall)
    {
        float height;
        if (level == 0) height = floorSize.y;
        else height = level * 2 +0.2f + floorSize.y;
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
        else height = level * 2 + 0.2f + floorSize.y;
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
        else height = level * 2 + 0.2f + floorSize.y;
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
        else height = level * 2 + 0.2f + floorSize.y;
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
            roofPrefab[prefabIndex],
            wingFolder.TransformPoint(
               new Vector3(
                       x * -3f + rotationOffset[(int)direction].x,
                        level * 2 +0.25f + floorSize.y,
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
