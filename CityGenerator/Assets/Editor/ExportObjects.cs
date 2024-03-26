#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
namespace Assets.Editor
{
    public class ExportObjects : EditorWindow
    {
        [MenuItem("Tools/Export Selected Objects")]
        static void ExportSelectedObjects()
        {
            string[] assetPaths = Selection.assetGUIDs;
            string exportPath = EditorUtility.SaveFilePanel("Export Package", "", "ExportedObjects", "unitypackage");

            if (exportPath.Length != 0)
            {
                AssetDatabase.ExportPackage(assetPaths, exportPath, ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
            }
        }
    }
}
#endif