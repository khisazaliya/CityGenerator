/*using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonController : MonoBehaviour
{
    public BuildingSettings BuildingSettings;

    [ContextMenu("Load")]
    public void LoadField()
    {
        BuildingSettings = JsonUtility.FromJson<BuildingSettings>(File.ReadAllText(Application.streamingAssetsPath + "/settings.json"));
    }

    [ContextMenu("Save")]
    public void SaveField()
    {
        File.WriteAllText(Application.streamingAssetsPath + "/settings.json", JsonUtility.ToJson(BuildingSettings));
    }
}
*/