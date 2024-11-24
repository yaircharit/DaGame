using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public static WorldGenerator current; // To have access to every block from anywhere

    [Header("Chunk Properties")]
    public int chunkLength;
    public int maxHeight;
    // maps
    public Dictionary<Vector2, Chunk> chunks;
    public List<Chunk> loadedChunks;
    public float noiseScale;    // Effects terrain smoothness
    private float[,] heightMap;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject chunkPrefab;

    private Player player;

    public void Start()
    {
        current = this;
        
        chunks = new Dictionary<Vector2, Chunk>();
        loadedChunks = new List<Chunk>();

        LoadNeighborhood(Vector2.zero);
        SpawnPlayer();
    }

    public void Update()
    {
        
    }

    public void LoadChunks(List<Chunk> toLoad)
    {
        var toRemove = loadedChunks.Where((c) => !toLoad.Contains(c));
        foreach ( var chunk in toRemove ) 
        {
            chunk.ClearData();
        }

        var toAdd = toLoad.Where((c) => !loadedChunks.Contains(c));
        foreach ( var chunk in toAdd ) // TODO: chunk in toLoad which is not in loadedChunks
        {
            chunk.Generate();
            chunk.UpdateMesh();
        }

        loadedChunks = toLoad;
    }

    public void LoadNeighborhood(Chunk tempChunk)
    {
        LoadNeighborhood(tempChunk.chunkPos);
    }

    public void LoadNeighborhood(Vector2 chunkPos, int distance = 1)
    {
        LoadChunks(GetNeighborhood(chunkPos, distance));
    }

    private List<Chunk> GetNeighborhood(Vector2 chunkPos, int distance = 1)
    {
        
        List<Chunk> res = new List<Chunk>();
        Chunk temp;
        Vector2 temp2 = new Vector2();

        for ( int i = -distance; i <= distance; i++ )
        {
            for ( int j = -distance; j <= distance; j++ )
            {
                temp2.x = chunkPos.x + i;
                temp2.y = chunkPos.y + j;
                temp = chunks.GetValueOrDefault(temp2);
                if (temp == null )
                {
                    temp = InstantiateChunk(temp2); // Adds also to dict
                }
                res.Add(temp);
            }
        }

        return res;
    }
    public Chunk InstantiateChunk(Vector3 worldPosition)
    {
        return InstantiateChunk(new Vector2((int)worldPosition.x/chunkLength, (int)worldPosition.z/chunkLength));
    }

    public Chunk InstantiateChunk(Vector2 chunkPosition)
    {
        Chunk res = Instantiate(chunkPrefab,transform).GetComponent<Chunk>(); // Create new Chunk
        res.Init(chunkPosition);
        chunks.Add(chunkPosition, res);
        return res;
    }

    /// <summary>
    /// Gets a chunk object that contains the block's position or null if does not exist
    /// </summary>
    /// <param name="position">Block's position</param>
    /// <returns>The chunk of this position</returns>
    public Chunk GetChunk(Vector3 position)
    {
        Vector2 chunkPos = new Vector2((int)position.x / chunkLength, (int)position.y / chunkLength);
        return chunks.GetValueOrDefault(chunkPos);
    }

    /// <summary>
    /// Gets Block object or null if empty
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Block GetBlock(Vector3 position)
    {
        Chunk chunk = GetChunk(position);
        if ( chunk == null )
            return null;

        return chunk.blocks.GetValueOrDefault(position);
    }

    private void SpawnPlayer()
    {
        SpawnPlayer(Vector3.up * 15);
    }

    //TODO: Orgenize mesh generation / game logic
    private void SpawnPlayer(Vector3 spawnPos)
    {
        // TODO: Kill player if needed?

        player = Instantiate(playerPrefab, spawnPos.Add(0.5f),new Quaternion()).GetComponent<Player>();
    }

    public static float GenerateHeight(Vector2 position)
    {
        return Noise.GenerateNoise(Vector2.one,current.chunkLength,position,current.noiseScale);
    }
}
