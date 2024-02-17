using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public void CombineMeshes(Transform buildingTransform)
    {
        MeshRenderer[] meshRenderers = buildingTransform.GetComponentsInChildren<MeshRenderer>();
        List<MeshFilter> meshFilters = new List<MeshFilter>();
        List<Material> materials = new List<Material>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
            meshFilters.Add(meshFilter);
            materials.AddRange(meshRenderer.sharedMaterials);
        }

        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Count];
        for (int i = 0; i < meshFilters.Count; i++)
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
            DestroyImmediate(meshFilters[i].gameObject);
        }

        GameObject combinedMeshObject = new GameObject("CombinedMesh");
        combinedMeshObject.transform.SetParent(buildingTransform);
        MeshFilter combinedMeshFilter = combinedMeshObject.AddComponent<MeshFilter>();
        MeshRenderer combinedMeshRenderer = combinedMeshObject.AddComponent<MeshRenderer>();


        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        combinedMesh.CombineMeshes(combineInstances, false);
        combinedMeshFilter.sharedMesh = combinedMesh;

        combinedMeshRenderer.sharedMaterials = materials.ToArray();
    }
}
