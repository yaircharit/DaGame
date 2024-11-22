using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;
    public Mesh mesh;

    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;

    public bool isCollidable;

    public MeshData(Chunk chunk)
    {
        isCollidable = true;

        meshFilter = chunk.GetComponent<MeshFilter>();
        meshCollider = chunk.GetComponent<MeshCollider>();

        mesh = new Mesh();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
    }

    private void GenerateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        vertices.Clear();
        triangles.Clear();
        uvs.Clear();

        mesh.RecalculateNormals();
    }

    public void UpdateMesh(Dictionary<Vector3, Block> blocks)
    {
        int[] currVerts = new int[4];
        int temp;

        foreach ( Block block in blocks.Values )
        {
            for ( int i = 0; i < 6; i++ ) // Each side
            {
                if ( block.neighbors[i] == null ) // Need to render this side
                {
                    for ( int j = 0; j < 4; j++ ) // Each vertex of this side
                    {
                        temp = vertices.FindIndex(block.vertices[BlockData.sideVertices[i, j]].Equals); // Search for current vertex
                        if ( temp == -1 )
                        {
                            // Add relevant side's verteces
                            currVerts[j] = vertices.Count;
                            vertices.Add(block.vertices[BlockData.sideVertices[i, j]]);
                            uvs.Add(BlockData.uvs[j]); // Add texture data
                        } else
                        {
                            // Reference existing vertex for triangles loop
                            currVerts[j] = temp;
                        }
                    }

                    int vertCount = vertices.Count;
                    foreach ( int index in BlockData.trianglesTemplate )
                    {
                        // Connect side into triangles;
                        triangles.Add(currVerts[index]);
                    }
                }
            }
        }

        GenerateMesh();

        meshFilter.sharedMesh = mesh;
        if ( isCollidable )
        {
            meshCollider.sharedMesh = mesh;
        }

        Debug.Log("Verteces count: " + mesh.vertices.Length);
        Debug.Log("Triangles count: " + mesh.triangles.Length / 3);
    }

    public void OnDrawGizmos()
    {
        foreach ( var vert in mesh.vertices )
        {
            Gizmos.DrawSphere(vert, 0.1f);
        }
    }
}

