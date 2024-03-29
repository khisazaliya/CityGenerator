using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityMeshSimplifier;

public class LODGeneratorService : MonoBehaviour
{
    Dictionary<GameObject, LODGroup> gameObjectLODGroups = new();
    public void GenerateLODs(List<GameObject> gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            if (!gameObjectLODGroups.ContainsKey(gameObject))
            {
                //GenerateLODs(building);
                LODLevel[] levels = new LODLevel[]
                {
                            new LODLevel(0.5f, 0.5f, 1f, true, true),
                            new LODLevel(0.005f, 0.5f, 0.4f, true, true)
                };

                bool autoCollectRenderers = true;
                SimplificationOptions simplificationOptions = new SimplificationOptions()
                {
                    PreserveBorderEdges = true,
                    PreserveUVSeamEdges = false,
                    PreserveUVFoldoverEdges = true,
                    PreserveSurfaceCurvature = false,
                    EnableSmartLink = true,
                    VertexLinkDistance = 0.00001,
                    MaxIterationCount = 100,
                    Agressiveness = 1.0,
                    ManualUVComponentCount = false,
                    UVComponentCount = 2
                };

                string saveAssetsPath = "Assets/Buildings";
                LODGroup lodGroup = LODGenerator.GenerateLODs(gameObject, levels, autoCollectRenderers, simplificationOptions, saveAssetsPath);
                gameObjectLODGroups.Add(gameObject, lodGroup);
                DestroyImmediate(gameObject.transform.Find("Wing").gameObject);
            }
            else
            {
                // Используем уже существующие LODs для этого здания
                gameObjectLODGroups[gameObject].gameObject.SetActive(true);
            }
        }
    }

    public void DestroyLODs(List<GameObject> gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            if (gameObjectLODGroups.ContainsKey(gameObject))
                LODGenerator.DestroyLODs(gameObject);
        }
    }
}