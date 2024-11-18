using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    // General Properties
    public int chunkLength => WorldGenerator.currentWorld.chunkLength;
    public int maxHeight => WorldGenerator.currentWorld.maxHeight;
    public int seaLevel => WorldGenerator.currentWorld.seaLevel;

    // Map Generation Variables
    public float noiseScale; // TODO: generate from Noise.GenerateNoise(), have a height map influence the chunk's noise
    private Vector2 offsetPos;

    // Mesh variables
    MeshData meshData;

    // Functional variables
    public Dictionary<Vector3, Block> blocks;
    //private Chunk[] neighbors; //TODO: maybe
    //private bool wasModified; //TODO: to know if save/load chunk or generate

    public void Start()
    {
        //if ( (int)offset.x % chunkLength != 0 || (int)offset.y % chunkLength != 0 )
        //{
        //    Debug.LogError("Unsupported chunk offset :" + offset.ToString());
        //    return;
        //}
        offsetPos = Vector2.zero;
        blocks = new Dictionary<Vector3, Block>();

        meshData = new MeshData(this);
        Generate();
        meshData.UpdateMesh(blocks);

    }

    public void Generate()
    {
        //blocks.Add(Vector3.zero, new Block(this, Vector3.zero));
        Vector3 currBlockPos = new Vector3();

        for ( int i = 0; i < chunkLength; i++ )
        {
            for ( int j = 0; j < chunkLength; j++ )
            {
                for ( int k = 0; k < seaLevel; k++ ) // Fill all blocks up to seaLevel
                {
                    currBlockPos.Set(i, k, j);
                    blocks.Add(currBlockPos, new Block(currBlockPos));
                }
            }
        }

        foreach ( var block in blocks.Values )
        {
            block.SetNeighbors();
        }
    }

    public void OnDrawGizmos()
    {
        foreach ( var vert in meshData.vertices )
        {
            Gizmos.DrawSphere(vert, 0.2f);
        }
    }
}
