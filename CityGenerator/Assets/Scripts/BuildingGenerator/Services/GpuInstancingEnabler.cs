using System.IO;
using UnityEngine;

namespace Assets.Scripts.BuildingGenerator.Services
{
    [RequireComponent(typeof(MeshRenderer))]
    class GpuInstancingEnabler : MonoBehaviour
    {

        private void Awake()
        {
            MaterialPropertyBlock materialPropertyBlock = new();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}
