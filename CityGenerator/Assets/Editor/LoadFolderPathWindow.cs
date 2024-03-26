/*#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class LoadFolderPathWindow : EditorWindow
{
    private string loadFolderPath = "";

    public Visualizer visualizer = new();
    [MenuItem("Window/Load Folder Path")]
    public static void ShowWindow()
    {
        GetWindow<LoadFolderPathWindow>("Load Folder Path");
    }

    public void OnGUI()
    {
        GUILayout.Label("Select Folder for Loading", EditorStyles.boldLabel);

        loadFolderPath = EditorGUILayout.TextField("Folder Path", loadFolderPath);

        if (GUILayout.Button("Select Folder"))
        {
            loadFolderPath = EditorUtility.OpenFolderPanel("Select Folder for Loading", "", "");
        }

        if (GUILayout.Button("Apply"))
        {
            visualizer.loadFolderPath = loadFolderPath;
            Close();
        }
    }
}
#endif*/