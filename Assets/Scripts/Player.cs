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

    }

    // Update is called once per frame
    void Update()
    {
        if ( Move() )
        {
            // Check if on the edge of a chunk
            Chunk tempChunk = WorldGenerator.current.GetChunk(position);
            if ( tempChunk == null )
            {
                tempChunk = WorldGenerator.current.InstantiateChunk(position);
            }

            if ( currentChunk != tempChunk )
            {
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
        Vector3 move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        if ( Input.GetKey(KeyCode.Space) ) //TODO: check if grounded
        {
            // Get Y axis input (jump)
            move.y = jumpSpeed;
        }

        if ( move != Vector3.zero )
        {
            rigidbody.velocity += move * speed * Time.deltaTime ;
        }


        float rotation = Input.GetAxis("Rotation"); // Get camera/player rotation inputs
        transform.eulerAngles += new Vector3(0, rotation * rotationSpeed * Time.deltaTime, 0); // Rotate camera/player



        return rigidbody.velocity != Vector3.zero;
    }
}
