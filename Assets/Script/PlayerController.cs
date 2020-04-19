using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float MovementSpeed = 5.0f;

    private Rigidbody RB;

    private GameObject HeldItem = null;
    private GameObject ItemInPickupRange = null;
    private GameObject ItemInInteractRange = null;
    public GameObject SmallItemHoldPoint = null;
    public GameObject LargeItemHoldPoint = null;

    public GameObject FollowCamera;
    public Vector3 FollowCamOffset = new Vector3(0.0f, 8.0f, -10.0f);

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontalMovement, 0.0f, verticalMovement);
        RB.AddForce(movement * MovementSpeed);
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
        }

        //Follow Camera Update
        Vector3 cameraPos = transform.position + FollowCamOffset;
        FollowCamera.transform.position = cameraPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pickup"))
        {
            if (HeldItem == null)
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
            if (ItemInInteractRange != null)
            {
                Debug.Log("Interaction with " + ItemInInteractRange.name);
                ItemInInteractRange.GetComponent<Interactable>().Interact();
            }
        }
    }

    private void Drop()
    {
        if (HeldItem == null || ItemInInteractRange == null)
            return;

        if (ItemInInteractRange.GetComponent<Interactable>().InsertItem(HeldItem))
        {
            //Item Placed Successfully
            HeldItem = null;
        }
        else
        {
            //TODO Error Noise
        }
    }

    private void PickUp()
    {
        if (ItemInPickupRange != null)
        {
            HeldItem = ItemInPickupRange;
            ItemInPickupRange = null;

            HeldItem.transform.rotation = transform.rotation;

            //Need to check if large or small item once implemented later
            HeldItem.transform.SetParent(SmallItemHoldPoint.transform);

            //Make Non Pickupable
            HeldItem.tag = "";

            //Set gravity to false while holding it
            Rigidbody tmpRB = HeldItem.GetComponent<Rigidbody>();
            tmpRB.isKinematic = true;
            tmpRB.detectCollisions = false;

            //Reset Rotation to zero
            HeldItem.transform.localRotation = Quaternion.identity;
            //We re-position the ball on our guide object 
            HeldItem.transform.position = SmallItemHoldPoint.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            ItemInPickupRange = other.gameObject;
        } 
        else if ( other.gameObject.CompareTag("Interactable"))
        {
            ItemInInteractRange = other.gameObject;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            ItemInPickupRange = null;
        }
        else if (other.gameObject.CompareTag("Interactable"))
        {
            ItemInInteractRange = null;
        }
    }
}
