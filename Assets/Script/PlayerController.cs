using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 5.0f;

    private Rigidbody rb;
    private BoxCollider interactionCollider;

    private GameObject heldItem = null;
    private GameObject itemInRange = null;

    public GameObject followCamera;
    public Vector3 followCamOffset = new Vector3(0.0f, 8.0f, -10.0f);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        interactionCollider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontalMovement, 0.0f, verticalMovement);
        rb.AddForce(movement * movementSpeed);
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
        }

        //Follow Camera Update
        Vector3 cameraPos = transform.position + followCamOffset;
        followCamera.transform.position = cameraPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
