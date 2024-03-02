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


    [HideInInspector] public Vector3 floorSize;

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
        bldgFolder.AddComponent<LODGeneratorHelper>();
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
        RenderRoof(wing, wing.Bounds.min.x, wing.Bounds.max.x, wing.Bounds.min.y, wing.Bounds.max.y, wingFolder, bldg.level);
        if (IsNorthOffsetCorrect(bldg, wing)) RenderNorthOffsetRoof(wing, bldg.minOffsetNorthWall, bldg.maxOffsetNorthWall+1, 
            wing.Bounds.max.y, wing.Bounds.max.y + bldg.depthOffsetNorthWall, wingFolder, bldg.northWallHeight > bldg.level ? bldg.level : bldg.northWallHeight);
        if (IsSouthOffsetCorrect(bldg, wing)) RenderSouthOffsetRoof(wing, bldg.minOffsetSouthWall, bldg.maxOffsetSouthWall + 1,
            wing.Bounds.min.y - bldg.depthOffsetSouthWall, wing.Bounds.min.y, wingFolder, bldg.southWallHeight > bldg.level ? bldg.level : bldg.southWallHeight);
        if (IsEastOffsetCorrect(bldg, wing)) RenderEastOffsetRoof(wing, wing.Bounds.max.x, wing.Bounds.max.x + bldg.depthOffsetEastWall, 
            bldg.minOffsetEastWall, bldg.maxOffsetEastWall + 1, wingFolder, bldg.eastWallHeight > bldg.level ? bldg.level : bldg.eastWallHeight);
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
                                PlaceFloor(x, wing.Bounds.min.y - bldg.depthOffsetSouthWall-1, i, new int[3] { 0, -90, 0 }, storyFolder, floorPrefabs[floorSouthOffsetPrefabIndex]);
                            else
                                PlaceFloor(x, y - 1, i, new int[3] { 0, -90, 0 }, storyFolder, floorPrefabs[floorSouthPrefabIndex]);
                            if (IsEastOffsetCorrect(bldg, wing) && i < bldg.eastWallHeight)
                            {
                                if (x >= wing.Bounds.max.x - bldg.depthOffsetEastWall && x <= wing.Bounds.max.x + bldg.minOffsetEastWall)
                                    PlaceFloor(x + bldg.depthOffsetEastWall, bldg.minOffsetEastWall-1, i, new int[3] { 0, -90, 0 }, storyFolder, floorPrefabs[floorSouthOffsetPrefabIndex]);
                            }
                        }

                        if (IsSouthOffsetCorrect(bldg, wing) && i < bldg.southWallHeight)
                        {
                            if (x >= bldg.minOffsetSouthWall && x <= bldg.maxOffsetSouthWall)
                            {
                                if (i == 0)
                                    PlaceSouthWall(x, y + bldg.depthOffsetSouthWall, i, storyFolder, wallPrefabs[wall0SouthOffsetPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceSouthWall(x, y + bldg.depthOffsetSouthWall, i, storyFolder, wallPrefabs[wall1SouthOffsetPrefabIndex]);
                                else
                                    PlaceSouthWall(x, y + bldg.depthOffsetSouthWall, i, storyFolder, wallPrefabs[wall2SouthOffsetPrefabIndex]);
                            }
                            else
                            {
                                if (i == 0)
                                    PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[wall0SouthPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[wall1SouthPrefabIndex]);
                                else
                                    PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[wall2SouthPrefabIndex]);
                                if (PlaceSouthBalcony(bldg, i, x, bldg.numberOfSouthBalconies, bldg.randomSeedOfSouthBalconies, balconiesSouthIndexes)) PlaceSouthWall(x, y, i, storyFolder, balconyPrefabs[balconySouthPrefabIndex]);
                            }
                        }
                        else
                        {
                            if (i == 0)
                                PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[wall0SouthPrefabIndex]);
                            else if (i % 2 == 1)
                                PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[wall1SouthPrefabIndex]);
                            else
                                PlaceSouthWall(x, y, i, storyFolder, wallPrefabs[wall2SouthPrefabIndex]);
                            if (PlaceSouthBalcony(bldg, i, x, bldg.numberOfSouthBalconies, bldg.randomSeedOfSouthBalconies, balconiesSouthIndexes)) PlaceSouthWall(x, y, i, storyFolder, balconyPrefabs[balconySouthPrefabIndex]);
                        }
                        if (IsEastOffsetCorrect(bldg, wing) && i < bldg.eastWallHeight)
                        {
                            if (x >= wing.Bounds.max.x - bldg.depthOffsetEastWall && x <= wing.Bounds.max.x + bldg.minOffsetEastWall)
                            {
                                if (i == 0)
                                    PlaceSouthWall(x + bldg.depthOffsetEastWall, -bldg.minOffsetEastWall, i, storyFolder, wallPrefabs[wall0EastOffsetPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceSouthWall(x + bldg.depthOffsetEastWall, -bldg.minOffsetEastWall, i, storyFolder, wallPrefabs[wall1EastOffsetPrefabIndex]);
                                else
                                    PlaceSouthWall(x + bldg.depthOffsetEastWall, -bldg.minOffsetEastWall, i, storyFolder, wallPrefabs[wall2EastOffsetPrefabIndex]);
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
                                PlaceFloor(x - (wing.Bounds.max.x - bldg.maxOffsetNorthWall - 1) + 1, y + bldg.depthOffsetNorthWall - 1, i, new int[3] { 0, 180, 0 }, storyFolder, floorPrefabs[floorNorthOffsetPrefabIndex]);

                            if (IsSouthOffsetCorrect(bldg, wing) && y >= wing.Bounds.max.y - bldg.depthOffsetSouthWall && y <= wing.Bounds.max.y + bldg.minOffsetSouthWall)
                                    PlaceFloor(x - (wing.Bounds.max.x - bldg.maxOffsetSouthWall - 1) + 1, y - wing.Bounds.max.y-1, i, new int[3] { 0, 180, 0 }, storyFolder, floorPrefabs[floorSouthOffsetPrefabIndex]);

                            if (IsEastOffsetCorrect(bldg, wing) && y >= wing.Bounds.min.y + bldg.minOffsetEastWall && y <= wing.Bounds.min.y + bldg.maxOffsetEastWall)
                            {
                                PlaceFloor(x + bldg.depthOffsetEastWall + 1, y - 1, i, new int[3] { 0, 180, 0 }, storyFolder, floorPrefabs[floorSouthOffsetPrefabIndex]);
                                if (entries.Contains(y))
                                {
                                    PlaceEastWall(x + bldg.depthOffsetEastWall, y, i, storyFolder, doorPrefabs[doorEastOffsetPrefabIndex]);
                                    PlaceStair(x + bldg.depthOffsetEastWall + 1, y - 1, i, storyFolder);
                                }
                            }
                            else
                            {
                                PlaceFloor(x + 1, y - 1, i, new int[3] { 0, 180, 0 }, storyFolder, floorPrefabs[floorEastPrefabIndex]);
                                if (entries.Contains(y))
                                {
                                    PlaceEastWall(x, y, i, storyFolder, doorPrefabs[doorEastPrefabIndex]);
                                    PlaceStair(x + 1, y - 1, i, storyFolder);
                                }
                            }
                        }
                        if (IsNorthOffsetCorrect(bldg, wing) && i < bldg.northWallHeight)
                        {
                            if (y >= wing.Bounds.max.y - bldg.depthOffsetNorthWall && y <= wing.Bounds.max.y + bldg.minOffsetNorthWall)
                            {
                                if (i == 0)
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetNorthWall - 1), y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[wall0NorthOffsetPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetNorthWall - 1), y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[wall1NorthOffsetPrefabIndex]);
                                else
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetNorthWall - 1), y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[wall2NorthOffsetPrefabIndex]);
                            }
                        }
                        if (IsSouthOffsetCorrect(bldg, wing) && i < bldg.southWallHeight)
                        {
                            if (y >= wing.Bounds.max.y - bldg.depthOffsetSouthWall && y <= wing.Bounds.max.y + bldg.minOffsetSouthWall)
                            {
                                if (i == 0)
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetSouthWall - 1), y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[wall0SouthOffsetPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetSouthWall - 1), y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[wall1SouthOffsetPrefabIndex]);
                                else
                                    PlaceEastWall(x - (wing.Bounds.max.x - bldg.maxOffsetSouthWall - 1), y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[wall2SouthOffsetPrefabIndex]);
                            }
                        }

                        if (IsEastOffsetCorrect(bldg, wing) && i < bldg.eastWallHeight)
                        {
                            if (y >= wing.Bounds.min.y + bldg.minOffsetEastWall && y <= wing.Bounds.min.y + bldg.maxOffsetEastWall)
                            {
                                if (i == 0)
                                    PlaceEastWall(x + bldg.depthOffsetEastWall, y, i, storyFolder, wallPrefabs[wall0EastOffsetPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceEastWall(x + bldg.depthOffsetEastWall, y, i, storyFolder, wallPrefabs[wall1EastOffsetPrefabIndex]);
                                else 
                                    PlaceEastWall(x + bldg.depthOffsetEastWall, y, i, storyFolder, wallPrefabs[wall2EastOffsetPrefabIndex]);
                            }
                            else
                            {
                                if (i == 0)
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[wall0EastPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[wall1EastPrefabIndex]);
                                else
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[wall2EastPrefabIndex]);
                            }
                        }
                        else
                        {
                            if (i ==0 && entries.Contains(y)) PlaceEastWall(x, y, i, storyFolder, wallDoorsPrefabs[wallDoorPrefabIndex]);
                            else
                            {
                                if (i == 0)
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[wall0EastPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[wall1EastPrefabIndex]);
                                else
                                    PlaceEastWall(x, y, i, storyFolder, wallPrefabs[wall2EastPrefabIndex]);
                                if (PlaceEastBalcony(bldg, i, y, bldg.numberOfEastBalconies, bldg.randomSeedOfEastBalconies, balconiesEastIndexes)) PlaceEastWall(x, y, i, storyFolder, balconyPrefabs[balconyEastPrefabIndex]);
                            }
                        }
                    }

                    //north wall
                    if (y == wing.Bounds.min.y + wing.Bounds.size.y - 1)
                    {
                        if (i == 0)
                        {
                            if (IsNorthOffsetCorrect(bldg, wing) &&  x >= bldg.minOffsetNorthWall && x <= bldg.maxOffsetNorthWall)
                                PlaceFloor(x + 1, y + bldg.depthOffsetNorthWall, i, new int[3] { 0, 90, 0 }, storyFolder, floorPrefabs[floorNorthOffsetPrefabIndex]);
                            else
                                PlaceFloor(x + 1, y, i, new int[3] { 0, 90, 0 }, storyFolder, floorPrefabs[floorNorthPrefabIndex]);
                            if (IsEastOffsetCorrect(bldg, wing) && i < bldg.eastWallHeight)
                            {
                                if (x >= wing.Bounds.max.x - bldg.depthOffsetEastWall && x <= wing.Bounds.max.x + bldg.minOffsetEastWall)
                                    PlaceFloor(x + bldg.depthOffsetEastWall + 1, bldg.maxOffsetEastWall, i, new int[3] { 0, 90, 0 }, storyFolder, floorPrefabs[floorSouthOffsetPrefabIndex]);
                            }
                        }
                        if (IsNorthOffsetCorrect(bldg, wing) && i < bldg.northWallHeight)
                        {
                            if (x >= bldg.minOffsetNorthWall && x <= bldg.maxOffsetNorthWall)
                            {
                                if (i == 0)
                                    PlaceNorthWall(x, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[wall0NorthOffsetPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceNorthWall(x, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[wall1NorthOffsetPrefabIndex]);
                                else
                                    PlaceNorthWall(x, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[wall2NorthOffsetPrefabIndex]);
                            }
                            else
                            {
                                if (i == 0)
                                    PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[wall0NorthPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[wall1NorthPrefabIndex]);
                                else PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[wall2NorthPrefabIndex]);
                                if (PlaceNorthBalcony(bldg, i, x, bldg.numberOfNorthBalconies, bldg.randomSeedOfNorthBalconies, balconiesNorthIndexes)) PlaceNorthWall(x, y, i, storyFolder, balconyPrefabs[balconyNorthPrefabIndex]);
                            }
                        }
                        else
                        {
                            if (i == 0)
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[wall0NorthPrefabIndex]);
                            else if (i % 2 == 1)
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[wall1NorthPrefabIndex]);
                            else
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[wall2NorthPrefabIndex]);
                            if (PlaceNorthBalcony(bldg, i, x, bldg.numberOfNorthBalconies, bldg.randomSeedOfNorthBalconies, balconiesNorthIndexes)) PlaceNorthWall(x, y, i, storyFolder, balconyPrefabs[balconyNorthPrefabIndex]);
                        }
                        if (IsEastOffsetCorrect(bldg, wing) && i < bldg.eastWallHeight)
                        {
                            if (x >= wing.Bounds.max.x - bldg.depthOffsetEastWall && x <= wing.Bounds.max.x + bldg.minOffsetEastWall)
                            {
                                if (i == 0)
                                    PlaceNorthWall(x + bldg.depthOffsetEastWall, bldg.maxOffsetEastWall, i, storyFolder, wallPrefabs[wall0EastOffsetPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceNorthWall(x + bldg.depthOffsetEastWall, bldg.maxOffsetEastWall, i, storyFolder, wallPrefabs[wall1EastOffsetPrefabIndex]);
                                else
                                    PlaceNorthWall(x + bldg.depthOffsetEastWall, bldg.maxOffsetEastWall, i, storyFolder, wallPrefabs[wall2EastOffsetPrefabIndex]);
                            }
                        }
                        else
                        {
                            if (i == 0)
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[wall0NorthPrefabIndex]);
                            else if (i % 2 == 1)
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[wall1NorthPrefabIndex]);
                            else
                                PlaceNorthWall(x, y, i, storyFolder, wallPrefabs[wall2NorthPrefabIndex]);
                            if (PlaceNorthBalcony(bldg, i, x, bldg.numberOfNorthBalconies, bldg.randomSeedOfNorthBalconies, balconiesNorthIndexes)) PlaceNorthWall(x, y, i, storyFolder, balconyPrefabs[balconyNorthPrefabIndex]);
                        }

                    }

                    //west wall
                    if (x == wing.Bounds.min.x)
                    {
                        if (i == 0)
                        {
                            if (IsNorthOffsetCorrect(bldg, wing) && y >= wing.Bounds.max.y - bldg.depthOffsetNorthWall && y <= wing.Bounds.max.y + bldg.minOffsetNorthWall)
                                PlaceFloor(x + bldg.minOffsetNorthWall, y + bldg.depthOffsetNorthWall, i, new int[3] { 0, 0, 0 }, storyFolder, floorPrefabs[floorNorthOffsetPrefabIndex]);
                            if (IsSouthOffsetCorrect(bldg, wing) && y >= wing.Bounds.max.y - bldg.depthOffsetSouthWall && y <= wing.Bounds.max.y + bldg.minOffsetSouthWall)
                                PlaceFloor(x + bldg.minOffsetSouthWall, y - wing.Bounds.max.y, i, new int[3] { 0, 0, 0 }, storyFolder, floorPrefabs[floorSouthOffsetPrefabIndex]);
                            PlaceFloor(x, y, i, new int[3] { 0, 0, 0 }, storyFolder, floorPrefabs[floorWestPrefabIndex]);
                        }
                        if (PlaceWestBalcony(bldg, i, y, bldg.numberOfWestBalconies, bldg.randomSeedOfWestBalconies, balconiesWestIndexes)) PlaceWestWall(x, y, i, storyFolder, balconyPrefabs[balconyWestPrefabIndex]);
                        if (i == 0)
                            PlaceWestWall(x, y, i, storyFolder, wallPrefabs[wall0WestPrefabIndex]);
                        else if (i % 2 == 1)
                            PlaceWestWall(x, y, i, storyFolder, wallPrefabs[wall1WestPrefabIndex]);
                        else PlaceWestWall(x, y, i, storyFolder, wallPrefabs[wall2WestPrefabIndex]);
                        if (IsNorthOffsetCorrect(bldg, wing) && i < bldg.northWallHeight)
                        {
                            if (y >= wing.Bounds.max.y - bldg.depthOffsetNorthWall && y <= wing.Bounds.max.y + bldg.minOffsetNorthWall)
                            {
                                if (i == 0)
                                    PlaceWestWall(x + bldg.minOffsetNorthWall, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[wall0NorthOffsetPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceWestWall(x + bldg.minOffsetNorthWall, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[wall1NorthOffsetPrefabIndex]);
                                else PlaceWestWall(x + bldg.minOffsetNorthWall, y + bldg.depthOffsetNorthWall, i, storyFolder, wallPrefabs[wall2NorthOffsetPrefabIndex]);
                            }
                        }
                        if (IsSouthOffsetCorrect(bldg, wing) && i < bldg.southWallHeight)
                        {
                            if (y >= wing.Bounds.max.y - bldg.depthOffsetSouthWall && y <= wing.Bounds.max.y + bldg.minOffsetSouthWall)
                            {
                                if (i == 0)
                                    PlaceWestWall(x + bldg.minOffsetSouthWall, y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[wall0SouthOffsetPrefabIndex]);
                                else if (i % 2 == 1)
                                    PlaceWestWall(x + bldg.minOffsetSouthWall, y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[wall1SouthOffsetPrefabIndex]);
                                else PlaceWestWall(x + bldg.minOffsetSouthWall, y - wing.Bounds.max.y, i, storyFolder, wallPrefabs[wall2SouthOffsetPrefabIndex]);
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
        if (bldg.maxOffsetEastWall > wing.Bounds.max.y) bldg.maxOffsetEastWall = wing.Bounds.max.y-1;
        return bldg.northWallHeight>0 &&  bldg.depthOffsetEastWall > 0 && bldg.minOffsetEastWall >= 0 && bldg.minOffsetEastWall <= wing.Bounds.max.y &&
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
            for (int i = 1; i < bldg.level; i++)
            {
                for (int j = 0; j < numberOfBalconies; j++)
                    balconiesSouthIndexes.Add(new Tuple<int, int>(i, j * balconySpacing));
            }
        }
        else
        {
            for (int i = 0; i < numberOfBalconies; i++)
            {
                balconiesSouthIndexes.Add(new Tuple<int, int>(rand.Next(2, 2+randomSeedOfBalconies), rand.Next(0, 2 + randomSeedOfBalconies)));
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
            for (int i = 1; i < bldg.level; i++)
            {
                for (int j = 0; j < numberOfBalconies; j++)
                    balconiesNorthIndexes.Add(new Tuple<int, int>(i, j * balconySpacing));
            }
        }
        else
        {
            for (int i = 0; i < numberOfBalconies; i++)
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
            for (int i = 1; i < bldg.level; i++)
            {
                for (int j = 0; j < numberOfBalconies; j++)
                    balconiesWestIndexes.Add(new Tuple<int, int>(i, j * balconySpacing));
            }
        }
        else
        {
            for (int i = 0; i < numberOfBalconies; i++)
            {
                balconiesWestIndexes.Add(new Tuple<int, int>(rand.Next(2, 2+randomSeedOfBalconies), rand.Next(0, 2+randomSeedOfBalconies)));
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
            for (int i = 1; i < bldg.level; i++)
            {
                for (int j = 0; j < numberOfBalconies; j++)
                    balconiesEastIndexes.Add(new Tuple<int, int>(i, j * balconySpacing));
            }
        }
        else
        {
            for (int i = 0; i < numberOfBalconies; i++)
            {
                balconiesEastIndexes.Add(new Tuple<int, int>(rand.Next(2, 2 + randomSeedOfBalconies), rand.Next(0, 2 + randomSeedOfBalconies)));
            }
        }
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
    private void PlaceStair(int x, int y, int level, Transform storyFolder)
    {
        var stairSize = GetPrefabSize(stairPrefabs[stairSouthPrefabIndex]);
        Transform f = Instantiate(stairPrefabs[stairSouthPrefabIndex], storyFolder.TransformPoint(new Vector3(x * -3f, 0f + level * 2.5f, y * -3f - 3f)), Quaternion.identity);
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
    }

    private void RenderRoof(Wing wing, int minX, int maxX, int minY, int maxY, Transform wingFolder, int level)
    {
        var direction = new RoofDirection();
        Transform prefab;
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if ((y == minY) && (x == minX))
                {
                    prefab = roofCornerPrefabs[roofCornerPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if ((y == minY) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[roofCornerPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if ((y == maxY - 1) && (x == minX))
                {
                    prefab = roofCornerPrefabs[roofCornerPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if ((y == maxY - 1) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[roofCornerPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                if (y == minY && !(y == maxY - 1 || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofBoundPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if (y == maxY - 1 && !(y == minY || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofBoundPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if (x == minX && !(y == minY || y == maxY - 1 || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofBoundPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if (x == maxX - 1 && !(y == minY || y == maxY - 1 || x == minX))
                {
                    prefab = roofBoundPrefabs[roofBoundPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                {
                    prefab = roofPrefabs[roofPrefabIndex];
                    direction = RoofDirection.North;
                }

                PlaceRoof(x, y, level, wingFolder, wing.GetRoof.Type, direction, prefab);
            }
        }
    }

    private void RenderNorthOffsetRoof(Wing wing, int minX, int maxX, int minY, int maxY, Transform wingFolder, int level)
    {
        var direction = new RoofDirection();
        Transform prefab;
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if ((y == minY) && (x == minX))
                {
                    prefab = roofCornerPrefabs[roofNorthOffsetCornerPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if ((y == minY) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[roofNorthOffsetCornerPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if ((y == maxY - 1) && (x == minX))
                {
                    prefab = roofCornerPrefabs[roofNorthOffsetCornerPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if ((y == maxY - 1) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[roofNorthOffsetCornerPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                if (y == minY && !(y == maxY - 1 || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofNorthOffsetBoundPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if (y == maxY - 1 && !(y == minY || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofNorthOffsetBoundPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if (x == minX && !(y == minY || y == maxY - 1 || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofNorthOffsetBoundPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if (x == maxX - 1 && !(y == minY || y == maxY - 1 || x == minX))
                {
                    prefab = roofBoundPrefabs[roofNorthOffsetBoundPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                {
                    prefab = roofPrefabs[roofNorthOffsetPrefabIndex];
                    direction = RoofDirection.North;
                }

                PlaceRoof(x, y, level, wingFolder, wing.GetRoof.Type, direction, prefab);
            }
        }
    }

    private void RenderSouthOffsetRoof(Wing wing, int minX, int maxX, int minY, int maxY, Transform wingFolder, int level)
    {
        var direction = new RoofDirection();
        Transform prefab;
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if ((y == minY) && (x == minX))
                {
                    prefab = roofCornerPrefabs[roofSouthOffsetCornerPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if ((y == minY) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[roofSouthOffsetCornerPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if ((y == maxY - 1) && (x == minX))
                {
                    prefab = roofCornerPrefabs[roofSouthOffsetCornerPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if ((y == maxY - 1) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[roofSouthOffsetCornerPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                if (y == minY && !(y == maxY - 1 || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofSouthOffsetBoundPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if (y == maxY - 1 && !(y == minY || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofSouthOffsetBoundPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if (x == minX && !(y == minY || y == maxY - 1 || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofSouthOffsetBoundPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if (x == maxX - 1 && !(y == minY || y == maxY - 1 || x == minX))
                {
                    prefab = roofBoundPrefabs[roofSouthOffsetBoundPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                {
                    prefab = roofPrefabs[roofSouthOffsetPrefabIndex];
                    direction = RoofDirection.North;
                }

                PlaceRoof(x, y, level, wingFolder, wing.GetRoof.Type, direction, prefab);
            }
        }
    }
    private void RenderEastOffsetRoof(Wing wing, int minX, int maxX, int minY, int maxY, Transform wingFolder, int level)
    {
        var direction = new RoofDirection();
        Transform prefab;
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if ((y == minY) && (x == minX))
                {
                    prefab = roofCornerPrefabs[roofEastOffsetCornerPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if ((y == minY) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[roofEastOffsetCornerPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if ((y == maxY - 1) && (x == minX))
                {
                    prefab = roofCornerPrefabs[roofEastOffsetCornerPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if ((y == maxY - 1) && (x == maxX - 1))
                {
                    prefab = roofCornerPrefabs[roofEastOffsetCornerPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                if (y == minY && !(y == maxY - 1 || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofEastOffsetBoundPrefabIndex];
                    direction = RoofDirection.East;
                }
                else
                if (y == maxY - 1 && !(y == minY || x == minX || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofEastOffsetBoundPrefabIndex];
                    direction = RoofDirection.West;
                }
                else
                if (x == minX && !(y == minY || y == maxY - 1 || x == maxX - 1))
                {
                    prefab = roofBoundPrefabs[roofEastOffsetBoundPrefabIndex];
                    direction = RoofDirection.South;
                }
                else
                if (x == maxX - 1 && !(y == minY || y == maxY - 1 || x == minX))
                {
                    prefab = roofBoundPrefabs[roofEastOffsetBoundPrefabIndex];
                    direction = RoofDirection.North;
                }
                else
                {
                    prefab = roofPrefabs[roofEastOffsetPrefabIndex];
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
    [HideInInspector] public int floorSouthPrefabIndex = 0;
    [HideInInspector] public int wall0SouthPrefabIndex = 0;
    [HideInInspector] public int wall1SouthPrefabIndex = 0;
    [HideInInspector] public int wall2SouthPrefabIndex = 0;
    [HideInInspector] public int doorSouthPrefabIndex = 0;
    [HideInInspector] public int roofPrefabIndex = 0;
    [HideInInspector] public int roofBoundPrefabIndex = 0;
    [HideInInspector] public int roofCornerPrefabIndex = 0;
    [HideInInspector] public int stairSouthPrefabIndex = 0;
    [HideInInspector] public int balconySouthPrefabIndex = 0;

    [HideInInspector] public int savedSeedOfSouthBalconies;
    [HideInInspector] public int savedNumberOfSouthBalconies;

    [HideInInspector] public int floorNorthPrefabIndex = 0;
    [HideInInspector] public int wall0NorthPrefabIndex = 0;
    [HideInInspector] public int wall1NorthPrefabIndex = 0;
    [HideInInspector] public int wall2NorthPrefabIndex = 0;
    [HideInInspector] public int doorNorthPrefabIndex = 0;
    [HideInInspector] public int stairNorthPrefabIndex = 0;
    [HideInInspector] public int balconyNorthPrefabIndex = 0;

    [HideInInspector] public int savedSeedOfNorthBalconies;
    [HideInInspector] public int savedNumberOfNorthBalconies;

    [HideInInspector] public int floorEastPrefabIndex = 0;
    [HideInInspector] public int wall0EastPrefabIndex = 0;
    [HideInInspector] public int wall1EastPrefabIndex = 0;
    [HideInInspector] public int wall2EastPrefabIndex = 0;
    [HideInInspector] public int wallDoorPrefabIndex = 0;
    [HideInInspector] public int doorEastPrefabIndex = 0;
    [HideInInspector] public int stairEastPrefabIndex = 0;
    [HideInInspector] public int balconyEastPrefabIndex = 0;

    [HideInInspector] public int savedSeedOfEastBalconies;
    [HideInInspector] public int savedNumberOfEastBalconies;

    [HideInInspector] public int floorWestPrefabIndex = 0;
    [HideInInspector] public int wall0WestPrefabIndex = 0;
    [HideInInspector] public int wall1WestPrefabIndex = 0;
    [HideInInspector] public int wall2WestPrefabIndex = 0;
    [HideInInspector] public int doorWestPrefabIndex = 0;
    [HideInInspector] public int stairWestPrefabIndex = 0;
    [HideInInspector] public int balconyWestPrefabIndex = 0;

    [HideInInspector] public int savedSeedOfWestBalconies;
    [HideInInspector] public int savedNumberOfWestBalconies;

    [HideInInspector] public int floorSouthOffsetPrefabIndex = 0;
    [HideInInspector] public int wall0SouthOffsetPrefabIndex = 0;
    [HideInInspector] public int wall1SouthOffsetPrefabIndex = 0;
    [HideInInspector] public int wall2SouthOffsetPrefabIndex = 0;
    [HideInInspector] public int doorSouthOffsetPrefabIndex = 0;
    [HideInInspector] public int roofSouthOffsetPrefabIndex = 0;
    [HideInInspector] public int roofSouthOffsetBoundPrefabIndex = 0;
    [HideInInspector] public int roofSouthOffsetCornerPrefabIndex = 0;
    [HideInInspector] public int stairSouthOffsetPrefabIndex = 0;
    [HideInInspector] public int balconySouthOffsetPrefabIndex = 0;


    [HideInInspector] public int floorNorthOffsetPrefabIndex = 0;
    [HideInInspector] public int wall0NorthOffsetPrefabIndex = 0;
    [HideInInspector] public int wall1NorthOffsetPrefabIndex = 0;
    [HideInInspector] public int wall2NorthOffsetPrefabIndex = 0;
    [HideInInspector] public int doorNorthOffsetPrefabIndex = 0;
    [HideInInspector] public int roofNorthOffsetPrefabIndex = 0;
    [HideInInspector] public int roofNorthOffsetBoundPrefabIndex = 0;
    [HideInInspector] public int roofNorthOffsetCornerPrefabIndex = 0;
    [HideInInspector] public int stairNorthOffsetPrefabIndex = 0;
    [HideInInspector] public int balconyNorthOffsetPrefabIndex = 0;

    [HideInInspector] public int floorEastOffsetPrefabIndex = 0;
    [HideInInspector] public int wall0EastOffsetPrefabIndex = 0;
    [HideInInspector] public int wall1EastOffsetPrefabIndex = 0;
    [HideInInspector] public int wall2EastOffsetPrefabIndex = 0;
    [HideInInspector] public int doorEastOffsetPrefabIndex = 0;
    [HideInInspector] public int roofEastOffsetPrefabIndex = 0;
    [HideInInspector] public int roofEastOffsetBoundPrefabIndex = 0;
    [HideInInspector] public int roofEastOffsetCornerPrefabIndex = 0;
    [HideInInspector] public int stairEastOffsetPrefabIndex = 0;
    [HideInInspector] public int balconyEastOffsetPrefabIndex = 0;
}
