using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRenderer : MonoBehaviour
{
    public Transform floorPrefab;
    public Transform[] wallPrefab;
    public Transform[] roofPrefab;
    Transform bldgFolder;


    public GameObject Render(Building bldg)
    {
        bldgFolder = new GameObject("Building").transform;
        foreach (Wing wing in bldg.Wings)
        {
            RenderWing(wing, bldg.level);
        }

        return bldgFolder.gameObject;
    }

    private void RenderWing(Wing wing, int level)
    {
        Transform wingFolder = new GameObject("Wing").transform;
        wingFolder.SetParent(bldgFolder);
        foreach (Story story in wing.Stories)
        {
            RenderStory(story, wing, wingFolder, level);
        }
        RenderRoof(wing, wingFolder, level);
    }



    private void RenderStory(Story story, Wing wing, Transform wingFolder, int level)
    {
        Transform storyFolder = new GameObject("Story ").transform;
        storyFolder.SetParent(wingFolder);
        for (int x = wing.Bounds.min.x; x < wing.Bounds.max.x; x++)
        {
            for (int y = wing.Bounds.min.y; y < wing.Bounds.max.y; y++)
            {
                for (int i = 0; i < level; i++)
                {
                    PlaceFloor(x, y, i, storyFolder);

                    //south wall
                    if (y == wing.Bounds.min.y)
                    {
                        Transform wall = wallPrefab[(int)story.Walls[x - wing.Bounds.min.x]];
                        PlaceSouthWall(x, y, i, storyFolder, wall);
                    }

                    //east wall
                    if (x == wing.Bounds.min.x + wing.Bounds.size.x - 1)
                    {
                        Transform wall = wallPrefab[(int)story.Walls[wing.Bounds.size.x + y - wing.Bounds.min.y]];
                        PlaceEastWall(x, y, i, storyFolder, wall);
                    }

                    //north wall
                    if (y == wing.Bounds.min.y + wing.Bounds.size.y - 1)
                    {
                        Transform wall = wallPrefab[(int)story.Walls[wing.Bounds.size.x * 2 + wing.Bounds.size.y - (x - wing.Bounds.min.x + 1)]];
                        PlaceNorthWall(x, y, i, storyFolder, wall);
                    }

                    //west wall
                    if (x == wing.Bounds.min.x)
                    {
                        Transform wall = wallPrefab[(int)story.Walls[(wing.Bounds.size.x + wing.Bounds.size.y) * 2 - (y - wing.Bounds.min.y + 1)]];
                        PlaceWestWall(x, y, i, storyFolder, wall);
                    }

                }
            }
        }
    }

    private void PlaceFloor(int x, int y, int level, Transform storyFolder)
    {
        Transform f = Instantiate(floorPrefab, storyFolder.TransformPoint(new Vector3(x * -3f, 0f + level * 2.5f, y * -3f)), Quaternion.identity);
        f.SetParent(storyFolder);
    }

    private void PlaceSouthWall(int x, int y, int level, Transform storyFolder, Transform wall)
    {
        Transform w = Instantiate(
            wall,
            storyFolder.TransformPoint(
                new Vector3(
                    x * -3f,
                    0.3f + level * 2.5f,
                    y * 3f - 0.5f
                    )
                ),
            Quaternion.Euler(0, 90, 0));
        w.SetParent(storyFolder);
    }

    private void PlaceEastWall(int x, int y, int level, Transform storyFolder, Transform wall)
    {
        Transform w = Instantiate(
            wall,
            storyFolder.TransformPoint(
                new Vector3(
                    x * -3f - 2.5f,
                    0.3f + level * 2.5f,
                    y * -3f
                    )
                ),
            Quaternion.identity);
        w.SetParent(storyFolder);
    }

    private void PlaceNorthWall(int x, int y, int level, Transform storyFolder, Transform wall)
    {
        Transform w = Instantiate(
            wall,
            storyFolder.TransformPoint(
                new Vector3(
                    x * -3f,
                    0.3f + level * 2.5f,
                    y * -3f - 3f
                    )
                ),
            Quaternion.Euler(0, 90, 0));
        w.SetParent(storyFolder);
    }

    private void PlaceWestWall(int x, int y, int level, Transform storyFolder, Transform wall)
    {
        Transform w = Instantiate(
            wall,
            storyFolder.TransformPoint(
                new Vector3(
                    x * -3f,
                    0.3f + level * 2.5f,
                    y * -3f
                    )
                ),
            Quaternion.identity);
        w.SetParent(storyFolder);
    }

    private void RenderRoof(Wing wing, Transform wingFolder, int level)
    {
        for (int x = wing.Bounds.min.x; x < wing.Bounds.max.x; x++)
        {
            for (int y = wing.Bounds.min.y; y < wing.Bounds.max.y; y++)
            {
                PlaceRoof(x, y, level, wingFolder, wing.GetRoof.Type, wing.GetRoof.Direction);
            }
        }
    }

    private void PlaceRoof(int x, int y, int level, Transform wingFolder, RoofType type, RoofDirection direction)
    {
        Transform r;
        r = Instantiate(
            roofPrefab[(int)type],
            wingFolder.TransformPoint(
               new Vector3(
                       x * -3f + rotationOffset[(int)direction].x,
                        level * 2.2f + (type == RoofType.Point ? -0.3f : 0f),
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

    public float GetPrefubSize(RoofType type)
    {
        float size = new();
        {
            // Создаем экземпляр префаба
            var instance = Instantiate(roofPrefab[0], Vector3.zero, Quaternion.identity);

            // Получаем коллайдер из экземпляра префаба
            Renderer prefabRenderer = roofPrefab[0].GetComponent<MeshRenderer>();

            if (prefabRenderer != null)
            {
                // Получаем размеры коллайдера
                size = prefabRenderer.bounds.size.y;

                // Выводим размеры в консоль
                Debug.Log("Prefab size: " + size);

                // Вы можете использовать size.x, size.y, size.z для получения конкретных размеров по осям
            }
            else
            {
                Debug.LogError("Prefab does not have a collider!");
            }

            // Уничтожаем временный экземпляр префаба
            Destroy(instance);
            return size;
        }
    }
}
