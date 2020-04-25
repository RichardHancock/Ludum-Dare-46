using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameData gameData;

    [SerializeField] 
    private float movementSpeed = 5.0f;

    private Rigidbody rb;

    private GameObject heldItem, itemInPickupRange, itemInInteractRange;

    [SerializeField] 
    private GameObject smallItemHoldPoint, largeItemHoldPoint;

    [SerializeField] 
    private GameObject followCamera;
    [SerializeField] 
    private Vector3 followCamOffset = new Vector3(0.0f, 6.0f, -5.0f);

    private float horizontalMovement, verticalMovement;

    private void Awake()
    {
        gameData = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (gameData.DisableInput)
            return;

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
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        if (!gameData.DisableInput && Input.GetButtonDown("Pickup"))
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
        else if (!gameData.DisableInput && Input.GetButtonDown("Interact/Confirm"))
        {
            if (itemInInteractRange != null)
            {
                itemInInteractRange.GetComponent<Interactable>().Interact();
            }
        }
    }

    private void Drop()
    {
        if (heldItem == null || itemInInteractRange == null)
        {
            gameData.PlayerAudio.clip = gameData.Error;
            gameData.PlayerAudio.Play();
            return;
        }
            

        if (itemInInteractRange.GetComponent<Interactable>().InsertItem(heldItem))
        {
            //Item Placed Successfully
            heldItem = null;
        }
        else
        {
            gameData.PlayerAudio.clip = gameData.Error;
            gameData.PlayerAudio.Play();
        }
    }

    private void PickUp()
    {
        if (itemInPickupRange != null)
        {
            heldItem = itemInPickupRange;
            itemInPickupRange = null;

            Rigidbody tmpRB = heldItem.GetComponent<Rigidbody>();
            tmpRB.isKinematic = true;
            tmpRB.detectCollisions = false;

            bool largeItem = heldItem.GetComponent<RackModule>().LargeItem;

            GameObject holdPoint = (largeItem ? largeItemHoldPoint : smallItemHoldPoint);


            heldItem.transform.rotation = transform.rotation;
            heldItem.transform.position = holdPoint.transform.position;

            //Need to check if large or small item once implemented later
            heldItem.transform.SetParent(holdPoint.transform, true);

            //Make Non Pickupable
            heldItem.tag = "Untagged";

            //Set gravity to false while holding it
            

            //Reset Rotation to zero
            //HeldItem.transform.localRotation = Quaternion.identity;
            //We re-position the ball on our guide object 
            heldItem.transform.localPosition = Vector3.zero;

            if(largeItem)
                heldItem.transform.Rotate(90, 0, 0);
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
