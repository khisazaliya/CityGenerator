#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(RoadGenerator))]
public class RoadGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoadGenerator roadGenerator = (RoadGenerator)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate road"))
        {
            roadGenerator.GenerateRoad();
        }
    }
}
#endif