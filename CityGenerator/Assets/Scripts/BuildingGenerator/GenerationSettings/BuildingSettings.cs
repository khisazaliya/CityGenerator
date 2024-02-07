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

    public BuildingSettings(Vector2Int size, int count, int numberOfLevels)
    {
        buildingSize = size;
        buildingCount = count;
        this.numberOfLevels = numberOfLevels;
    }
}