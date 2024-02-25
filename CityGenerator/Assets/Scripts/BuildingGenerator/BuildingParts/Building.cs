using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building 
{
    Vector2Int size;
    Wing[] wings;
    BuildingType type;

    public int numberOfEntries;

    public int numberOfBalconies;
    public int randomSeedOfBalconies;

    public int level;
    public Vector2Int Size { get { return size; } }
    public Wing[] Wings { get { return wings; } }

    public BuildingType Type { get => type; }

    public int minOffsetNorthWall;

    public int maxOffsetNorthWall;

    public int depthOffsetNorthWall;

    public Building(int numberOfLevels, int sizeX, int sizeY, Wing[] wings, int numberOfEntries, int numberOfBalconies, int randomSeedOfBalconies, int minOffsetNorthWall,
        int maxOffsetNorthWall, int depthOffsetNorthWall) {
        level = numberOfLevels;
        size = new Vector2Int(sizeX, sizeY);
        this.wings = wings;
        this.numberOfEntries = numberOfEntries;
        this.numberOfBalconies = numberOfBalconies;
        this.randomSeedOfBalconies = randomSeedOfBalconies;
        this.minOffsetNorthWall = minOffsetNorthWall;
        this.maxOffsetNorthWall = maxOffsetNorthWall;
        this.depthOffsetNorthWall = depthOffsetNorthWall;
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