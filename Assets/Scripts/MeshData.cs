using System.Collections.Generic;
using System.Linq;
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

        //TODO: Clear local variables after storing in mesh
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();

        mesh.RecalculateNormals();
    }

    public void UpdateMesh(Dictionary<Vector3,Block> blocks)
    {
        int index = 0;

        foreach ( Block block in blocks.Values )
        {

            for ( int i = 0; i < 6; i++ )
            {
                for ( int j = 0; j < 6; j++ )
                {
                    if ( block.neighbors[i] == null )
                    {
                        triangles.Add(vertices.Count);

                        index = BlockData.triangles[i, j];
                        vertices.Add(block.vertices[index]);

                        uvs.Add(BlockData.uvs[j]);
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

