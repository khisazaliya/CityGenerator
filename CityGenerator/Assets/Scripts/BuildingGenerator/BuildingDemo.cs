using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDemo : MonoBehaviour
{

    public BuildingSettings[] settings;
    public void GenerateBuilding(Vector3 position)
    {
        for (int i = 0; i< settings.Length; i++)
        {
            Building b = BuildingGenerator.Generate(settings[i]);
            GetComponent<BuildingRenderer>().Render(b);
            //b.transform.position = position;
            Debug.Log(b.ToString());
        }
    }

    public BuildingDemo(BuildingSettings[] settings)
    {
        this.settings = settings;
    }

    public BuildingDemo()
    {
    }
}
