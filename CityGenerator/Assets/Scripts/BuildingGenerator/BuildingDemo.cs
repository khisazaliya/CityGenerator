using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDemo : MonoBehaviour
{

    public BuildingSettings[] settings;
    public void GenerateBuilding()
    {
        for (int i = 0; i< settings.Length; i++)
        {
            Building b = BuildingGenerator.Generate(settings[i]);
            GetComponent<BuildingRenderer>().Render(b);
            Debug.Log(b.ToString());
        }
    }


}
