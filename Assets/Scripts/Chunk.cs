using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    // General Properties
    public int chunkLength => WorldGenerator.current.chunkLength;
    public int maxHeight => WorldGenerator.current.maxHeight;

    // Map Generation Variables
    public float noiseScale; // TODO: generate from Noise.GenerateNoise(), have a height map influence the chunk's noise
    private Vector2 position;

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
        position = Vector2.zero; //TODO: pass position somehow
        blocks = new Dictionary<Vector3, Block>();
        meshData = new MeshData(this);


        noiseScale = WorldGenerator.GenerateHeight(position);
        Debug.Log("Chunk noise: " +noiseScale);

        Generate();
        meshData.UpdateMesh(blocks);

    }

    public void Generate()
    {
        Vector3 currBlockPos = new Vector3();

        for ( int i = 0; i < chunkLength; i++ )
        {
            for ( int j = 0; j < chunkLength; j++ )
            {
                int height = GenerateBlockHeight(i, j);
                for ( int k = 0; k < height; k++ ) // Fill all blocks up to seaLevel
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

    private int GenerateBlockHeight(Vector2 blockPos)
    {
        var res = Noise.GenerateNoise(blockPos,noiseScale,position,maxHeight);
        Debug.Log(res);
        return Mathf.FloorToInt(res);
    }

    private int GenerateBlockHeight(float x, float y)
    {
        return GenerateBlockHeight(new Vector2(x,y));
    }

    public void OnDrawGizmos()
    {
        foreach ( var vert in meshData.vertices )
        {
            Gizmos.DrawSphere(vert, 0.2f);
        }
    }
}
