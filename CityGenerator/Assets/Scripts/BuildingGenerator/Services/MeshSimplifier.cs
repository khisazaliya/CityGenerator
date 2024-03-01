/*using UnityEngine;
using UnityEngine.AI;
using UnityMeshSimplifier;

public class MeshSimplifier : MonoBehaviour
{
    public MeshFilter meshFilter;
    public float targetTriangleCount = 1000;

    void Start()
    {
        SimplifyMesh();
    }

    void SimplifyMesh()
    {
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            Mesh inputMesh = meshFilter.sharedMesh;
            var meshSimplifier = new MeshSimplifier();
            meshSimplifier.Initialize(inputMesh);
            meshSimplifier.SimplifyMesh(targetTriangleCount);
            Mesh simplifiedMesh = meshSimplifier.ToMesh();
            meshFilter.sharedMesh = simplifiedMesh;
        }
        else
        {
            Debug.LogError("MeshFilter or its sharedMesh is not assigned.");
        }
    }
}
*/