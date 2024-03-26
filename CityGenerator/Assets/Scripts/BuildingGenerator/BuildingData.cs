using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData : MonoBehaviour
{
    public static Building Build(BuildingSettings buildingSettings)
    {
        return new Building(
           buildingSettings.buildingSize.x,
           buildingSettings.buildingSize.y,
           GenerateWings(buildingSettings),
           buildingSettings.type,
           buildingSettings.numberOfEntries,
           buildingSettings.offsetOfEntries,
           buildingSettings.MaxNumberOfSouthBalconies,
           buildingSettings.randomSeedOfSouthBalconies,
           buildingSettings.MaxNumberOfNorthBalconies,
           buildingSettings.randomSeedOfNorthBalconies,
           buildingSettings.MaxNumberOfWestBalconies,
           buildingSettings.randomSeedOfWestBalconies,
           buildingSettings.MaxNumberOfEastBalconies,
           buildingSettings.randomSeedOfEastBalconies,
           buildingSettings.randomSeedOfRoofElements,
           buildingSettings.balconyLocations,
           buildingSettings.entryLocations,
           buildingSettings.numberOfLevels,
           buildingSettings.minOffsetNorthWall,
            buildingSettings.maxOffsetNorthWall,
            buildingSettings.depthOffsetNorthWall,
            buildingSettings.northWallHeight,
            buildingSettings.minOffsetSouthWall,
            buildingSettings.maxOffsetSouthWall,
            buildingSettings.depthOffsetSouthWall,
            buildingSettings.southWallHeight,
            buildingSettings.minOffsetEastWall,
            buildingSettings.maxOffsetEastWall,
            buildingSettings.depthOffsetEastWall,
            buildingSettings.eastWallHeight,
              buildingSettings.floorSouthPrefabIndex,
        buildingSettings.wall0SouthPrefabIndex,
        buildingSettings.wall1SouthPrefabIndex,
        buildingSettings.wall2SouthPrefabIndex,
        buildingSettings.doorSouthPrefabIndex,
        buildingSettings.roofPrefabIndex,
        buildingSettings.roofBoundPrefabIndex,
        buildingSettings.roofCornerPrefabIndex,
        buildingSettings.stairSouthPrefabIndex,
        buildingSettings.balconySouthPrefabIndex,

        buildingSettings.savedSeedOfSouthBalconies,
        buildingSettings.savedNumberOfSouthBalconies,

        buildingSettings.floorNorthPrefabIndex,
        buildingSettings.wall0NorthPrefabIndex,
        buildingSettings.wall1NorthPrefabIndex,
        buildingSettings.wall2NorthPrefabIndex,
        buildingSettings.doorNorthPrefabIndex,
        buildingSettings.stairNorthPrefabIndex,
        buildingSettings.balconyNorthPrefabIndex,

        buildingSettings.savedSeedOfNorthBalconies,
        buildingSettings.savedNumberOfNorthBalconies,

        buildingSettings.floorEastPrefabIndex,
        buildingSettings.wall0EastPrefabIndex,
        buildingSettings.wall1EastPrefabIndex,
        buildingSettings.wall2EastPrefabIndex,
        buildingSettings.wallDoorPrefabIndex,
        buildingSettings.doorEastPrefabIndex,
        buildingSettings.stairEastPrefabIndex,
        buildingSettings.balconyEastPrefabIndex,

        buildingSettings.savedSeedOfEastBalconies,
        buildingSettings.savedNumberOfEastBalconies,

        buildingSettings.floorWestPrefabIndex,
        buildingSettings.wall0WestPrefabIndex,
        buildingSettings.wall1WestPrefabIndex,
        buildingSettings.wall2WestPrefabIndex,
        buildingSettings.doorWestPrefabIndex,
        buildingSettings.stairWestPrefabIndex,
        buildingSettings.balconyWestPrefabIndex,

        buildingSettings.savedSeedOfWestBalconies,
        buildingSettings.savedNumberOfWestBalconies,

        buildingSettings.floorSouthOffsetPrefabIndex,
        buildingSettings.wall0SouthOffsetPrefabIndex,
        buildingSettings.wall1SouthOffsetPrefabIndex,
        buildingSettings.wall2SouthOffsetPrefabIndex,
        buildingSettings.doorSouthOffsetPrefabIndex,
        buildingSettings.roofSouthOffsetPrefabIndex,
        buildingSettings.roofSouthOffsetBoundPrefabIndex,
        buildingSettings.roofSouthOffsetCornerPrefabIndex,
        buildingSettings.stairSouthOffsetPrefabIndex,
        buildingSettings.balconySouthOffsetPrefabIndex,


        buildingSettings.floorNorthOffsetPrefabIndex,
        buildingSettings.wall0NorthOffsetPrefabIndex,
        buildingSettings.wall1NorthOffsetPrefabIndex,
        buildingSettings.wall2NorthOffsetPrefabIndex,
        buildingSettings.doorNorthOffsetPrefabIndex,
        buildingSettings.roofNorthOffsetPrefabIndex,
        buildingSettings.roofNorthOffsetBoundPrefabIndex,
        buildingSettings.roofNorthOffsetCornerPrefabIndex,
        buildingSettings.stairNorthOffsetPrefabIndex,
        buildingSettings.balconyNorthOffsetPrefabIndex,

        buildingSettings.floorEastOffsetPrefabIndex,
        buildingSettings.wall0EastOffsetPrefabIndex,
        buildingSettings.wall1EastOffsetPrefabIndex,
        buildingSettings.wall2EastOffsetPrefabIndex,
        buildingSettings.doorEastOffsetPrefabIndex,
        buildingSettings.roofEastOffsetPrefabIndex,
        buildingSettings.roofEastOffsetBoundPrefabIndex,
        buildingSettings.roofEastOffsetCornerPrefabIndex,
        buildingSettings.stairEastOffsetPrefabIndex,
        buildingSettings.balconyEastOffsetPrefabIndex

       );
    }

    public static Wing[] GenerateWings(BuildingSettings buildingSettings)
    {
        return new Wing[] {
        GenerateWing(
            buildingSettings,
            new RectInt(0, 0 , buildingSettings.buildingSize.x, buildingSettings.buildingSize.y)
            )
        };
    }
    public static Wing GenerateWing(BuildingSettings buildingSettigns,
     RectInt bounds)
    {
        return new Wing(
            bounds,
              GenerateStories(buildingSettigns, bounds),
              GenerateRoof(bounds)
        );
    }

    public static Story[] GenerateStories(BuildingSettings buildingSettings, RectInt bounds)
    {
        return new Story[]  {
         GenerateStory(buildingSettings, bounds)
        };
    }

    public static Story GenerateStory(BuildingSettings buildingSettings, RectInt bounds)
    {
        return new Story(
            GenerateWalls(bounds)
        );
    }

    public static Wall[] GenerateWalls(RectInt bounds)
    {
        return new Wall[(bounds.size.x + bounds.size.y) * 2];
    }

    public static Roof GenerateRoof(RectInt bounds)
    {
        return new Roof();
    }
}
[System.Serializable]
public class BuildingFileData
{
    public PrefabIndexes prefabIndexes;
    public List<BuildingSettings> buildingSettings;
}
