using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 5.0f;

    private Rigidbody rb;
    private BoxCollider interactionCollider;

    private GameObject heldItem = null;
    private GameObject itemInPickupRange = null;
    private GameObject itemInInteractRange = null;
    public GameObject smallItemHoldPoint = null;
    public GameObject largeItemHoldPoint = null;

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
        if (Input.GetButtonDown("Pickup"))
        {
            if (heldItem == null)
            {
                PickUp();
            }
            else
            {
                Drop();
            }
        }
        else if (Input.GetButtonDown("Interact/Confirm"))
        {
            if (itemInInteractRange != null)
            {
                Debug.Log("Interaction with " + itemInInteractRange.name);
                itemInInteractRange.GetComponent<Interactable>().Interact();
            }
        }
    }

    private void Drop()
    {
        if (heldItem != null && itemInInteractRange != null)
        {
            Debug.Log("Item can drop");
        }
    }

    private void PickUp()
    {
        if (itemInPickupRange != null)
        {
            heldItem = itemInPickupRange;
            itemInPickupRange = null;

            heldItem.transform.rotation = transform.rotation;

            //Need to check if large or small item once implemented later
            heldItem.transform.SetParent(smallItemHoldPoint.transform);

            //Set gravity to false while holding it
            Rigidbody tmpRB = heldItem.GetComponent<Rigidbody>();
            tmpRB.isKinematic = true;
            tmpRB.detectCollisions = false;

            //Reset Rotation to zero
            heldItem.transform.localRotation = Quaternion.identity;
            //We re-position the ball on our guide object 
            heldItem.transform.position = smallItemHoldPoint.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            itemInPickupRange = other.gameObject;
        } 
        else if ( other.gameObject.CompareTag("Interactable"))
        {
            itemInInteractRange = other.gameObject;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            itemInPickupRange = null;
        }
        else if (other.gameObject.CompareTag("Interactable"))
        {
            itemInInteractRange = null;
        }
    }
}
