using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDemo : MonoBehaviour
{

    public BuildingSettings settings;
    public void GenerateBuilding()
    {
        Building b = BuildingGenerator.Generate(settings);
        GetComponent<BuildingRenderer>().Render(b);
        Debug.Log(b.ToString());
    }


}
