using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingRenderer : MonoBehaviour
{
    public Transform floorPrefab;
    public Transform[] wallPrefab;
    public Transform[] doorPrefab;
    public Transform[] roofPrefab;
    public Transform stairPrefab;
    public Vector3 floorSize;
    Transform bldgFolder;


    public GameObject Render(Building bldg)
    {
        bldgFolder = new GameObject("Building").transform;
        foreach (Wing wing in bldg.Wings)
        {
            RenderWing(wing, bldg.level, bldg.numberOfEntries);
        }

        return bldgFolder.gameObject;
    }

    private void RenderWing(Wing wing, int level, int numberOfEntries)
    {
        Transform wingFolder = new GameObject("Wing").transform;
        wingFolder.SetParent(bldgFolder);
        foreach (Story story in wing.Stories)
        {
            RenderStory(story, wing, wingFolder, level, numberOfEntries);
        }
        RenderRoof(wing, wingFolder, level);
    }



    private void RenderStory(Story story, Wing wing, Transform wingFolder, int level, int numberOfEntries)
    {
        Transform storyFolder = new GameObject("Story ").transform;
        storyFolder.SetParent(wingFolder);

        List<int> entries = CalculateEntryIndex(wing, numberOfEntries);
        for (int x = wing.Bounds.min.x; x < wing.Bounds.max.x; x++)
        {
            for (int y = wing.Bounds.min.y; y < wing.Bounds.max.y; y++)
            {
                for (int i = 0; i < level; i++)
                {

                    //south wall
                    if (y == wing.Bounds.min.y)
                    {
                        if (i == 0) PlaceFloor(x, y-1, i, new int[3] { 0, -90, 0 }, storyFolder);
                        Transform wall = wallPrefab[(int)story.Walls[x - wing.Bounds.min.x]];
                        PlaceSouthWall(x, y, i, storyFolder, wall);
                    }

                    //east wall
                    if (x == wing.Bounds.min.x + wing.Bounds.size.x - 1)
                    {
                        Transform wall;
                        if (i == 0)
                        {
                            if (entries.Contains(y))
                            {
                                wall = doorPrefab[0];
                                PlaceStair(x, y, i, storyFolder);
                            }
                            else
                            {
                                wall = wallPrefab[(int)story.Walls[wing.Bounds.size.x + y - wing.Bounds.min.y]];
                                PlaceFloor(x +1,  y -1, i, new int[3] { 0, 180, 0 },  storyFolder);
                            }
                        }
                        else
                            wall = wallPrefab[(int)story.Walls[wing.Bounds.size.x + y - wing.Bounds.min.y]];
                        PlaceEastWall(x, y, i, storyFolder, wall);
                    }

                    //north wall
                    if (y == wing.Bounds.min.y + wing.Bounds.size.y - 1)
                    {
                        if (i == 0) PlaceFloor(x+1, y, i, new int[3] { 0, 90, 0 }, storyFolder);
                        Transform wall = wallPrefab[(int)story.Walls[wing.Bounds.size.x * 2 + wing.Bounds.size.y - (x - wing.Bounds.min.x + 1)]];
                        PlaceNorthWall(x, y, i, storyFolder, wall);
                    }

                    //west wall
                    if (x == wing.Bounds.min.x)
                    {
                        if (i == 0) PlaceFloor(x, y, i, new int[3] { 0, 0, 0 }, storyFolder);
                        Transform wall = wallPrefab[(int)story.Walls[(wing.Bounds.size.x + wing.Bounds.size.y) * 2 - (y - wing.Bounds.min.y + 1)]];
                        PlaceWestWall(x, y, i, storyFolder, wall);
                    }

                }
            }
        }
    }

    public List<int> CalculateEntryIndex(Wing wing, int numberOfEntries)
    {
        var doorSpacing = (int)Math.Ceiling((wing.Bounds.size.y - wing.Bounds.min.y) / ((double)numberOfEntries+1));
        Debug.Log("doorspace " + doorSpacing);
        List<int> entries = new();
        for (int i = 1; i <= numberOfEntries; i++)
        {
            int currentDoorPosition = wing.Bounds.min.y + doorSpacing * i - 1;
            entries.Add(currentDoorPosition);
            Debug.Log("position " + currentDoorPosition);
        }
        return entries;
    }
    private void PlaceFloor(int x, int y, int level, int[] angles, Transform storyFolder)
    {
        floorSize = GetPrefabSize(floorPrefab);
        Transform f = Instantiate(floorPrefab, storyFolder.TransformPoint(new Vector3(x * -3f, 0f + level * 2.5f, y * -3f -3f)), Quaternion.Euler(angles[0], angles[1], angles[2]));
        f.SetParent(storyFolder);
    }

    private void PlaceStair(int x, int y, int level, Transform storyFolder)
    {
        var stairSize = GetPrefabSize(stairPrefab);
        Transform f = Instantiate(stairPrefab, storyFolder.TransformPoint(new Vector3(x * -4f -1, 0f + level * 2.5f, y * -3f)), Quaternion.identity);
        f.SetParent(storyFolder);
    }

    //боковая
    private void PlaceSouthWall(int x, int y, int level, Transform storyFolder, Transform wall)
    {
        float height;
        if (level == 0) height = floorSize.y;
        else height = level * 2 + floorSize.y;
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
        else height = level*2 + floorSize.y;
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
        else height = level * 2 + floorSize.y;
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
                Debug.Log(wing.GetRoof.Direction + "direction");
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
                        level * 2 + floorSize.y,
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
            Debug.Log("Prefab size: " + size);
        }
        else
        {
            Debug.LogError("Prefab does not have a MeshRenderer component!");
        }
        Destroy(instance);

        return size;
    }

}
