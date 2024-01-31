using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Generation/Building Settings")]
public class BuildingSettings : ScriptableObject
{
    [SerializeField]
    public Vector2Int buildingSize;
    [SerializeField]
    public WingStrategy wingStrategy;
    [SerializeField]
    public WingsStrategy wingsStrategy;
    [SerializeField]
    public RoofStrategy roofStrategy;
    [SerializeField]
    public WallsStrategy wallsStrategy;
    [SerializeField]
    public StoryStrategy storyStrategy;
    [SerializeField]
    public StoriesStrategy storiesStrategy;
    [SerializeField]
    public Vector2Int Size { get { return buildingSize; } }


}
