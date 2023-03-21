using UnityEngine;

public class ProceduralMesh : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 2f;

    private MeshFilter meshFilter;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;

    void Start() {
        meshFilter = GetComponent<MeshFilter>();
        originalVertices = meshFilter.mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    void Update() {
        for (int i = 0; i < originalVertices.Length; i++) {
            Vector3 vertex = originalVertices[i];
            vertex.y = Mathf.Sin(Time.time * frequency + vertex.x) * amplitude;
            displacedVertices[i] = vertex;
        }

        meshFilter.mesh.vertices = displacedVertices;
        meshFilter.mesh.RecalculateNormals();
    }
}

