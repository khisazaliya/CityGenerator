using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building 
{
    Vector2Int size;
    Wing[] wings;
    BuildingType type;

    public int numberOfEntries;

    public int offsetOfEntries;

    public int numberOfSouthBalconies;
    public int randomSeedOfSouthBalconies;

    public int numberOfNorthBalconies;
    public int randomSeedOfNorthBalconies;

    public int numberOfWestBalconies;
    public int randomSeedOfWestBalconies;

    public int numberOfEastBalconies;
    public int randomSeedOfEastBalconies;

    public List<Tuple<int, int>> balconyLocations = new List<Tuple<int, int>>();
    public List<Vector3Int> entryLocations = new List<Vector3Int>();

    public int level;
    public Vector2Int Size { get { return size; } }
    public Wing[] Wings { get { return wings; } }

    public BuildingType Type { get => type; }

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
    public Building(int numberOfLevels, int sizeX, int sizeY, Wing[] wings, int numberOfEntries, int offsetOfEntries, int numberOfSouthBalconies, int randomSeedOfSouthBalconies,
        int numberOfNorthBalconies, int randomSeedOfNorthBalconies,
        int numberOfWestBalconies, int randomSeedOfWestBalconies,
        int numberOfEastBalconies, int randomSeedOfEastBalconies, 
        int minOffsetNorthWall, int maxOffsetNorthWall, int depthOffsetNorthWall, int northWallHeight, 
        int minOffsetSouthWall, int maxOffsetSouthWall, int depthOffsetSouthWall, int southWallHeight,
        int minOffsetEastWall, int maxOffsetEastWall, int depthOffsetEastWall, int eastWallHeight
        ) {
        level = numberOfLevels;
        size = new Vector2Int(sizeX, sizeY);
        this.wings = wings;
        this.numberOfEntries = numberOfEntries;
        this.offsetOfEntries = offsetOfEntries;
        this.numberOfSouthBalconies = numberOfSouthBalconies;
        this.randomSeedOfSouthBalconies = randomSeedOfSouthBalconies;
        this.numberOfNorthBalconies = numberOfNorthBalconies;
        this.randomSeedOfNorthBalconies = randomSeedOfNorthBalconies;
        this.numberOfWestBalconies = numberOfWestBalconies;
        this.randomSeedOfWestBalconies = randomSeedOfWestBalconies;
        this.numberOfEastBalconies = numberOfEastBalconies;
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
    }

    public Building(int sizeX, int sizeY)
    {
        size = new Vector2Int(sizeX, sizeY);
    }

    public override string ToString(){
        string bldg = "Building:(" + size.ToString() + ";" + wings.Length + ")\n";
        foreach (var w in wings){
            bldg += "\t" + w.ToString() + "\n";
        }
        return bldg;
    }
}

public enum BuildingType
{
    Residental
}