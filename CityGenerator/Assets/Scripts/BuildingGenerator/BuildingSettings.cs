using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class BuildingSettings
{
    [Range(1, 50)]
    public int x;

    [Range(1, 50)]
    public int y;
    public Vector2Int buildingSize => new Vector2Int(x, y);

    public int buildingCount;
    [Range(0, 50)]
    public int numberOfLevels;
    public BuildingType type;

    [Range(0, 50)]
    public int numberOfEntries;

    [Range(0, 50)]
    public int offsetOfEntries;

    [Range(0, 50)]
    public int MaxNumberOfSouthBalconies;

    [Range(0, 50)]
    public int randomSeedOfSouthBalconies;

    [Range(0, 50)]
    public int MaxNumberOfNorthBalconies;

    [Range(0, 50)]
    public int randomSeedOfNorthBalconies;

    [Range(0, 50)]
    public int MaxNumberOfWestBalconies;

    [Range(0, 50)]
    public int randomSeedOfWestBalconies;

    [Range(0, 50)]
    public int MaxNumberOfEastBalconies;

    [Range(0, 50)]
    public int randomSeedOfEastBalconies;

    [Range(0, 50)]
    public int randomSeedOfRoofElements;


    public int minOffsetNorthWall;

    public int maxOffsetNorthWall;

    public int depthOffsetNorthWall;

    public int northWallHeight;



    public int minOffsetSouthWall;

    public int maxOffsetSouthWall;

    public int depthOffsetSouthWall;

    public int southWallHeight;


    public int minOffsetEastWall;

    public int maxOffsetEastWall;

    public int depthOffsetEastWall;

    public int eastWallHeight;
    public List<Tuple<int, int>> balconyLocations = new List<Tuple<int, int>>();
    public List<Vector3Int> entryLocations = new List<Vector3Int>();

    [HideInInspector] public int floorSouthPrefabIndex = 0;
    [HideInInspector] public int wall0SouthPrefabIndex = 0;
    [HideInInspector] public int wall1SouthPrefabIndex = 0;
    [HideInInspector] public int wall2SouthPrefabIndex = 0;
    [HideInInspector] public int doorSouthPrefabIndex = 0;
    [HideInInspector] public int roofPrefabIndex = 0;
    [HideInInspector] public int roofBoundPrefabIndex = 0;
    [HideInInspector] public int roofCornerPrefabIndex = 0;
    [HideInInspector] public int roofElementPrefabIndex = 0;
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


    public BuildingSettings(Vector2Int size, int count, int numberOfLevels, BuildingType type, int numberOfEntries, int offsetOfEntries, int numberOfBalconies, int randomSeedOfBalconies,
        int randomSeedOfRoofElements,
        int minOffsetNorthWall, int maxOffsetNorthWall, int depthOffsetNorthWall, int northWallHeight,
        int minOffsetSouthWall, int maxOffsetSouthWall, int depthOffsetSouthWall, int southWallHeight)
    {
        x = size.x;
        y = size.y;
        buildingCount = count;
        this.numberOfLevels = numberOfLevels;
        this.type = type;
        this.numberOfEntries = numberOfEntries;
        this.offsetOfEntries = offsetOfEntries;
        this.MaxNumberOfSouthBalconies = numberOfBalconies;
        this.randomSeedOfSouthBalconies = randomSeedOfBalconies;
        this.randomSeedOfRoofElements = randomSeedOfRoofElements;
        this.minOffsetNorthWall = minOffsetNorthWall;
        this.maxOffsetNorthWall = maxOffsetNorthWall;
        this.depthOffsetNorthWall = depthOffsetNorthWall;
        this.northWallHeight = northWallHeight;

        this.minOffsetSouthWall = minOffsetSouthWall;
        this.maxOffsetSouthWall = maxOffsetSouthWall;
        this.depthOffsetSouthWall = depthOffsetSouthWall;
        this.southWallHeight = southWallHeight;
    }

    public BuildingSettings(int x, int y, int buildingCount, int numberOfLevels, BuildingType type, int numberOfEntries, int offsetOfEntries, int maxNumberOfSouthBalconies, int randomSeedOfSouthBalconies, int maxNumberOfNorthBalconies, int randomSeedOfNorthBalconies, int maxNumberOfWestBalconies, int randomSeedOfWestBalconies, int maxNumberOfEastBalconies, int randomSeedOfEastBalconies, int minOffsetNorthWall, int maxOffsetNorthWall, int depthOffsetNorthWall, int northWallHeight, int minOffsetSouthWall, int maxOffsetSouthWall, int depthOffsetSouthWall, int southWallHeight, int minOffsetEastWall, int maxOffsetEastWall, int depthOffsetEastWall, int eastWallHeight, List<Tuple<int, int>> balconyLocations, List<Vector3Int> entryLocations, int floorSouthPrefabIndex, int wall0SouthPrefabIndex, int wall1SouthPrefabIndex, int wall2SouthPrefabIndex, int doorSouthPrefabIndex, int roofPrefabIndex, int roofBoundPrefabIndex, int roofCornerPrefabIndex, int stairSouthPrefabIndex, int balconySouthPrefabIndex, int savedSeedOfSouthBalconies, int savedNumberOfSouthBalconies, int floorNorthPrefabIndex, int wall0NorthPrefabIndex, int wall1NorthPrefabIndex, int wall2NorthPrefabIndex, int doorNorthPrefabIndex, int stairNorthPrefabIndex, int balconyNorthPrefabIndex, int savedSeedOfNorthBalconies, int savedNumberOfNorthBalconies, int floorEastPrefabIndex, int wall0EastPrefabIndex, int wall1EastPrefabIndex, int wall2EastPrefabIndex, int wallDoorPrefabIndex, int doorEastPrefabIndex, int stairEastPrefabIndex, int balconyEastPrefabIndex, int savedSeedOfEastBalconies, int savedNumberOfEastBalconies, int floorWestPrefabIndex, int wall0WestPrefabIndex, int wall1WestPrefabIndex, int wall2WestPrefabIndex, int doorWestPrefabIndex, int stairWestPrefabIndex, int balconyWestPrefabIndex, int savedSeedOfWestBalconies, int savedNumberOfWestBalconies, int floorSouthOffsetPrefabIndex, int wall0SouthOffsetPrefabIndex, int wall1SouthOffsetPrefabIndex, int wall2SouthOffsetPrefabIndex, int doorSouthOffsetPrefabIndex, int roofSouthOffsetPrefabIndex, int roofSouthOffsetBoundPrefabIndex, int roofSouthOffsetCornerPrefabIndex, int stairSouthOffsetPrefabIndex, int balconySouthOffsetPrefabIndex, int floorNorthOffsetPrefabIndex, int wall0NorthOffsetPrefabIndex, int wall1NorthOffsetPrefabIndex, int wall2NorthOffsetPrefabIndex, int doorNorthOffsetPrefabIndex, int roofNorthOffsetPrefabIndex, int roofNorthOffsetBoundPrefabIndex, int roofNorthOffsetCornerPrefabIndex, int stairNorthOffsetPrefabIndex, int balconyNorthOffsetPrefabIndex, int floorEastOffsetPrefabIndex, int wall0EastOffsetPrefabIndex, int wall1EastOffsetPrefabIndex, int wall2EastOffsetPrefabIndex, int doorEastOffsetPrefabIndex, int roofEastOffsetPrefabIndex, int roofEastOffsetBoundPrefabIndex, int roofEastOffsetCornerPrefabIndex, int stairEastOffsetPrefabIndex, int balconyEastOffsetPrefabIndex)
    {
        this.x = x;
        this.y = y;
        this.buildingCount = buildingCount;
        this.numberOfLevels = numberOfLevels;
        this.type = type;
        this.numberOfEntries = numberOfEntries;
        this.offsetOfEntries = offsetOfEntries;
        MaxNumberOfSouthBalconies = maxNumberOfSouthBalconies;
        this.randomSeedOfSouthBalconies = randomSeedOfSouthBalconies;
        MaxNumberOfNorthBalconies = maxNumberOfNorthBalconies;
        this.randomSeedOfNorthBalconies = randomSeedOfNorthBalconies;
        MaxNumberOfWestBalconies = maxNumberOfWestBalconies;
        this.randomSeedOfWestBalconies = randomSeedOfWestBalconies;
        MaxNumberOfEastBalconies = maxNumberOfEastBalconies;
        this.randomSeedOfEastBalconies = randomSeedOfEastBalconies;
        this.minOffsetNorthWall = minOffsetNorthWall;
        this.maxOffsetNorthWall = maxOffsetNorthWall;
        this.depthOffsetNorthWall = depthOffsetNorthWall;
        this.northWallHeight = northWallHeight;
        this.minOffsetSouthWall = minOffsetSouthWall;
        this.maxOffsetSouthWall = maxOffsetSouthWall;
        this.depthOffsetSouthWall = depthOffsetSouthWall;
        this.southWallHeight = southWallHeight;
        this.minOffsetEastWall = minOffsetEastWall;
        this.maxOffsetEastWall = maxOffsetEastWall;
        this.depthOffsetEastWall = depthOffsetEastWall;
        this.eastWallHeight = eastWallHeight;
        this.balconyLocations = balconyLocations;
        this.entryLocations = entryLocations;
        this.floorSouthPrefabIndex = floorSouthPrefabIndex;
        this.wall0SouthPrefabIndex = wall0SouthPrefabIndex;
        this.wall1SouthPrefabIndex = wall1SouthPrefabIndex;
        this.wall2SouthPrefabIndex = wall2SouthPrefabIndex;
        this.doorSouthPrefabIndex = doorSouthPrefabIndex;
        this.roofPrefabIndex = roofPrefabIndex;
        this.roofBoundPrefabIndex = roofBoundPrefabIndex;
        this.roofCornerPrefabIndex = roofCornerPrefabIndex;
        this.roofElementPrefabIndex = roofElementPrefabIndex;
        this.stairSouthPrefabIndex = stairSouthPrefabIndex;
        this.balconySouthPrefabIndex = balconySouthPrefabIndex;
        this.savedSeedOfSouthBalconies = savedSeedOfSouthBalconies;
        this.savedNumberOfSouthBalconies = savedNumberOfSouthBalconies;
        this.floorNorthPrefabIndex = floorNorthPrefabIndex;
        this.wall0NorthPrefabIndex = wall0NorthPrefabIndex;
        this.wall1NorthPrefabIndex = wall1NorthPrefabIndex;
        this.wall2NorthPrefabIndex = wall2NorthPrefabIndex;
        this.doorNorthPrefabIndex = doorNorthPrefabIndex;
        this.stairNorthPrefabIndex = stairNorthPrefabIndex;
        this.balconyNorthPrefabIndex = balconyNorthPrefabIndex;
        this.savedSeedOfNorthBalconies = savedSeedOfNorthBalconies;
        this.savedNumberOfNorthBalconies = savedNumberOfNorthBalconies;
        this.floorEastPrefabIndex = floorEastPrefabIndex;
        this.wall0EastPrefabIndex = wall0EastPrefabIndex;
        this.wall1EastPrefabIndex = wall1EastPrefabIndex;
        this.wall2EastPrefabIndex = wall2EastPrefabIndex;
        this.wallDoorPrefabIndex = wallDoorPrefabIndex;
        this.doorEastPrefabIndex = doorEastPrefabIndex;
        this.stairEastPrefabIndex = stairEastPrefabIndex;
        this.balconyEastPrefabIndex = balconyEastPrefabIndex;
        this.savedSeedOfEastBalconies = savedSeedOfEastBalconies;
        this.savedNumberOfEastBalconies = savedNumberOfEastBalconies;
        this.floorWestPrefabIndex = floorWestPrefabIndex;
        this.wall0WestPrefabIndex = wall0WestPrefabIndex;
        this.wall1WestPrefabIndex = wall1WestPrefabIndex;
        this.wall2WestPrefabIndex = wall2WestPrefabIndex;
        this.doorWestPrefabIndex = doorWestPrefabIndex;
        this.stairWestPrefabIndex = stairWestPrefabIndex;
        this.balconyWestPrefabIndex = balconyWestPrefabIndex;
        this.savedSeedOfWestBalconies = savedSeedOfWestBalconies;
        this.savedNumberOfWestBalconies = savedNumberOfWestBalconies;
        this.floorSouthOffsetPrefabIndex = floorSouthOffsetPrefabIndex;
        this.wall0SouthOffsetPrefabIndex = wall0SouthOffsetPrefabIndex;
        this.wall1SouthOffsetPrefabIndex = wall1SouthOffsetPrefabIndex;
        this.wall2SouthOffsetPrefabIndex = wall2SouthOffsetPrefabIndex;
        this.doorSouthOffsetPrefabIndex = doorSouthOffsetPrefabIndex;
        this.roofSouthOffsetPrefabIndex = roofSouthOffsetPrefabIndex;
        this.roofSouthOffsetBoundPrefabIndex = roofSouthOffsetBoundPrefabIndex;
        this.roofSouthOffsetCornerPrefabIndex = roofSouthOffsetCornerPrefabIndex;
        this.stairSouthOffsetPrefabIndex = stairSouthOffsetPrefabIndex;
        this.balconySouthOffsetPrefabIndex = balconySouthOffsetPrefabIndex;
        this.floorNorthOffsetPrefabIndex = floorNorthOffsetPrefabIndex;
        this.wall0NorthOffsetPrefabIndex = wall0NorthOffsetPrefabIndex;
        this.wall1NorthOffsetPrefabIndex = wall1NorthOffsetPrefabIndex;
        this.wall2NorthOffsetPrefabIndex = wall2NorthOffsetPrefabIndex;
        this.doorNorthOffsetPrefabIndex = doorNorthOffsetPrefabIndex;
        this.roofNorthOffsetPrefabIndex = roofNorthOffsetPrefabIndex;
        this.roofNorthOffsetBoundPrefabIndex = roofNorthOffsetBoundPrefabIndex;
        this.roofNorthOffsetCornerPrefabIndex = roofNorthOffsetCornerPrefabIndex;
        this.stairNorthOffsetPrefabIndex = stairNorthOffsetPrefabIndex;
        this.balconyNorthOffsetPrefabIndex = balconyNorthOffsetPrefabIndex;
        this.floorEastOffsetPrefabIndex = floorEastOffsetPrefabIndex;
        this.wall0EastOffsetPrefabIndex = wall0EastOffsetPrefabIndex;
        this.wall1EastOffsetPrefabIndex = wall1EastOffsetPrefabIndex;
        this.wall2EastOffsetPrefabIndex = wall2EastOffsetPrefabIndex;
        this.doorEastOffsetPrefabIndex = doorEastOffsetPrefabIndex;
        this.roofEastOffsetPrefabIndex = roofEastOffsetPrefabIndex;
        this.roofEastOffsetBoundPrefabIndex = roofEastOffsetBoundPrefabIndex;
        this.roofEastOffsetCornerPrefabIndex = roofEastOffsetCornerPrefabIndex;
        this.stairEastOffsetPrefabIndex = stairEastOffsetPrefabIndex;
        this.balconyEastOffsetPrefabIndex = balconyEastOffsetPrefabIndex;
    }
}