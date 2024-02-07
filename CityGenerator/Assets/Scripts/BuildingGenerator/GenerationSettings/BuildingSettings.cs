using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class BuildingSettings
{
    public Vector2Int buildingSize;
    public int buildingCount;
    public int numberOfLevels;
    public BuildingType type;

    public int numberOfEntries;
    public BuildingSettings(Vector2Int size, int count, int numberOfLevels, BuildingType type, int numberOfEntries)
    {
        buildingSize = size;
        buildingCount = count;
        this.numberOfLevels = numberOfLevels;
        this.type = type;
        this.numberOfEntries = numberOfEntries;
    }
}