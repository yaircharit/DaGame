using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    // Properties
    public Vector3 position;
    public BlockType type;
    
    // Outside Objects
    private Chunk container;
    public Block[] neighbors;

    // Mesh properties
    public Vector3[] vertices; 
    public Vector2[] uvs => BlockData.uvs;

    // Functions
    public Block(Chunk container,Vector3 position, BlockType type = BlockType.Grass)
    {
        this.position = position;
        this.type = type;

        vertices = BlockData.vertices.Plus(position);

        neighbors = new Block[6];
    }

    public void SetNeighbors()
    {
        Vector3 tempPos;
        for ( int i = 0; i < BlockData.neighbors.Length; i++ ) // 6 neighbors
        {
            tempPos = position + BlockData.neighbors[i];
            neighbors[i] = WorldGenerator.currentWorld.GetBlock(tempPos);
        }
    }
}
