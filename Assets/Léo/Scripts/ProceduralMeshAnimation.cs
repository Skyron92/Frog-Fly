using UnityEngine;

public class ProceduralMeshAnimation : MonoBehaviour
{
    public MeshFilter meshFilter;
    public float waveAmplitude = 0.5f;
    public float waveFrequency = 1f;

    private Vector3[] baseVertices;

    void Start()
    {
        baseVertices = meshFilter.mesh.vertices;
    }

    void Update()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            float distance = Vector3.Distance(vertices[i], Vector3.zero);
            vertices[i].y = baseVertices[i].y + Mathf.Sin(Time.time * waveFrequency + distance) * waveAmplitude;
        }
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
    }
}

