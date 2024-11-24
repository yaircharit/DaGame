using System;
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
    public float noiseScale; 
    public Vector2 chunkPos;

    // Mesh variables
    MeshData meshData;
    bool isLoaded;

    // Functional variables
    public Dictionary<Vector3, Block> blocks;
    public bool wasModified; //TODO: to know if save/load chunk or generate

    public void Start()
    {
        
    }

    public void Init(Vector2 pos)
    {
        wasModified = false;
        chunkPos = pos;
        gameObject.transform.position = new Vector3(chunkPos.x*chunkLength,0,chunkPos.y*chunkLength);
        blocks = new Dictionary<Vector3, Block>();
        meshData = new MeshData(this);

        noiseScale = WorldGenerator.GenerateHeight(chunkPos);
        Debug.Log("Chunk noise: " + noiseScale);
    }

    public bool IsInChunk(Vector3 worldPosition)
    {
        return (worldPosition.x / chunkLength == chunkPos.x && worldPosition.y / chunkLength == chunkPos.y);
    }

    public void UpdateMesh()
    {
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

    public void ClearData()
    {
        if ( !wasModified )
        {
            blocks.Clear();
        }

        meshData.Clear();
    }

    public void LoadNeighborhood()
    {
        WorldGenerator.current.LoadNeighborhood(chunkPos);
    }

    private int GenerateBlockHeight(Vector2 blockPos)
    {
        var res = Noise.GenerateNoise(blockPos, noiseScale, chunkPos, maxHeight);
        //Debug.Log(res);
        return Mathf.FloorToInt(res);
    }

    private int GenerateBlockHeight(float x, float y)
    {
        return GenerateBlockHeight(new Vector2(x, y));
    }

    public void OnDrawGizmos()
    {
        foreach ( var vert in meshData.vertices )
        {
            Gizmos.DrawSphere(vert, 0.2f);
        }
    }
}
