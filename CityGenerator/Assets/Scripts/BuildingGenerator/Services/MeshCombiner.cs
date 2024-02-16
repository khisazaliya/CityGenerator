using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public void CombineMeshes(Transform buildingTransform)
    {
        // Получаем все меш-рендереры внутри здания
        MeshRenderer[] meshRenderers = buildingTransform.GetComponentsInChildren<MeshRenderer>();

        // Создаем список всех мешей и материалов
        List<MeshFilter> meshFilters = new List<MeshFilter>();
        List<Material> materials = new List<Material>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();

            // Добавляем меш и материалы в список
            meshFilters.Add(meshFilter);
            materials.AddRange(meshRenderer.sharedMaterials);
        }

        // Создаем новый комбинированный меш
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Count];
        for (int i = 0; i < meshFilters.Count; i++)
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;

            // Очищаем объекты мешей после получения информации
            Destroy(meshFilters[i].gameObject);
        }

        // Создаем новый GameObject для комбинированного меша
        GameObject combinedMeshObject = new GameObject("CombinedMesh");
        combinedMeshObject.transform.SetParent(buildingTransform);

        // Добавляем MeshFilter и MeshRenderer
        MeshFilter combinedMeshFilter = combinedMeshObject.AddComponent<MeshFilter>();
        MeshRenderer combinedMeshRenderer = combinedMeshObject.AddComponent<MeshRenderer>();

        // Создаем новый меш и присваиваем его комбинированному фильтру
        // Создаем новый меш с указанием формата индексов
        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        // Комбинируем меши
        combinedMesh.CombineMeshes(combineInstances, false);
        combinedMeshFilter.sharedMesh = combinedMesh;

        // Присваиваем материалы комбинированному рендереру
        combinedMeshRenderer.sharedMaterials = materials.ToArray();
    }
}
