using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;

public class BuildingData : MonoBehaviour
{
    public static Building Build(BuildingSettings buildingSettings)
    {
        return new Building(
           buildingSettings.numberOfLevels,
           buildingSettings.buildingSize.x,
           buildingSettings.buildingSize.y,
           GenerateWings(buildingSettings),
           buildingSettings.numberOfEntries,
           buildingSettings.MaxNumberOfBalconies,
           buildingSettings.randomSeedOfBalconies,
           buildingSettings.randomOffsetNorthWall
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
