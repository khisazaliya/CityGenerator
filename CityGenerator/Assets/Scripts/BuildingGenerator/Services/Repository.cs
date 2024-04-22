﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.BuildingGenerator.Services
{
    public class Repository
    {
        [SerializeField]
        public string loadFilePath = "";
        private string saveFilePath = "";

        [ContextMenu("Load")]
        public List<BuildingSettings> LoadField()
        {
            if (string.IsNullOrEmpty(loadFilePath))
            {
                loadFilePath = EditorUtility.OpenFilePanel("Load Building Settings", "", "json");
                if (string.IsNullOrEmpty(loadFilePath)) throw new NullReferenceException(); // Если пользователь не выбрал файл, выйти из метода
            }

            string jsonText = File.ReadAllText(loadFilePath);
            var wrapper = JsonUtility.FromJson<BuildingSettingsWrapper>(jsonText);
            return wrapper.buildingSettings;
        }

        public void SetLoadFilePath(string path)
        {
            loadFilePath = path;
        }

        [ContextMenu("Save")]
        public void SaveField(List<BuildingSettings> buildingSettings)
        {
            if (string.IsNullOrEmpty(saveFilePath))
            {
                saveFilePath = EditorUtility.SaveFilePanel("Save Settings", "", "settings", "json");
                if (string.IsNullOrEmpty(saveFilePath))
                {
                    Debug.LogWarning("Save operation cancelled.");
                    return;
                }
            }

            List<BuildingSettings> allSettings = new List<BuildingSettings>();
            if (File.Exists(saveFilePath))
            {
                string jsonText = File.ReadAllText(saveFilePath);
                var wrapper = JsonUtility.FromJson<BuildingSettingsWrapper>(jsonText);
                if (wrapper != null && wrapper.buildingSettings != null)
                    allSettings.AddRange(wrapper.buildingSettings);
            }

            allSettings.AddRange(buildingSettings);
            BuildingSettingsWrapper newWrapper = new BuildingSettingsWrapper
            {
                buildingSettings = allSettings
            };
            string json = JsonUtility.ToJson(newWrapper, true);
            File.WriteAllText(saveFilePath, json);
        }

        [ContextMenu("Save As New File")]
        public void SaveAsNewFile(List<BuildingSettings> buildingSettings)
        {
            saveFilePath = EditorUtility.SaveFilePanel("Save Settings As New File", "", "settings", "json");
            if (string.IsNullOrEmpty(saveFilePath))
            {
                Debug.LogWarning("Save operation cancelled.");
                return;
            }

            SaveField(buildingSettings);
        }

        [ContextMenu("Add Building Settings To File")]
        public void AddBuildingSettingsToFile(List<BuildingSettings> buildingSettings)
        {
            string filePath = EditorUtility.OpenFilePanel("Add Building Settings To File", "", "json");
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogWarning("Operation cancelled.");
                return;
            }
            string jsonText = File.ReadAllText(filePath);
            var wrapper = JsonUtility.FromJson<BuildingSettingsWrapper>(jsonText);
            List<BuildingSettings> existingSettings = wrapper.buildingSettings;

            existingSettings.AddRange(buildingSettings);
            BuildingSettingsWrapper newWrapper = new BuildingSettingsWrapper
            {
                buildingSettings = existingSettings
            };
            string json = JsonUtility.ToJson(newWrapper, true);
            File.WriteAllText(filePath, json);

            Debug.Log("Building settings added to file: " + filePath);
        }

        private class BuildingSettingsWrapper
        {
            public List<BuildingSettings> buildingSettings;
        }
    }
}

