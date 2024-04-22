using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isCollapsed;
    public List<Prototype> possiblePrototypes;
    public List<int> prototypeWeights;
    public Vector2 coords = new Vector2();
    public Cell posZneighbour;
    public Cell negZneighbour;
    public Cell posXneighbour;
    public Cell negXneighbour;
    public List<Prototype> validPrototypes;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public NeighbourList validNeighbours;
    public void GenerateWeight(Weights weights)
    {
        prototypeWeights = new List<int>(new int[possiblePrototypes.Count]);
        int i = 0;
        foreach (Prototype p in possiblePrototypes)
        {
            foreach (Attribute attribute in p.attributes)
                prototypeWeights[i] += weights.GetWeight(attribute);

            prototypeWeights[i] = (int)((float)prototypeWeights[i] / (float)p.attributes.Count);
            i++;
        }
    }
}