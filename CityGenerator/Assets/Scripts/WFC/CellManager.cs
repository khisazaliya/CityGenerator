using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public string cellDataFilePath; // Путь к файлу, где будет сохранена информация о ячейках

    // Метод для сохранения информации о ячейках в файл
    public void SaveCellData(List<CellData> cellDataList)
    {
        string jsonData = JsonUtility.ToJson(cellDataList); // Преобразование списка данных в формат JSON
        File.WriteAllText(cellDataFilePath, jsonData); // Запись данных в файл
        Debug.Log("Cell data saved.");
    }

    // Метод для загрузки информации о ячейках из файла
    public List<CellData> LoadCellData()
    {
        if (File.Exists(cellDataFilePath))
        {
            string jsonData = File.ReadAllText(cellDataFilePath); // Чтение данных из файла
            return JsonUtility.FromJson<List<CellData>>(jsonData); // Преобразование JSON в список объектов CellData
        }
        else
        {
            Debug.LogError("Cell data file not found: " + cellDataFilePath);
            return new List<CellData>();
        }
    }
}