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
    public BuildingSettings(Vector2Int size, int count, int numberOfLevels, BuildingType type, int numberOfEntries, int offsetOfEntries,  int numberOfBalconies, int randomSeedOfBalconies,
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
        this.minOffsetNorthWall = minOffsetNorthWall;
        this.maxOffsetNorthWall = maxOffsetNorthWall;
        this.depthOffsetNorthWall = depthOffsetNorthWall;
        this.northWallHeight = northWallHeight;

        this.minOffsetSouthWall = minOffsetSouthWall;
        this.maxOffsetSouthWall = maxOffsetSouthWall;
        this.depthOffsetSouthWall = depthOffsetSouthWall;
        this.southWallHeight = southWallHeight;
    }
}
