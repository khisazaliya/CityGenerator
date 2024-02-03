/*using UnityEngine;


public class MeshCombiner : MonoBehaviour
{
    public GameObject CombineMeshes(GameObject building)
    {
        MeshFilter[] meshFilters = building.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        // Create a new combined mesh
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);

        // Create a new GameObject for the combined mesh
        GameObject combinedObject = new GameObject("CombinedMesh");
        combinedObject.transform.SetParent(building.transform);
        combinedObject.transform.localPosition = Vector3.zero;

        // Add a MeshFilter and MeshRenderer to the combined GameObject
        MeshFilter meshFilter = combinedObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = combinedObject.AddComponent<MeshRenderer>();

        // Assign the combined mesh to the MeshFilter
        meshFilter.mesh = combinedMesh;

        // Optional: Assign the material from one of the original objects
        if (meshFilters.Length > 0)
        {
            meshRenderer.material = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;
        }
        return building;
    }
}

*/