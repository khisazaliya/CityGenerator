using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDemo : MonoBehaviour
{
    public BuildingStrategiesSettings strategySettings;
    public BuildingSettings[] buildingSettings;
    GameObject renderedBuilding = new();
    // public MeshCombiner meshCombiner;

    public GameObject GenerateBuilding(Vector3 position, Quaternion rotation, BuildingSettings buildingSettings)
    {

            Building b = BuildingGenerator.Generate(strategySettings, buildingSettings);
            Debug.Log(buildingSettings.numberOfLevels + "bldg");
            BuildingRenderer buildingRenderer = GetComponent<BuildingRenderer>();
            renderedBuilding = buildingRenderer.Render(b);
            //renderedBuilding = meshCombiner.CombineMeshes(renderedBuilding);
            renderedBuilding.transform.SetPositionAndRotation(position, rotation);
            float scaleFactor = 0.5f;
            // Получаем текущий масштаб
            Vector3 currentScale = renderedBuilding.transform.localScale;

            // Умножаем каждую компоненту вектора на scaleFactor
            Vector3 newScale = new Vector3(
                currentScale.x * scaleFactor,
                currentScale.y * scaleFactor,
                currentScale.z * scaleFactor
            );

            // Устанавливаем новый масштаб
            renderedBuilding.transform.localScale = newScale;
            Debug.Log(b.ToString());
        return renderedBuilding;
    }

    public BuildingDemo(BuildingStrategiesSettings settings)
    {
        this.strategySettings = settings;
    }

   
}
