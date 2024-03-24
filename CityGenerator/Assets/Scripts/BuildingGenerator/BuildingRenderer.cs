using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityMeshSimplifier;

public class BuildingRenderer : MonoBehaviour
{
    public List<Transform> floorPrefabs;
    public List<Transform> wallPrefabs;
    public List<Transform> wallDoorsPrefabs;
    public List<Transform> doorPrefabs;
    public List<Transform> roofPrefabs;
    public List<Transform> roofBoundPrefabs;
    public List<Transform> roofCornerPrefabs;
    public List<Transform> stairPrefabs;
    public List<Transform> balconyPrefabs;

    public int savedSeedOfEastBalconies;
    public int savedNumberOfEastBalconies;

    public int savedSeedOfWestBalconies;
    public int savedNumberOfWestBalconies;

    public int savedSeedOfNorthBalconies;
    public int savedNumberOfNorthBalconies;

    public int savedSeedOfSouthBalconies;
    public int savedNumberOfSouthBalconies;

    public Material oldMaterial;
    public Material newMaterial;
    [SerializeField]
    public bool ChangeWindowLight = false;
    [HideInInspector] public Vector3 floorSize;

    public float probability = 50f;

    public System.Random rand = new System.Random();
    List<Tuple<int, int>> balconiesSouthIndexes = new List<Tuple<int, int>>();
    List<Tuple<int, int>> balconiesNorthIndexes = new List<Tuple<int, int>>();
    List<Tuple<int, int>> balconiesWestIndexes = new List<Tuple<int, int>>();
    List<Tuple<int, int>> balconiesEastIndexes = new List<Tuple<int, int>>();
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
        /* meshCombiner.CombineMeshes(bldgFolder);
         GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
           string objectName = "Wing";
           foreach (GameObject obj in objects)
           {
               if (obj.name == objectName)
               {
                   DestroyImmediate(obj);
               }
           }*/
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
        RenderRoof(wing, wing.Bounds.min.x, wing.Bounds.max.x, wing.Bounds.min.y, wing.Bounds.max.y, wingFolder, bldg.level, bldg);
        if (IsNorthOffsetCorrect(bldg, wing)) RenderNorthOffsetRoof(wing, bldg.minOffsetNorthWall, bldg.maxOffsetNorthWall + 1,
            wing.Bounds.max.y, wing.Bounds.max.y + bldg.depthOffsetNorthWall, wingFolder, bldg.northWallHeight > bldg.level ? bldg.level : bldg.northWallHeight, bldg);
        if (IsSouthOffsetCorrect(bldg, wing)) RenderSouthOffsetRoof(wing, bldg.minOffsetSouthWall, bldg.maxOffsetSouthWall + 1,
            wing.Bounds.min.y - bldg.depthOffsetSouthWall, wing.Bounds.min.y, wingFolder, bldg.southWallHeight > bldg.level ? bldg.level : bldg.southWallHeight, bldg);
        if (IsEastOffsetCorrect(bldg, wing)) RenderEastOffsetRoof(wing, wing.Bounds.max.x, wing.Bounds.max.x + bldg.depthOffsetEastWall,
            bldg.minOffsetEastWall, bldg.maxOffsetEastWall + 1, wingFolder, bldg.eastWallHeight > bldg.level ? bldg.level : bldg.eastWallHeight, bldg);
    }

    private void RenderStory(Story story, Wing wing, Transform wingFolder, Building bldg)
    {
        Transform storyFolder = new GameObject("Story ").transform;
        storyFolder.SetParent(wingFolder);
        List<int> entries = CalculateEntryIndex(wing, bldg.numberOfEntries, bldg);
        List<Tuple<int, int>> offsets = GenerateBuildingShape(bldg.Size.x);
        for (int x = wing.Bounds.min.x; x < wing.Bounds.max.x; x++)
        {
            for (int y = wing.Bounds.min.y; y < wing.Bounds.max.y; y++)
            {
                for (int i = 0; i < bldg.level; i++)
                {
                    //south wall
                    if (y == wing.Bounds.min.y)
                    {
                        if (i == 0)
                        {
                            if (IsSouthOffsetCorrect(bldg, wing) &&
                               x >= bldg.minOffsetSouthWall && x <= bldg.maxOffsetSouthWall)
                                PlaceFloor(x, wing.Bounds.min.y - bldg.depthOffsetSouthWall - 1, i, new int[3] { 0, -90, 0 }, storyFolder, floorPrefabs[bldg.floorSouthOffsetPrefabIndex]);
                            else
                                PlaceFloor(x, y - 1, i, new int[3] { 0, -90, 0 }, storyFolder, floorPrefabs[bldg.floorSouthPrefabIndex]);
                            if (IsEastOffsetCorrect(bldg, wing) && i < bldg.eastWallHeight)
                            {
                                if (x >= wing.Bounds.max.x - bldg.depthOffsetEastWall && x <= wing.Bounds.max.x + bldg.minOffsetEastWall)
                                    PlaceFloor(x + bldg.depthOffsetEastWall, bldg.minOffsetEastWall - 1, i, new int[3] { 0, -90, 0 }, storyFolder, floorPrefabs[bldg.floorSouthOffsetPrefabIndex]);
                            }
                        }

                        if (IsSouthOffsetCorrect(bldg, wing) && i < bldg.southWallHeight)
                        {
                            if (x >= bldg.minOffsetSouthWall && x <= bldg.maxOffsetSouthWall)
                            {
                                if (i == 0)
                                    PlaceSouthWall(x, y + bldg.depthOffsetSouthWall, i, storyFolder, wallPrefabs[bldg.wall0SouthOffsetPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceSouthWall(x, y + bldg.depthOffsetSouthWall, i, storyFolder, wallPrefabs[bldg.wall1SouthOffsetPrefabIndex], bldg);
                                else
                                    PlaceSouthWall(x, y + bldg.depthOffsetSouthWall, i, storyFolder, wallPrefabs[bldg.wall2SouthOffsetPrefabIndex], bldg);
                            }
                            else
                            {
                                if (i == 0)
                                    PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall0SouthPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall1SouthPrefabIndex], bldg);
                                else
                                    PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall2SouthPrefabIndex], bldg);
                                if (PlaceSouthBalcony(bldg, i, x, bldg.numberOfSouthBalconies, bldg.randomSeedOfSouthBalconies, balconiesSouthIndexes))
                                    PlaceSouthWall(x, y, i, storyFolder, balconyPrefabs[bldg.balconySouthPrefabIndex], bldg);
                            }
                        }
                        else
                        {
                            if (i == 0)
                                PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall0SouthPrefabIndex], bldg);
                            else if (i % 2 == 1)
                                PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall1SouthPrefabIndex], bldg);
                            else
                                PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall2SouthPrefabIndex], bldg);
                            if (PlaceSouthBalcony(bldg, i, x, bldg.numberOfSouthBalconies, bldg.randomSeedOfSouthBalconies, balconiesSouthIndexes))
                                PlaceSouthWall(x, y, i, storyFolder, balconyPrefabs[bldg.balconySouthPrefabIndex], bldg);
                        }
                        if (IsEastOffsetCorrect(bldg, wing) && i < bldg.eastWallHeight)
                        {
                            if (x >= wing.Bounds.max.x - bldg.depthOffsetEastWall && x <= wing.Bounds.max.x + bldg.minOffsetEastWall)
                            {
                                if (i == 0)
                                    PlaceSouthWall(x + bldg.depthOffsetEastWall, -bldg.minOffsetEastWall, i, storyFolder, wallPrefabs[bldg.wall0EastOffsetPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceSouthWall(x + bldg.depthOffsetEastWall, -bldg.minOffsetEastWall, i, storyFolder, wallPrefabs[bldg.wall1EastOffsetPrefabIndex], bldg);
                                else
                                    PlaceSouthWall(x + bldg.depthOffsetEastWall, -bldg.minOffsetEastWall, i, storyFolder, wallPrefabs[bldg.wall2EastOffsetPrefabIndex], bldg);
                            }
                        }
                    }

                    //east wall
                    if (x == wing.Bounds.min.x + wing.Bounds.size.x - 1)
                    {
                        if (i == 0)
                        {
                            if (IsNorthOffsetCorrect(bldg, wing) &&
                           y >= wing.Bounds.max.y - bldg.depthOffsetNorthWall && y <= wing.Bounds.max.y + bldg.minOffsetNorthWall)
                                PlaceFloor(x - (wing.Bounds.max.x - bldg.maxOffsetNorthWall - 1) + 1, y + bldg.depthOffsetNorthWall - 1, i, new int[3] { 0, 180, 0 }, storyFolder, floorPrefabs[bldg.floorNorthOffsetPrefabIndex]);

                            if (IsSouthOffsetCorrect(bldg, wing) && y >= wing.Bounds.max.y - bldg.depthOffsetSouthWall && y <= wing.Bounds.max.y + bldg.minOffsetSouthWall)
                                PlaceFloor(x - (wing.Bounds.max.x - bldg.maxOffsetSouthWall - 1) + 1, y - wing.Bounds.max.y - 1, i, new int[3] { 0, 180, 0 }, storyFolder, floorPrefabs[bldg.floorSouthOffsetPrefabIndex]);

                            if (IsEastOffsetCorrect(bldg, wing) && y >= wing.Bounds.min.y + bldg.minOffsetEastWall && y <= wing.Bounds.min.y + bldg.maxOffsetEastWall)
                            {
                                PlaceFloor(x + bldg.depthOffsetEastWall + 1, y - 1, i, new int[3] { 0, 180, 0 }, storyFolder, floorPrefabs[bldg.floorSouthOffsetPrefabIndex]);
                                if (entries.Contains(y))
                                {
                                    PlaceEastWall(x + bldg.depthOffsetEastWall, y, i, storyFolder, doorPrefabs[bldg.doorEastOffsetPrefabIndex], bldg);
                                    PlaceStair(x + bldg.depthOffsetEastWall + 1, y - 1, i, storyFolder, stairPrefabs[bldg.stairEastPrefabIndex], bldg);
                                }
                            }
                            else
                            {
                                PlaceFloor(x + 1, y - 1, i, new int[3] { 0, 180, 0 }, storyFolder, floorPrefabs[bldg.floorEastPrefabIndex]);
                                if (entries.Contains(y))
                                {
                                    PlaceEastWall(x, y, i, storyFolder, doorPrefabs[bldg.doorEastPrefabIndex], bldg);
                                    PlaceStair(x + 1, y - 1, i, storyFolder, stairPrefabs[bldg.stairEastPrefabIndex], bldg);
                                }
                            }
                        }
                        if (IsNorthOffsetCorrect(bldg, wing) && i < bldg.northWallHeight)
                        {
                            if (y >= wing.Bounds.max.y - bldg.depthOffsetNorthWall && y <= wing.Bounds.max.y + bldg.minOffsetNorthWall)
                            {
                                if (i == 0)
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetNorthWall - 1), y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[bldg.wall0NorthOffsetPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetNorthWall - 1), y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[bldg.wall1NorthOffsetPrefabIndex], bldg);
                                else
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetNorthWall - 1), y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[bldg.wall2NorthOffsetPrefabIndex], bldg);
                            }
                        }
                        if (IsSouthOffsetCorrect(bldg, wing) && i < bldg.southWallHeight)
                        {
                            if (y >= wing.Bounds.max.y - bldg.depthOffsetSouthWall && y <= wing.Bounds.max.y + bldg.minOffsetSouthWall)
                            {
                                if (i == 0)
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetSouthWall - 1), y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[bldg.wall0SouthOffsetPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetSouthWall - 1), y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[bldg.wall1SouthOffsetPrefabIndex], bldg);
                                else
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetSouthWall - 1), y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[bldg.wall2SouthOffsetPrefabIndex], bldg);
                            }
                        }

                        if (IsEastOffsetCorrect(bldg, wing) && i < bldg.eastWallHeight)
                        {
                            if (y >= wing.Bounds.min.y + bldg.minOffsetEastWall && y <= wing.Bounds.min.y + bldg.maxOffsetEastWall)
                            {
                                if (i == 0)
                                    PlaceEastWall(x + bldg.depthOffsetEastWall, y, i, storyFolder, wallPrefabs[bldg.wall0EastOffsetPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceEastWall(x + bldg.depthOffsetEastWall, y, i, storyFolder, wallPrefabs[bldg.wall1EastOffsetPrefabIndex], bldg);
                                else
                                    PlaceEastWall(x + bldg.depthOffsetEastWall, y, i, storyFolder, wallPrefabs[bldg.wall2EastOffsetPrefabIndex], bldg);
                            }
                            else
                            {
                                if (i == 0 && entries.Contains(y)) PlaceEastWall(x, y, i, storyFolder, wallDoorsPrefabs[bldg.wallDoorPrefabIndex], bldg);
                                else if (i == 0)
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[bldg.wall0EastPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[bldg.wall1EastPrefabIndex], bldg);
                                else
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[bldg.wall2EastPrefabIndex], bldg);
                            }
                        }
                        else
                        {
                            {

                                if (i == 0 && entries.Contains(y)) PlaceEastWall(x, y, i, storyFolder, wallDoorsPrefabs[bldg.wallDoorPrefabIndex], bldg);
                                else if (i == 0)
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[bldg.wall0EastPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[bldg.wall1EastPrefabIndex], bldg);
                                else
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[bldg.wall2EastPrefabIndex], bldg);
                                if (PlaceEastBalcony(bldg, i, y, bldg.numberOfEastBalconies, bldg.randomSeedOfEastBalconies, balconiesEastIndexes))
                                {
                                    PlaceEastWall(x, y, i, storyFolder, balconyPrefabs[bldg.balconyEastPrefabIndex], bldg);
                                }
                            }
                        }
                    }

                    //north wall
                    if (y == wing.Bounds.min.y + wing.Bounds.size.y - 1)
                    {
                        if (i == 0)
                        {
                            if (IsNorthOffsetCorrect(bldg, wing) && x >= bldg.minOffsetNorthWall && x <= bldg.maxOffsetNorthWall)
                                PlaceFloor(x + 1, y + bldg.depthOffsetNorthWall, i, new int[3] { 0, 90, 0 }, storyFolder, floorPrefabs[bldg.floorNorthOffsetPrefabIndex]);
                            else
                                PlaceFloor(x + 1, y, i, new int[3] { 0, 90, 0 }, storyFolder, floorPrefabs[bldg.floorNorthPrefabIndex]);
                            if (IsEastOffsetCorrect(bldg, wing) && i < bldg.eastWallHeight)
                            {
                                if (x >= wing.Bounds.max.x - bldg.depthOffsetEastWall && x <= wing.Bounds.max.x + bldg.minOffsetEastWall)
                                    PlaceFloor(x + bldg.depthOffsetEastWall + 1, bldg.maxOffsetEastWall, i, new int[3] { 0, 90, 0 }, storyFolder, floorPrefabs[bldg.floorSouthOffsetPrefabIndex]);
                            }
                        }
                        if (IsNorthOffsetCorrect(bldg, wing) && i < bldg.northWallHeight)
                        {
                            if (x >= bldg.minOffsetNorthWall && x <= bldg.maxOffsetNorthWall)
                            {
                                if (i == 0)
                                    PlaceNorthWall(x, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[bldg.wall0NorthOffsetPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceNorthWall(x, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[bldg.wall1NorthOffsetPrefabIndex], bldg);
                                else
                                    PlaceNorthWall(x, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[bldg.wall2NorthOffsetPrefabIndex], bldg);
                            }
                            else
                            {
                                if (i == 0)
                                    PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall0NorthPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall1NorthPrefabIndex], bldg);
                                else PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall2NorthPrefabIndex], bldg);
                                if (PlaceNorthBalcony(bldg, i, x, bldg.numberOfNorthBalconies, bldg.randomSeedOfNorthBalconies, balconiesNorthIndexes)) PlaceNorthWall(x, y, i, storyFolder, balconyPrefabs[bldg.balconyNorthPrefabIndex], bldg);
                            }
                        }
                        else
                        {
                            if (i == 0)
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall0NorthPrefabIndex], bldg);
                            else if (i % 2 == 1)
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall1NorthPrefabIndex], bldg);
                            else
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall2NorthPrefabIndex], bldg);
                            if (PlaceNorthBalcony(bldg, i, x, bldg.numberOfNorthBalconies, bldg.randomSeedOfNorthBalconies, balconiesNorthIndexes)) PlaceNorthWall(x, y, i, storyFolder, balconyPrefabs[bldg.balconyNorthPrefabIndex], bldg);
                        }
                        if (IsEastOffsetCorrect(bldg, wing) && i < bldg.eastWallHeight)
                        {
                            if (x >= wing.Bounds.max.x - bldg.depthOffsetEastWall && x <= wing.Bounds.max.x + bldg.minOffsetEastWall)
                            {
                                if (i == 0)
                                    PlaceNorthWall(x + bldg.depthOffsetEastWall, bldg.maxOffsetEastWall, i, storyFolder, wallPrefabs[bldg.wall0EastOffsetPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceNorthWall(x + bldg.depthOffsetEastWall, bldg.maxOffsetEastWall, i, storyFolder, wallPrefabs[bldg.wall1EastOffsetPrefabIndex], bldg);
                                else
                                    PlaceNorthWall(x + bldg.depthOffsetEastWall, bldg.maxOffsetEastWall, i, storyFolder, wallPrefabs[bldg.wall2EastOffsetPrefabIndex], bldg);
                            }
                        }
                        else
                        {
                            if (i == 0)
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall0NorthPrefabIndex], bldg);
                            else if (i % 2 == 1)
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall1NorthPrefabIndex], bldg);
                            else
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[bldg.wall2NorthPrefabIndex], bldg);
                            if (PlaceNorthBalcony(bldg, i, x, bldg.numberOfNorthBalconies, bldg.randomSeedOfNorthBalconies, balconiesNorthIndexes)) PlaceNorthWall(x, y, i, storyFolder, balconyPrefabs[bldg.balconyNorthPrefabIndex], bldg);
                        }

                    }

                    //west wall
                    if (x == wing.Bounds.min.x)
                    {
                        if (i == 0)
                        {
                            if (IsNorthOffsetCorrect(bldg, wing) && y >= wing.Bounds.max.y - bldg.depthOffsetNorthWall && y <= wing.Bounds.max.y + bldg.minOffsetNorthWall)
                                PlaceFloor(x + bldg.minOffsetNorthWall, y + bldg.depthOffsetNorthWall, i, new int[3] { 0, 0, 0 }, storyFolder, floorPrefabs[bldg.floorNorthOffsetPrefabIndex]);
                            if (IsSouthOffsetCorrect(bldg, wing) && y >= wing.Bounds.max.y - bldg.depthOffsetSouthWall && y <= wing.Bounds.max.y + bldg.minOffsetSouthWall)
                                PlaceFloor(x + bldg.minOffsetSouthWall, y - wing.Bounds.max.y, i, new int[3] { 0, 0, 0 }, storyFolder, floorPrefabs[bldg.floorSouthOffsetPrefabIndex]);
                            PlaceFloor(x, y, i, new int[3] { 0, 0, 0 }, storyFolder, floorPrefabs[bldg.floorWestPrefabIndex]);
                        }
                        if (PlaceWestBalcony(bldg, i, y, bldg.numberOfWestBalconies, bldg.randomSeedOfWestBalconies, balconiesWestIndexes)) PlaceWestWall(x, y, i, storyFolder, balconyPrefabs[bldg.balconyWestPrefabIndex], bldg);
                        if (i == 0)
                            PlaceWestWall(x, y, i, storyFolder, wallPrefabs[bldg.wall0WestPrefabIndex], bldg);
                        else if (i % 2 == 1)
                            PlaceWestWall(x, y, i, storyFolder, wallPrefabs[bldg.wall1WestPrefabIndex], bldg);
                        else PlaceWestWall(x, y, i, storyFolder, wallPrefabs[bldg.wall2WestPrefabIndex], bldg);
                        if (IsNorthOffsetCorrect(bldg, wing) && i < bldg.northWallHeight)
                        {
                            if (y >= wing.Bounds.max.y - bldg.depthOffsetNorthWall && y <= wing.Bounds.max.y + bldg.minOffsetNorthWall)
                            {
                                if (i == 0)
                                    PlaceWestWall(x + bldg.minOffsetNorthWall, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[bldg.wall0NorthOffsetPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceWestWall(x + bldg.minOffsetNorthWall, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[bldg.wall1NorthOffsetPrefabIndex], bldg);
                                else PlaceWestWall(x + bldg.minOffsetNorthWall, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[bldg.wall2NorthOffsetPrefabIndex], bldg);
                            }
                        }
                        if (IsSouthOffsetCorrect(bldg, wing) && i < bldg.southWallHeight)
                        {
                            if (y >= wing.Bounds.max.y - bldg.depthOffsetSouthWall && y <= wing.Bounds.max.y + bldg.minOffsetSouthWall)
                            {
                                if (i == 0)
                                    PlaceWestWall(x + bldg.minOffsetSouthWall, y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[bldg.wall0SouthOffsetPrefabIndex], bldg);
                                else if (i % 2 == 1)
                                    PlaceWestWall(x + bldg.minOffsetSouthWall, y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[bldg.wall1SouthOffsetPrefabIndex], bldg);
                                else PlaceWestWall(x + bldg.minOffsetSouthWall, y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[bldg.wall2SouthOffsetPrefabIndex], bldg);
                            }
                        }
                    }

                }
            }
        }
    }

    public bool IsNorthOffsetCorrect(Building bldg, Wing wing)
    {
        if (bldg.depthOffsetNorthWall > wing.Bounds.max.y) bldg.depthOffsetNorthWall = wing.Bounds.max.y;
        return bldg.northWallHeight > 0 && bldg.depthOffsetNorthWall > 0 && bldg.minOffsetNorthWall >= 0 && bldg.minOffsetNorthWall <= wing.Bounds.max.x &&
                             bldg.maxOffsetNorthWall >= 0 && bldg.maxOffsetNorthWall < wing.Bounds.size.x && bldg.maxOffsetNorthWall >= bldg.minOffsetNorthWall;
    }

    public bool IsSouthOffsetCorrect(Building bldg, Wing wing)
    {
        if (bldg.depthOffsetSouthWall > wing.Bounds.max.y) bldg.depthOffsetSouthWall = wing.Bounds.max.y;
        return bldg.northWallHeight > 0 && bldg.depthOffsetSouthWall > 0 && bldg.minOffsetSouthWall >= 0 && bldg.minOffsetSouthWall <= wing.Bounds.max.x &&
                             bldg.maxOffsetSouthWall >= 0 && bldg.maxOffsetSouthWall < wing.Bounds.size.x && bldg.maxOffsetSouthWall >= bldg.minOffsetSouthWall;
    }

    public bool IsEastOffsetCorrect(Building bldg, Wing wing)
    {
        if (bldg.depthOffsetEastWall > wing.Bounds.max.x) bldg.depthOffsetEastWall = wing.Bounds.max.x;
        if (bldg.maxOffsetEastWall > wing.Bounds.max.y) bldg.maxOffsetEastWall = wing.Bounds.max.y - 1;
        return bldg.eastWallHeight > 0 && bldg.depthOffsetEastWall > 0 && bldg.minOffsetEastWall >= 0 && bldg.minOffsetEastWall <= wing.Bounds.max.y &&
                             bldg.maxOffsetEastWall >= 0 && bldg.maxOffsetEastWall < wing.Bounds.size.y && bldg.maxOffsetEastWall >= bldg.minOffsetEastWall;
    }
    public List<Tuple<int, int>> GenerateBuildingShape(int max)
    {
        List<Tuple<int, int>> offset = new();
        offset.Add(new Tuple<int, int>(rand.Next(0, max), rand.Next(0, max)));
        var sortedTuples = offset.Select(t => Tuple.Create(Math.Min(t.Item1, t.Item2), Math.Max(t.Item1, t.Item2))).ToList();
        return sortedTuples;
    }

    public List<Tuple<int, int>> CalculateSouthBalconiesIndex(Building bldg, int numberOfBalconies, int randomSeedOfBalconies)
    {
        if (randomSeedOfBalconies == savedSeedOfSouthBalconies && numberOfBalconies == savedNumberOfSouthBalconies) return balconiesSouthIndexes;
        savedSeedOfSouthBalconies = randomSeedOfBalconies;
        savedNumberOfSouthBalconies = numberOfBalconies;
        balconiesSouthIndexes.Clear();
        var balconySpacing = (int)Math.Ceiling(bldg.Size.y / (double)numberOfBalconies);
        if (randomSeedOfBalconies == 0)
        {
            for (int i = 1; i <= bldg.level; i++)
            {
                for (int j = 0; j < numberOfBalconies; j++)
                    balconiesSouthIndexes.Add(new Tuple<int, int>(i, j * balconySpacing));
            }
        }
        else
        {
            for (int i = 1; i < numberOfBalconies; i++)
            {
                balconiesSouthIndexes.Add(new Tuple<int, int>(rand.Next(2, 2 + randomSeedOfBalconies), rand.Next(0, 2 + randomSeedOfBalconies)));
            }
        }
        return balconiesSouthIndexes;
    }

    public List<Tuple<int, int>> CalculateNorthBalconiesIndex(Building bldg, int numberOfBalconies, int randomSeedOfBalconies)
    {
        if (randomSeedOfBalconies == savedSeedOfNorthBalconies && numberOfBalconies == savedNumberOfNorthBalconies) return balconiesNorthIndexes;
        savedSeedOfNorthBalconies = randomSeedOfBalconies;
        savedNumberOfNorthBalconies = numberOfBalconies;
        balconiesNorthIndexes.Clear();
        var balconySpacing = (int)Math.Ceiling(bldg.Size.y / (double)numberOfBalconies);
        if (randomSeedOfBalconies == 0)
        {
            for (int i = 1; i <= bldg.level; i++)
            {
                for (int j = 0; j < numberOfBalconies; j++)
                    balconiesNorthIndexes.Add(new Tuple<int, int>(i, j * balconySpacing));
            }
        }
        else
        {
            for (int i = 1; i < numberOfBalconies; i++)
            {
                balconiesNorthIndexes.Add(new Tuple<int, int>(rand.Next(2, 2 + randomSeedOfBalconies), rand.Next(0, 2 + randomSeedOfBalconies)));
            }
        }
        return balconiesNorthIndexes;
    }
    public List<Tuple<int, int>> CalculateWestBalconiesIndex(Building bldg, int numberOfBalconies, int randomSeedOfBalconies)
    {
        if (randomSeedOfBalconies == savedSeedOfWestBalconies && numberOfBalconies == savedNumberOfWestBalconies) return balconiesWestIndexes;
        savedSeedOfWestBalconies = randomSeedOfBalconies;
        savedNumberOfWestBalconies = numberOfBalconies;
        balconiesWestIndexes.Clear();
        var balconySpacing = (int)Math.Ceiling(bldg.Size.y / (double)numberOfBalconies);
        if (randomSeedOfBalconies == 0)
        {
            for (int i = 1; i <= bldg.level; i++)
            {
                for (int j = 0; j < numberOfBalconies; j++)
                    balconiesWestIndexes.Add(new Tuple<int, int>(i, j * balconySpacing));
            }
        }
        else
        {
            for (int i = 1; i < numberOfBalconies; i++)
            {
                balconiesWestIndexes.Add(new Tuple<int, int>(rand.Next(2, 2 + randomSeedOfBalconies), rand.Next(0, 2 + randomSeedOfBalconies)));
            }
        }
        return balconiesWestIndexes;
    }

    public List<Tuple<int, int>> CalculateEastBalconiesIndex(Building bldg, int numberOfBalconies, int randomSeedOfBalconies)
    {
        if (randomSeedOfBalconies == savedSeedOfEastBalconies && numberOfBalconies == savedNumberOfEastBalconies) return balconiesEastIndexes;
        savedSeedOfEastBalconies = randomSeedOfBalconies;
        savedNumberOfEastBalconies = numberOfBalconies;
        balconiesEastIndexes.Clear();
        var balconySpacing = (int)Math.Ceiling(bldg.Size.y / (double)numberOfBalconies);
        if (randomSeedOfBalconies == 0)
        {
            for (int i = 1; i <= bldg.level + 1; i++)
            {
                for (int j = 0; j < numberOfBalconies; j++)
                    balconiesEastIndexes.Add(new Tuple<int, int>(i, j * balconySpacing));
            }
        }
        else
        {
            for (int i = 1; i < numberOfBalconies; i++)
            {
                balconiesEastIndexes.Add(new Tuple<int, int>(rand.Next(2, 2 + randomSeedOfBalconies), rand.Next(0, 2 + randomSeedOfBalconies)));
            }
        }
        bldg.balconyLocations = balconiesEastIndexes;
        return balconiesEastIndexes;
    }
    public List<int> CalculateEntryIndex(Wing wing, int numberOfEntries, Building bldg)
    {
        var doorSpacing = (int)Math.Ceiling((wing.Bounds.size.y - wing.Bounds.min.y) / ((double)numberOfEntries + 1));
        List<int> entries = new();
        for (int i = 1; i <= numberOfEntries; i++)
        {
            int currentDoorPosition = wing.Bounds.min.y + doorSpacing * i - 2 + bldg.offsetOfEntries;
            entries.Add(currentDoorPosition);
        }
        return entries;
    }
    private void PlaceFloor(int x, int y, int level, int[] angles, Transform storyFolder, Transform floor)
    {
        floorSize = GetPrefabSize(floor);
        Transform f = Instantiate(floor, storyFolder.TransformPoint(new Vector3(x * -3f, 0f + level * 2.5f, y * -3f - 3f)), Quaternion.Euler(angles[0], angles[1], angles[2]));
        f.SetParent(storyFolder);
    }

    private bool PlaceSouthBalcony(Building bldg, int level, int place, int number, int random, List<Tuple<int, int>> balconiesIndexes)
    {
        balconiesIndexes = CalculateSouthBalconiesIndex(bldg, number, random);
        Tuple<int, int> pairToCheck = new Tuple<int, int>(2, 2);
        pairToCheck = new Tuple<int, int>(level, place);
        return balconiesIndexes.Contains(pairToCheck);
    }
    private bool PlaceNorthBalcony(Building bldg, int level, int place, int number, int random, List<Tuple<int, int>> balconiesIndexes)
    {
        balconiesIndexes = CalculateNorthBalconiesIndex(bldg, number, random);
        Tuple<int, int> pairToCheck = new Tuple<int, int>(2, 2);
        pairToCheck = new Tuple<int, int>(level, place);
        return balconiesIndexes.Contains(pairToCheck);
    }
    private bool PlaceWestBalcony(Building bldg, int level, int place, int number, int random, List<Tuple<int, int>> balconiesIndexes)
    {
        balconiesIndexes = CalculateWestBalconiesIndex(bldg, number, random);
        Tuple<int, int> pairToCheck = new Tuple<int, int>(2, 2);
        pairToCheck = new Tuple<int, int>(level, place);
        return balconiesIndexes.Contains(pairToCheck);
    }
    private bool PlaceEastBalcony(Building bldg, int level, int place, int number, int random, List<Tuple<int, int>> balconiesIndexes)
    {
        balconiesIndexes = CalculateEastBalconiesIndex(bldg, number, random);
        Tuple<int, int> pairToCheck = new Tuple<int, int>(2, 2);
        pairToCheck = new Tuple<int, int>(level, place);
        return balconiesIndexes.Contains(pairToCheck);
    }
    private void PlaceStair(int x, int y, int level, Transform storyFolder, Transform stair, Building bldg)
    {
        var stairSize = GetPrefabSize(stairPrefabs[bldg.stairSouthPrefabIndex]);
        Transform f = Instantiate(stair, storyFolder.TransformPoint(new Vector3(x * -3f, 0f + level * 2.5f, y * -3f - 3f)), Quaternion.identity);
        f.SetParent(storyFolder);
    }

    //боковая
    private void PlaceSouthWall(int x, int y, int level, Transform storyFolder, Transform wall, Building bldg)
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
        if (level == bldg.level - 1)
        {
            try
            {
                var stair = w.Find("stair_Cube.051").gameObject;
                DestroyImmediate(stair);
            }
            catch (NullReferenceException)
            {
                return;
            }
        }
        if (level > 0 &&  ChangeWindowLight && UnityEngine.Random.Range(0f, 100f) < probability)
        {
            ChangeWindowMaterial(w);
        }
    }


    private void PlaceEastWall(int x, int y, int level, Transform storyFolder, Transform wall, Building bldg)
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
        if (level == bldg.level-1)
        {
            try
            {
                var stair = w.Find("stair_Cube.051").gameObject;
                DestroyImmediate(stair);
            }
            catch (NullReferenceException)
            {
                return;
            }
        }
        if (level > 0 && ChangeWindowLight && UnityEngine.Random.Range(0f, 100f) < probability)
        {
            ChangeWindowMaterial(w);
        }
    }

    private void PlaceNorthWall(int x, int y, int level, Transform storyFolder, Transform wall, Building bldg)
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
        if (level == bldg.level - 1)
        {
            try
            {
                var stair = w.Find("stair_Cube.051").gameObject;
                DestroyImmediate(stair);
            }
            catch (NullReferenceException)
            {
                return;
            }
        }
        if (level > 0 && ChangeWindowLight && UnityEngine.Random.Range(0f, 100f) < probability)
        {
            ChangeWindowMaterial(w);
        }
    }

    private void PlaceWestWall(int x, int y, int level, Transform storyFolder, Transform wall, Building bldg)
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
                    y * -3f - 3f
                    )
                ),
            Quaternion.Euler(0, 180, 0));
        w.SetParent(storyFolder);
        if (level == bldg.level - 1)
        {
            try
            {
                var stair = w.Find("stair_Cube.051").gameObject;
                DestroyImmediate(stair);
            }
            catch (NullReferenceException)
            {
                return;
            }
        }
        if (level > 0 && ChangeWindowLight && UnityEngine.Random.Range(0f, 100f) < probability)
        {
            ChangeWindowMaterial(w);
        }
    }
    public void ChangeWindowMaterial(Transform wall)
    {
        Transform windowTransform = wall ;
        try
        {
            windowTransform = wall.GetChild(1);
        }
        catch (NullReferenceException)
        {
            Debug.Log("changing light is not impossible");
        }
        if (UnityEngine.Random.Range(0f, 100f) < 10f)
        {

            var renderer = windowTransform.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material[] materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i].name == oldMaterial.name)
                    {
                        materials[i] = newMaterial;
                    }
                    else
                        if (materials[i].name == newMaterial.name)
                    {
                        materials[i] = oldMaterial;
                    }
                }
                renderer.sharedMaterials = materials;
            }
        }
    }

    private void RenderRoof(Wing wing, int minX, int maxX, int minY, int maxY, Transform wingFolder, int level, Building bldg)
    {
        var direction = new RoofDirection();
        Transform prefab;
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if ((y == minY) && (x == minX))
                {
                    prefab = roofCornerPrefabs[bldg.roofCornerPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if ((y == minY) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[bldg.roofCornerPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if ((y == minY) && (x == minX))
                {
                    prefab = roofCornerPrefabs[bldg.roofCornerPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                if ((y == minY) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[bldg.roofCornerPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if ((y == maxY - 1) && (x == minX))
                {
                    prefab = roofCornerPrefabs[bldg.roofCornerPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if ((y == maxY - 1) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[bldg.roofCornerPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                if (y == minY && !(y == maxY - 1 || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofBoundPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if (y == maxY - 1 && !(y == minY || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofBoundPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if (x == minX && !(y == minY || y == maxY - 1 || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofBoundPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if (x == maxX - 1 && !(y == minY || y == maxY - 1 || x == minX))
                {
                    prefab = roofBoundPrefabs[bldg.roofBoundPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                {
                    prefab = roofPrefabs[bldg.roofPrefabIndex];
                    direction = RoofDirection.North;
                }

                PlaceRoof(x, y, level, wingFolder, wing.GetRoof.Type, direction, prefab);
            }
        }
    }

    private void RenderNorthOffsetRoof(Wing wing, int minX, int maxX, int minY, int maxY, Transform wingFolder, int level, Building bldg)
    {
        var direction = new RoofDirection();
        Transform prefab;
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if (y == minY  || y == maxY - 1 )
                {
                    prefab = roofCornerPrefabs[bldg.roofNorthOffsetCornerPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if ((y == minY) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[bldg.roofNorthOffsetCornerPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if ((y == maxY - 1) && (x == minX))
                {
                    prefab = roofCornerPrefabs[bldg.roofNorthOffsetCornerPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if ((y == maxY - 1) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[bldg.roofNorthOffsetCornerPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                if (y == minY && !(y == maxY - 1 || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofNorthOffsetBoundPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if (y == maxY - 1 && !(y == minY || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofNorthOffsetBoundPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if (x == minX && !(y == minY || y == maxY - 1 || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofNorthOffsetBoundPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if (x == maxX - 1 && !(y == minY || y == maxY - 1 || x == minX))
                {
                    prefab = roofBoundPrefabs[bldg.roofNorthOffsetBoundPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                {
                    prefab = roofPrefabs[bldg.roofNorthOffsetPrefabIndex];
                    direction = RoofDirection.North;
                }

                PlaceRoof(x, y, level, wingFolder, wing.GetRoof.Type, direction, prefab);
            }
        }
    }

    private void RenderSouthOffsetRoof(Wing wing, int minX, int maxX, int minY, int maxY, Transform wingFolder, int level, Building bldg)
    {
        var direction = new RoofDirection();
        Transform prefab;
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if ((y == minY) && (x == minX))
                {
                    prefab = roofCornerPrefabs[bldg.roofSouthOffsetCornerPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if ((y == minY) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[bldg.roofSouthOffsetCornerPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if ((y == maxY - 1) && (x == minX))
                {
                    prefab = roofCornerPrefabs[bldg.roofSouthOffsetCornerPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if ((y == maxY - 1) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[bldg.roofSouthOffsetCornerPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                if (y == minY && !(y == maxY - 1 || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofSouthOffsetBoundPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if (y == maxY - 1 && !(y == minY || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofSouthOffsetBoundPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if (x == minX && !(y == minY || y == maxY - 1 || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofSouthOffsetBoundPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if (x == maxX - 1 && !(y == minY || y == maxY - 1 || x == minX))
                {
                    prefab = roofBoundPrefabs[bldg.roofSouthOffsetBoundPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                {
                    prefab = roofPrefabs[bldg.roofSouthOffsetPrefabIndex];
                    direction = RoofDirection.North;
                }

                PlaceRoof(x, y, level, wingFolder, wing.GetRoof.Type, direction, prefab);
            }
        }
    }
    private void RenderEastOffsetRoof(Wing wing, int minX, int maxX, int minY, int maxY, Transform wingFolder, int level, Building bldg)
    {
        var direction = new RoofDirection();
        Transform prefab;
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if ((y == minY) && (x == minX))
                {
                    prefab = roofCornerPrefabs[bldg.roofEastOffsetCornerPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if ((y == minY) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[bldg.roofEastOffsetCornerPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if ((y == maxY - 1) && (x == minX))
                {
                    prefab = roofCornerPrefabs[bldg.roofEastOffsetCornerPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if ((y == maxY - 1) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[bldg.roofEastOffsetCornerPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                if (y == minY && !(y == maxY - 1 || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofEastOffsetBoundPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if (y == maxY - 1 && !(y == minY || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofEastOffsetBoundPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if (x == minX && !(y == minY || y == maxY - 1 || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[bldg.roofEastOffsetBoundPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if (x == maxX - 1 && !(y == minY || y == maxY - 1 || x == minX))
                {
                    prefab = roofBoundPrefabs[bldg.roofEastOffsetBoundPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                {
                    prefab = roofPrefabs[bldg.roofEastOffsetPrefabIndex];
                    direction = RoofDirection.North;
                }

                PlaceRoof(x, y, level, wingFolder, wing.GetRoof.Type, direction, prefab);
            }
        }
    }

    private void PlaceRoof(int x, int y, int level, Transform wingFolder, RoofType type, RoofDirection direction, Transform prefab)
    {
        Transform r;
        r = Instantiate(
            prefab,
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
        }
        else
        {
            Debug.LogError("Prefab does not have a MeshRenderer component!");
        }
        DestroyImmediate(instance);

        return size;
    }
 
}
