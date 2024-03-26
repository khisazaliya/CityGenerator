#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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
    public BuildingGenerator buildingGenerator;
    [HideInInspector] public List<Transform> places = new();
    [HideInInspector] public List<Transform> streetElementsPlaces = new();
    [HideInInspector] public List<Vector3> oldBuildingsPlaces = new();
    [HideInInspector] public List<Vector3> oldStreetElementsPlaces = new();
    [HideInInspector] public List<GameObject> buildings = new List<GameObject>();
    [HideInInspector] public List<Vector3> oldNaturesPlaces = new();
    [HideInInspector] public List<GameObject> natures = new List<GameObject>();
    [HideInInspector] public List<GameObject> streetElements = new List<GameObject>();
    public List<GameObject> naturePrefabs = new List<GameObject>();
    public int natureCount;
    public int streetElementsCount;
    public List<GameObject> streetPrefabs = new List<GameObject>();
    public LODGeneratorService LODGeneratorService = new();
    [HideInInspector] public System.Random rand = new();
    void Start()
    {
        StartCoroutine(CollapseOverTime());
    }
    public void InitializeWaveFunction()
    {
        ClearAll();
        places.Clear();
        oldBuildingsPlaces.Clear();
        buildings.Clear();
        oldNaturesPlaces.Clear();
        natures.Clear();
        streetElementsPlaces.Clear();
        oldStreetElementsPlaces.Clear();
        streetElements.Clear();
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
       // CreateBorder();
    }
/*     private void CreateBorder()
     {
        for (int x = 0; x < size.x; x++)
         {
            Cell cell = GetCell(x * gridOffset + startPosition.x, -1 * gridOffset + startPosition.z);
            if (cell.possiblePrototypes[0].attributes.Contains(Attribute.Crossroad))
                DoInstantiate(borderIntersectionPrefab, new Vector3(x * gridOffset + startPosition.x, 0, -1 * gridOffset + startPosition.z), Quaternion.identity, this.transform);
            else
            DoInstantiate(borderPrefab, new Vector3(x * gridOffset + startPosition.x, 0, size.y * gridOffset + startPosition.z), Quaternion.identity, this.transform);
         }
         for (int z = 0; z < size.y; z++)
         {
             DoInstantiate(borderPrefab, new Vector3(-1 * gridOffset, 0, z * gridOffset + startPosition.z), Quaternion.identity, this.transform);
             DoInstantiate(borderPrefab, new Vector3(size.x * gridOffset + startPosition.x, 0, z * gridOffset + startPosition.z), Quaternion.identity, this.transform);
         }
     }*/

    private void DoInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        Transform temp = ((GameObject)Instantiate(prefab, position, rotation)).transform;
        temp.parent = parent;
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
        List<Cell> cellWithLowestEntropy = new List<Cell>();
        int x = 100000;

        foreach (Cell c in cells)
        {
            if (!c.isCollapsed)
            {
                if (c.possiblePrototypes.Count == x)
                {
                    cellWithLowestEntropy.Add(c);
                }
                else if (c.possiblePrototypes.Count < x)
                {
                    cellWithLowestEntropy.Clear();
                    cellWithLowestEntropy.Add(c);
                    x = c.possiblePrototypes.Count;
                }
            }
        }
        return cellWithLowestEntropy[Random.Range(0, cellWithLowestEntropy.Count)];
    }
    private void CollapseAt(Cell cell)
    {
        int selectedPrototype = SelectPrototype(cell.prototypeWeights);
        Prototype finalPrototype = cell.possiblePrototypes[selectedPrototype];
        finalPrototype.prefab = cell.possiblePrototypes[selectedPrototype].prefab;
        cell.possiblePrototypes.Clear();
        cell.possiblePrototypes.Add(finalPrototype);
        GameObject finalPrefab = Instantiate(finalPrototype.prefab, cell.transform, true);
        if (finalPrototype.attributes[0].Equals(Attribute.Building)) places.Add(finalPrefab.transform);
        else
            streetElementsPlaces.Add(finalPrefab.transform);
        finalPrefab.transform.Rotate(new Vector3(0f, finalPrototype.meshRotation * 90, 0f), Space.Self);
        finalPrefab.transform.localPosition = Vector3.zero;
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

    private List<WFC_Socket> GetPossibleSockets(List<Prototype> possibleNeighbors)
    {
        List<WFC_Socket> socketsAccepted = new List<WFC_Socket>();
        foreach (Prototype proto in possibleNeighbors)
        {
            if (!socketsAccepted.Contains(proto.posX))
                socketsAccepted.Add(proto.posX);
            if (!socketsAccepted.Contains(proto.negX))
                socketsAccepted.Add(proto.negX);
            if (!socketsAccepted.Contains(proto.posZ))
                socketsAccepted.Add(proto.posZ);
            if (!socketsAccepted.Contains(proto.negZ))
                socketsAccepted.Add(proto.negZ);
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
    }

    public void PlaceBuildings()
    {
        float[] rotationOffset = {
        0f,
        90f,
        180f, 
        270f
    };

        Vector3[] rotationOffset2 = {
        new Vector3 (-5f, 270f, 1f),
        new Vector3 (0f, 0f, 0f),
        new Vector3 (1f, 90, -5f),
        new Vector3 (-5f, 180, -5f)
    };
        if (places.Count == 0)
        {
            Debug.LogWarning("List of free places is empty");
            return;
        }


        buildingGenerator.LoadField();

        foreach (var buildingSetting in buildingGenerator.buildingSettings)
        {
            for (int j = 0; j < buildingSetting.buildingCount; j++)
            {
                if (places.Count > 0)
                {
                    int index = rand.Next(0, places.Count);
                    int rotationIndex = rand.Next(0, 4);
                 //   var building = buildingGenerator.GenerateBuilding(new Vector3(0,0,0), Quaternion.Euler(0f, rotationOffset2[rotationIndex].y, 0f), buildingSetting);
                    var building = buildingGenerator.GenerateBuilding(new Vector3(0, 0, 0), Quaternion.identity, buildingSetting);
                   // Vector3 position = places[index].position - new Vector3(building.transform.localScale.x * -5f, 0f, building.transform.localScale.z * -5f)
                     //   + new Vector3(rotationOffset2[rotationIndex].x, 0, rotationOffset2[rotationIndex].z);
                    Vector3 position = places[index].position - new Vector3(building.transform.localScale.x * -5f, 0f, building.transform.localScale.z * -5f);
                    building.transform.position = position;
                    oldBuildingsPlaces.Add(position);
                    buildings.Add(building);
                    places.RemoveAt(index);
                }
            }
        }
    }

    public void PlaceNature()
    {
        if (places.Count == 0)
        {
            Debug.LogWarning("List of free places is empty");
            return;
        }
        if (places.Count > 0)
        {
            for (int i = 0; i < natureCount; i++)
            {
                int natureIndex = rand.Next(0, naturePrefabs.Count);
                int index = rand.Next(0, places.Count);
                GameObject nature = (GameObject)PrefabUtility.InstantiatePrefab(naturePrefabs[natureIndex] as GameObject);
                natures.Add(nature);
                nature.name = "Nature";
                nature.transform.position = places[index].position + new Vector3(0, 0, 3);
                oldNaturesPlaces.Add(places[index].position);
                places.RemoveAt(index);
            }
        }

    }

    public void RandomizeBuildingPositions()
    {
        List<Vector3> newPositions = new List<Vector3>(oldBuildingsPlaces);
        foreach (var building in buildings)
        {
            int index = Random.Range(0, newPositions.Count);
            if (index < newPositions.Count)
            {
                building.transform.position = newPositions[index];
                newPositions.RemoveAt(index);
            }
        }
    }

    public void RandomizeNaturePositions()
    {
        List<Vector3> newPositions = new List<Vector3>(oldNaturesPlaces);
        foreach (var nature in natures)
        {
            int index = Random.Range(0, newPositions.Count);
            if (index < newPositions.Count)
            {
                nature.transform.position = newPositions[index] + new Vector3(0, 0, 3);
                newPositions.RemoveAt(index);
            }
        }
    }

    public void RandomizeStreetElementsPositions()
    {
        List<Vector3> newPositions = new List<Vector3>(oldStreetElementsPlaces);
        foreach (var streetElement in streetElements)
        {
            int index = Random.Range(0, newPositions.Count);
            if (index < newPositions.Count)
            {
                streetElement.transform.position = newPositions[index];
                newPositions.RemoveAt(index);
            }
        }
    }

    public void PlaceStreetElements()
    {
        if (streetElementsPlaces.Count == 0)
        {
            Debug.LogWarning("List of free places is empty");
            return;
        }
        if (streetElementsPlaces.Count > 0)
        {
            for (int i=0; i< streetElementsCount; i++)
            {
                int streetElementIndex = rand.Next(0, streetPrefabs.Count);
                int index = rand.Next(0, streetElementsPlaces.Count);
                GameObject streetElement = (GameObject)PrefabUtility.InstantiatePrefab(streetPrefabs[streetElementIndex] as GameObject);
                streetElements.Add(streetElement);
                streetElement.name = "StreetElement";
                streetElement.transform.position = streetElementsPlaces[index].position + new Vector3(gridOffset / 2, 0, gridOffset / 2);
                oldStreetElementsPlaces.Add(streetElementsPlaces[index].position);
                streetElementsPlaces.RemoveAt(index);
            }
        }

    }
    public void DestroyBuildings()
    {
        buildings.Clear();
        oldBuildingsPlaces.Clear();
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        string objectName = "Building";
        foreach (GameObject obj in objects)
        {
            if (obj.name == objectName)
            {
                DestroyImmediate(obj);
            }
        }
    }

    public void DestroyNature()
    {
        natures.Clear();
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        string objectName = "Nature"; 
        foreach (GameObject obj in objects)
        {
            if (obj.name == objectName)
            {
                DestroyImmediate(obj);
            }
        }
    }

    public void DestroyStreetElements()
    {
        natures.Clear();
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        string objectName = "StreetElement";
        foreach (GameObject obj in objects)
        {
            if (obj.name == objectName)
            {
                DestroyImmediate(obj);
            }
        }
    }
}
#endif