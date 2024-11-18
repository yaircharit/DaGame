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

    new public Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);
        if ( Input.GetKey(KeyCode.Space) )
        {
            move.y = jumpSpeed;
        }

        rigidbody.velocity += move * Time.deltaTime;   

        float rotation = Input.GetAxis("Rotation");

        transform.eulerAngles += new Vector3(0, rotation * rotationSpeed * Time.deltaTime, 0);
    }
}
