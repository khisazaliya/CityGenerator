using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
public class PrototypeGenerator : MonoBehaviour
{
    public List<Prototype> roadModules;
    public List<Prototype> prototypes;
    public string path = "Assets/Data/Prototypes";
    WFC_Socket posXHolder;
    WFC_Socket negXHolder;
    WFC_Socket posZHolder;
    WFC_Socket negZHolder;
    List<GameObject> prototypeHolder = new List<GameObject>();

    [SerializeField] private GameObject prototypeHolderPrefab;

    [ContextMenu("Generate Prototypes")]
    public void GeneratePrototypes()
    {
        prototypes.Clear();
#if UNITY_EDITOR
        if (Directory.Exists(path))
            Directory.Delete(path);

        Directory.CreateDirectory(path);
#endif

        for (int i = 0; i < roadModules.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Prototype newProto = CreateMyAsset(path, roadModules[i].name, j.ToString().Replace(" ", ""));
                prototypes.Add(newProto);
            }
        }
        UpdatePrototypes();

        prototypeHolderPrefab.GetComponent<Cell>().possiblePrototypes = prototypes;
    }
    public void UpdatePrototypes()
    {
        for (int i = 0; i < roadModules.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                prototypes[i * 4 + j].prefab = roadModules[i].prefab;
                prototypes[i * 4 + j].validNeighbours = new NeighbourList();
                prototypes[i * 4 + j].meshRotation = j;
                prototypes[i * 4 + j].attributes = roadModules[i].attributes;

                prototypes[i * 4 + j].posX = roadModules[i].posX;
                prototypes[i * 4 + j].negX = roadModules[i].negX;
                prototypes[i * 4 + j].posZ = roadModules[i].posZ;
                prototypes[i * 4 + j].negZ = roadModules[i].negZ;

                if (j == 0)
                {
                    posXHolder = prototypes[i * 4 + j].posX;
                    negXHolder = prototypes[i * 4 + j].negX;
                    posZHolder = prototypes[i * 4 + j].posZ;
                    negZHolder = prototypes[i * 4 + j].negZ;
                }
                else
                {
                    prototypes[i * 4 + j].negZ = posXHolder;
                    prototypes[i * 4 + j].negX = negZHolder;
                    prototypes[i * 4 + j].posZ = negXHolder;
                    prototypes[i * 4 + j].posX = posZHolder;

                    posXHolder = prototypes[i * 4 + j].posX;
                    negXHolder = prototypes[i * 4 + j].negX;
                    posZHolder = prototypes[i * 4 + j].posZ;
                    negZHolder = prototypes[i * 4 + j].negZ;
                }
            }
        }
        for (int i = 0; i < prototypes.Count; i++)
            prototypes[i].validNeighbours = GetValidNeighbors(prototypes[i]);
    }
    public static Prototype CreateMyAsset(string assetFolder, string name, string j)
    {
        Prototype asset = ScriptableObject.CreateInstance<Prototype>();
#if UNITY_EDITOR
        AssetDatabase.CreateAsset(asset, assetFolder + "/" + name + "_" + j + ".asset");
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
#endif

        return asset;
    }
    private NeighbourList GetValidNeighbors(Prototype prototype)
    {
        NeighbourList neighbourList = new NeighbourList();
        foreach (Prototype p in prototypes)
        {
            if (prototype.posX == p.negX)
                neighbourList.posX.Add(p);
            if (prototype.negX == p.posX)
                neighbourList.negX.Add(p);
            if (prototype.posZ == p.negZ)
                neighbourList.posZ.Add(p);
            if (prototype.negZ == p.posZ)
                neighbourList.negZ.Add(p);
        }
        return neighbourList;
    }
  
}
