using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDemo : MonoBehaviour
{
    public BuildingSettings[] settings;
    public MeshCombiner meshCombiner;

    public GameObject GenerateBuilding(Vector3 position, Quaternion rotation)
    {
        GameObject renderedBuilding = new();
        for (int i = 0; i < settings.Length; i++)
        {
            Building b = BuildingGenerator.Generate(settings[i]);
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
        }
        return renderedBuilding;
    }

    public BuildingDemo(BuildingSettings[] settings)
    {
        this.settings = settings;
    }

    public BuildingDemo()
    {
    }
}
