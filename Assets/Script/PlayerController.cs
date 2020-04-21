using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    GameData gameData;

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
        gameData = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();

        RB = GetComponent<Rigidbody>();

        
    }

    void FixedUpdate()
    {
        if (gameData.DisableInput)
            return;

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
        if (!gameData.DisableInput && Input.GetButtonDown("Pickup"))
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
        else if (!gameData.DisableInput && Input.GetButtonDown("Interact/Confirm"))
        {
            if (ItemInInteractRange != null)
            {
                ItemInInteractRange.GetComponent<Interactable>().Interact();
            }
        }
    }

    private void Drop()
    {
        if (HeldItem == null || ItemInInteractRange == null)
        {
            gameData.PlayerAudio.clip = gameData.Error;
            gameData.PlayerAudio.Play();
            return;
        }
            

        if (ItemInInteractRange.GetComponent<Interactable>().InsertItem(HeldItem))
        {
            //Item Placed Successfully
            HeldItem = null;
        }
        else
        {
            gameData.PlayerAudio.clip = gameData.Error;
            gameData.PlayerAudio.Play();
        }
    }

    private void PickUp()
    {
        if (ItemInPickupRange != null)
        {
            HeldItem = ItemInPickupRange;
            ItemInPickupRange = null;

            Rigidbody tmpRB = HeldItem.GetComponent<Rigidbody>();
            tmpRB.isKinematic = true;
            tmpRB.detectCollisions = false;

            bool largeItem = HeldItem.GetComponent<RackModule>().LargeItem;

            GameObject holdPoint = (largeItem ? LargeItemHoldPoint : SmallItemHoldPoint);


            HeldItem.transform.rotation = transform.rotation;
            HeldItem.transform.position = holdPoint.transform.position;

            //Need to check if large or small item once implemented later
            HeldItem.transform.SetParent(holdPoint.transform, true);

            //Make Non Pickupable
            HeldItem.tag = "Untagged";

            //Set gravity to false while holding it
            

            //Reset Rotation to zero
            //HeldItem.transform.localRotation = Quaternion.identity;
            //We re-position the ball on our guide object 
            HeldItem.transform.localPosition = Vector3.zero;

            if(largeItem)
                HeldItem.transform.Rotate(90, 0, 0);
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
