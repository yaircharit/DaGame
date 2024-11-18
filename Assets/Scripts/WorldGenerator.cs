using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public static WorldGenerator currentWorld; // To have access to every block from anywhere

    [Header("Chunk Properties")]
    public int chunkLength;
    public int maxHeight;
    public int seaLevel;

    // maps
    public Dictionary<Vector2, Chunk> chunks;
    private float[,] heightMap;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject chunkPrefab;

    private Player player;

    public void Start()
    {
        currentWorld = this;
        
        chunks = new Dictionary<Vector2, Chunk>();
        
        chunks.Add(Vector2.zero, Instantiate(chunkPrefab).GetComponent<Chunk>());

        if (playerPrefab.GetComponent<Player>().spawnOnLoad)
            SpawnPlayer();
    }

    public void Update()
    {
        
    }


    /// <summary>
    /// Gets a chunk object that contains the block's position or null if does not exist
    /// </summary>
    /// <param name="position">Block's position</param>
    /// <returns>The chunk of this position</returns>
    public Chunk GetChunk(Vector3 position)
    {
        Vector2 diff = new Vector2((int)position.x % chunkLength, (int)position.y % chunkLength);
        Vector2 chunkPos = new Vector2((int)position.x - diff.x, (int)position.y - diff.y);
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
}
