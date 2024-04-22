using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public string cellDataFilePath; 

    public void SaveCellData(List<CellData> cellDataList)
    {
        string jsonData = JsonUtility.ToJson(cellDataList); 
        File.WriteAllText(cellDataFilePath, jsonData); 
        Debug.Log("Cell data saved.");
    }

    public List<CellData> LoadCellData()
    {
        if (File.Exists(cellDataFilePath))
        {
            string jsonData = File.ReadAllText(cellDataFilePath); 
            return JsonUtility.FromJson<List<CellData>>(jsonData);
        }
        else
        {
            Debug.LogError("Cell data file not found: " + cellDataFilePath);
            return new List<CellData>();
        }
    }
}