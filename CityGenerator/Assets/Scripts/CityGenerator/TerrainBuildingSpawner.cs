using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CityGenerator
{
    public class TerrainBuildingSpawner : MonoBehaviour
    {
        public GameObject[] buildingPrefabs;
        public Terrain terrain;

        public int numBuildings = 100;
        public float minHeight = 50f;
        public float maxHeight = 200f;

        void Start()
        {
            GenerateCity();
        }

        Quaternion GetRandomRotation(Vector3 normal)
        {
            // Вычисляем вектор направления "вверх" для здания
            Vector3 up = Vector3.up;

            // Создаем кватернион, поворачивающий объект из вектора "вверх" в нормаль поверхности
            Quaternion rotation = Quaternion.FromToRotation(up, normal);

            // Получаем случайное вращение вокруг оси Y
            Quaternion randomRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

            // Комбинируем вращения
            return rotation * randomRotation;
        }

        void GenerateCity()
        {
            for (int i = 0; i < numBuildings; i++)
            {
                Vector3 randomPoint = GetRandomPointOnTerrain();
                float height = terrain.SampleHeight(randomPoint);

                if (height >= minHeight && height <= maxHeight)
                {
                    int prefabIndex = UnityEngine.Random.Range(0, buildingPrefabs.Length);
                    GameObject buildingPrefab = buildingPrefabs[prefabIndex];

                    Vector3 buildingSize = buildingPrefab.GetComponent<Renderer>().bounds.size;

                    // Определяем позицию спавна здания с учетом его высоты и размеров
                    Vector3 spawnPosition = new Vector3(randomPoint.x, height + (buildingSize.y / 2f), randomPoint.z);

                    // Получаем нормаль поверхности террейна в точке спавна здания
                    Vector3 normal = terrain.terrainData.GetInterpolatedNormal(randomPoint.x / terrain.terrainData.size.x, randomPoint.z / terrain.terrainData.size.z);

                    // Получаем случайный поворот для здания с учетом нормали поверхности
                    Quaternion randomRotation = GetRandomRotation(normal);

                    // Создаем здание с учетом позиции, поворота и префаба
                    GameObject building = Instantiate(buildingPrefab, spawnPosition, randomRotation);
                }
            }
        }

        Vector3 GetRandomPointOnTerrain()
        {
            float terrainWidth = terrain.terrainData.size.x;
            float terrainHeight = terrain.terrainData.size.z;

            float randomX = UnityEngine.Random.Range(0f, terrainWidth) + terrain.transform.position.x;
            float randomZ = UnityEngine.Random.Range(0f, terrainHeight) + terrain.transform.position.z;

            float height = terrain.SampleHeight(new Vector3(randomX, 0f, randomZ));

            return new Vector3(randomX, height, randomZ);
        }
    }
}