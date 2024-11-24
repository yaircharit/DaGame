using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    public float speed;
    public float jumpSpeed;
    public float rotationSpeed;
    public float maxSpeed;
    public bool spawnOnLoad;
    public int chunkLoadMargin;

    public Vector3 position => transform.position;
    public Chunk currentChunk;

    new public Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Load chunks around player
        currentChunk = WorldGenerator.current.GetChunk(position);
        if ( currentChunk == null )
        {
            currentChunk = WorldGenerator.current.InstantiateChunk(position);
            //currentChunk.LoadNeighborhood();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( Move() )
        {
            // Check if on the edge of a chunk
            Chunk tempChunk = WorldGenerator.current.GetChunk(position);
            if ( currentChunk != tempChunk )
            {
                // TODO: load new chunks
                WorldGenerator.current.LoadNeighborhood(tempChunk);
                currentChunk = tempChunk;
            }
        }
    }

    /// <summary>
    /// Gets user inputs and applies velocity accordingly
    /// </summary>
    /// <returns>true if the player is moving (velocity != 0)</returns>
    private bool Move()
    {
        // Get X and Z axis inputs
        Vector3 move = new Vector3(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);
        if ( Input.GetKey(KeyCode.Space) )
        {
            // Get Y axis input (jump)
            move.y = jumpSpeed;
        }
        rigidbody.velocity += move * Time.deltaTime; // Add velocity accordingly

        float rotation = Input.GetAxis("Rotation"); // Get camera/player rotation inputs
        transform.eulerAngles += new Vector3(0, rotation * rotationSpeed * Time.deltaTime, 0); // Rotate camera/player

        

        return rigidbody.velocity != Vector3.zero;
    }
}
