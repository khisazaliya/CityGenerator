#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;
using Assets.Scripts.BuildingGenerator.Services;

public class WaveFunctionCollapse : MonoBehaviour
{
    public GameObject allProtoPrefab;
    public float gridOffset = 1;
    public Vector2 size;
    public Vector3 startPosition;
    public List<Cell> cells;
    public Dictionary<Vector2, Cell> activeCells = new Dictionary<Vector2, Cell>();
    public List<Cell> cellsAffected = new List<Cell>();
    public Weights weights;
    [HideInInspector] public List<Transform> places = new();
    [HideInInspector] public List<Transform> streetElementsPlaces = new();
    [HideInInspector] public List<Vector3> oldBuildingsPlaces = new();
    [HideInInspector] public List<Vector3> oldStreetElementsPlaces = new();
    [HideInInspector] public List<Vector3> oldNaturesPlaces = new();

    void Start()
    {
        StartCoroutine(CollapseOverTime());
    }

    public void InitializeWaveFunction()
    {
        ClearAll();
    
        for (int x = 0, y = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector3 pos = new Vector3(x * gridOffset + startPosition.x, 0, z * gridOffset + startPosition.z);

                if (this.gameObject.transform.childCount > y)
                {
                    GameObject block = this.transform.GetChild(y).gameObject;
                    block.SetActive(true);
                    block.transform.position = pos;
                }
                else
                {
#if UNITY_EDITOR
                    GameObject block = (GameObject)PrefabUtility.InstantiatePrefab(allProtoPrefab as GameObject);
                    PrefabUtility.UnpackPrefabInstance(block, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                    block.transform.SetParent(this.transform);
                    block.transform.position = pos;
#endif
                }
                Cell cell = this.transform.GetChild(y).gameObject.GetComponent<Cell>();
                cell.coords = new Vector2(x, z);
                cells.Add(cell);
                activeCells.Add(cell.coords, cell);
                y++;
            }
        }
        foreach (Cell c in cells)
            FindNeighbours(c);

        foreach (Cell c in cells)
            c.GenerateWeight(weights);

        StartCollapse();
    }

    private void FindNeighbours(Cell c)
    {
        c.posZneighbour = GetCell(c.coords.x, c.coords.y + 1);
        c.negZneighbour = GetCell(c.coords.x, c.coords.y - 1);
        c.posXneighbour = GetCell(c.coords.x + 1, c.coords.y);
        c.negXneighbour = GetCell(c.coords.x - 1, c.coords.y);
    }
    private Cell GetCell(float x, float z)
    {
        Cell cell = null;
        if (activeCells.TryGetValue(new Vector2(x, z), out cell))
            return cell;
        else
            return null;
    }
    int collapsed;

    public bool SensitiveToTerrain { get; private set; }

    public void StartCollapse()
    {
        collapsed = 0;
        while (!isCollapsed())
            Iterate();
    }
    public IEnumerator CollapseOverTime()
    {
        while (!isCollapsed())
        {
            Iterate();
            yield return new WaitForSeconds(0.5f);
        }
    }
    private bool isCollapsed()
    {
        foreach (Cell c in cells)
            if (c.possiblePrototypes.Count > 1)
                return false;

        return true;
    }
    private void Iterate()
    {
        Cell cell = GetCellWithLowestEntropy();
        CollapseAt(cell);
        Propagate(cell);
    }
    private Cell GetCellWithLowestEntropy()
    {
        List<Cell> cellsWithLowestEntropy = new List<Cell>();
        float lowestEntropy = float.MaxValue;

        foreach (Cell cell in cells)
        {
            if (!cell.isCollapsed)
            {
                float entropy = CalculateEntropy(cell);
                if (entropy < lowestEntropy)
                {
                    cellsWithLowestEntropy.Clear();
                    cellsWithLowestEntropy.Add(cell);
                    lowestEntropy = entropy;
                }
                else if (entropy == lowestEntropy)
                {
                    cellsWithLowestEntropy.Add(cell);
                }
            }
        }

        return cellsWithLowestEntropy.Count > 0 ? cellsWithLowestEntropy[Random.Range(0, cellsWithLowestEntropy.Count)] : null;
    }
    private float CalculateEntropy(Cell cell)
    {
        float entropy = 0;
        int total = 0;
        foreach (int weight in cell.prototypeWeights)
            total += weight;


        foreach (var prototypeWeight in cell.prototypeWeights)
        {
            float probability = (float)prototypeWeight / total;
            entropy -= probability * Mathf.Log(probability, 2);
        }

        return entropy;
    }
    private void CollapseAt(Cell cell)
    {
        int selectedPrototype = SelectPrototype(cell.prototypeWeights);
        Prototype finalPrototype = cell.possiblePrototypes[selectedPrototype];
        finalPrototype.prefab = cell.possiblePrototypes[selectedPrototype].prefab;
        cell.possiblePrototypes.Clear();
        cell.possiblePrototypes.Add(finalPrototype);

        GameObject finalPrefab = Instantiate(finalPrototype.prefab, cell.transform, true);

        Vector3 cellCenter = cell.transform.position;
        Vector3 spawnPosition = new Vector3(cellCenter.x, 0f, cellCenter.z);

        float angle = 0;
        RaycastHit hit;
        if (SensitiveToTerrain)
        {
            if (Physics.Raycast(spawnPosition+ new Vector3(0, 200, 0), Vector3.down, out hit))
            {
                Vector3 surfaceNormal = hit.normal;
                Vector3 upDirection = Vector3.up; // Направление вверх

                // Получаем угол наклона поверхности
                angle = Vector3.Angle(surfaceNormal, upDirection);
                finalPrefab.transform.position = hit.point;

                finalPrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
            else
            {

                return;
            }
        }
        else
        {
            finalPrefab.transform.position = spawnPosition;
        }
        finalPrefab.transform.Rotate(new Vector3(0f, finalPrototype.meshRotation * 90, 0f), Space.Self);
        finalPrefab.transform.position += new Vector3(0, 0.6f, 0);
        if (angle < 10)
        {
            if (finalPrototype.attributes[0].Equals(Attribute.Building))
                places.Add(finalPrefab.transform);
            else
                streetElementsPlaces.Add(finalPrefab.transform);
        }

        cell.name = cell.coords.ToString() + "_" + collapsed.ToString();
        collapsed++;
        cell.isCollapsed = true;
    }


    private int SelectPrototype(List<int> prototypeWeights)
    {
        int total = 0;
        foreach (int weight in prototypeWeights)
            total += weight;

        total = Random.Range(0, total);

        foreach (int weight in prototypeWeights)
        {
            for (int i = 0; i < prototypeWeights.Count; i++)
            {
                if (total <= prototypeWeights[i])
                {
                    return i;
                }
                else
                    total -= weight;
            }
        }
        return 0;
    }
    private void Propagate(Cell cell)
    {
        cellsAffected.Add(cell);
        int y = 0;
        while (cellsAffected.Count > 0)
        {
            Cell currentCell = cellsAffected[0];
            cellsAffected.Remove(currentCell);
            Cell otherCell = currentCell.posXneighbour;
            if (otherCell != null)
            {
                List<WFC_Socket> possibleConnections = GetPossibleSocketsPosX(currentCell.possiblePrototypes);

                bool constrained = false;
                for (int i = 0; i < otherCell.possiblePrototypes.Count; i++)
                {
                    if (!possibleConnections.Contains(otherCell.possiblePrototypes[i].negX))
                    {
                        otherCell.possiblePrototypes.RemoveAt(i);
                        otherCell.prototypeWeights.RemoveAt(i);
                        i -= 1;
                        constrained = true;
                    }
                }

                if (constrained)
                    cellsAffected.Add(otherCell);
            }

            otherCell = currentCell.posZneighbour;
            if (otherCell != null)
            {
                List<WFC_Socket> possibleConnections = GetPossibleSocketsPosZ(currentCell.possiblePrototypes);
                bool hasBeenConstrained = false;

                //check all neighbours
                for (int i = 0; i < otherCell.possiblePrototypes.Count; i++)
                {
                    if (!possibleConnections.Contains(otherCell.possiblePrototypes[i].negZ))
                    {
                        otherCell.possiblePrototypes.RemoveAt(i);
                        otherCell.prototypeWeights.RemoveAt(i);
                        i -= 1;
                        hasBeenConstrained = true;
                    }
                }
                if (hasBeenConstrained)
                    cellsAffected.Add(otherCell);
            }
            otherCell = currentCell.negXneighbour;
            if (otherCell != null)
            {
                List<WFC_Socket> possibleConnections = GetPossibleSocketsNegX(currentCell.possiblePrototypes);
                bool hasBeenConstrained = false;
                for (int i = 0; i < otherCell.possiblePrototypes.Count; i++)
                {
                    if (!possibleConnections.Contains(otherCell.possiblePrototypes[i].posX))
                    {
                        otherCell.possiblePrototypes.RemoveAt(i);
                        otherCell.prototypeWeights.RemoveAt(i);
                        i -= 1;
                        hasBeenConstrained = true;
                    }
                }
                if (hasBeenConstrained)
                    cellsAffected.Add(otherCell);
            }
            otherCell = currentCell.negZneighbour;
            if (otherCell != null)
            {
                List<WFC_Socket> possibleConnections = GetPossibleSocketsNegZ(currentCell.possiblePrototypes);
                bool hasBeenConstrained = false;
                for (int i = 0; i < otherCell.possiblePrototypes.Count; i++)
                {
                    if (!possibleConnections.Contains(otherCell.possiblePrototypes[i].posZ))
                    {
                        otherCell.possiblePrototypes.RemoveAt(i);
                        otherCell.prototypeWeights.RemoveAt(i);
                        i -= 1;
                        hasBeenConstrained = true;
                    }
                }
                if (hasBeenConstrained)
                    cellsAffected.Add(otherCell);
            }
            y++;
        }
    }
    private List<WFC_Socket> GetPossibleSocketsNegX(List<Prototype> prototypesAvailable)
    {
        List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
        foreach (Prototype proto in prototypesAvailable)
        {
            if (!socketsAccepted.Contains(proto.negX))
                socketsAccepted.Add(proto.negX);
        }
        return socketsAccepted;
    }
    private List<WFC_Socket> GetPossibleSocketsNegZ(List<Prototype> prototypesAvailable)
    {
        List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
        foreach (Prototype proto in prototypesAvailable)
        {
            if (!socketsAccepted.Contains(proto.negZ))
                socketsAccepted.Add(proto.negZ);
        }
        return socketsAccepted;
    }
    private List<WFC_Socket> GetPossibleSocketsPosZ(List<Prototype> prototypesAvailable)
    {
        List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
        foreach (Prototype proto in prototypesAvailable)
        {
            if (!socketsAccepted.Contains(proto.posZ))
                socketsAccepted.Add(proto.posZ);
        }
        return socketsAccepted;
    }
    private List<WFC_Socket> GetPossibleSocketsPosX(List<Prototype> prototypesAvailable)
    {
        List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
        foreach (Prototype proto in prototypesAvailable)
        {
            if (!socketsAccepted.Contains(proto.posX))
            {
                socketsAccepted.Add(proto.posX);
            }
        }
        return socketsAccepted;
    }

    public void ClearAll()
    {
        cells.Clear();
        activeCells.Clear();
        for (int i = this.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }
        places.Clear();
        oldBuildingsPlaces.Clear();
        oldNaturesPlaces.Clear();
        streetElementsPlaces.Clear();
        oldStreetElementsPlaces.Clear();
    }
}






#endif