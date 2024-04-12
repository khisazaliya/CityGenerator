using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoadGenerator : MonoBehaviour
{
    public Transform[] controlPoints; // Управляющие точки кривой Безье
    public Mesh roadMesh; // Меш модуля дороги
    public float spacing = 1f; // Расстояние между модулями дороги

    private MeshFilter meshFilter;

    void Start()
    {
        GenerateRoad();
    }

    public void GenerateRoad()
    {
        if (controlPoints.Length < 2)
        {
            Debug.LogError("Необходимо указать как минимум 2 управляющие точки для кривой Безье!");
            return;
        }

        Vector3[] points = new Vector3[controlPoints.Length];
        for (int i = 0; i < controlPoints.Length; i++)
        {
            points[i] = controlPoints[i].position;
        }

        Vector3[] vertices = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            vertices[i] = points[i] + Vector3.up * Random.Range(-0.2f, 0.2f); // Случайное смещение вершин для реалистичности
        }

        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2((float)i / (uv.Length - 1), 0); // Простые UV-координаты
        }

        int[] triangles = new int[(vertices.Length - 1) * 6];
        int vert = 0;
        int tris = 0;
        for (int i = 0; i < vertices.Length - 1; i++)
        {
            triangles[tris] = vert + 0;
            triangles[tris + 1] = vert + 1;
            triangles[tris + 2] = vert + 2;
            triangles[tris + 3] = vert + 2;
            triangles[tris + 4] = vert + 1;
            triangles[tris + 5] = vert + 3;
            vert++;
            tris += 6;
        }

        Mesh mesh = roadMesh;

        // Создаем новый массив вершин с увеличенным размером и копируем в него старые вершины
        Vector3[] newVertices = new Vector3[vertices.Length + mesh.vertices.Length];
        vertices.CopyTo(newVertices, 0);
        mesh.vertices.CopyTo(newVertices, vertices.Length);

        // Создаем новый массив UV-координат с увеличенным размером и копируем в него старые координаты
        Vector2[] newUV = new Vector2[uv.Length + mesh.uv.Length];
        uv.CopyTo(newUV, 0);
        mesh.uv.CopyTo(newUV, uv.Length);

        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
