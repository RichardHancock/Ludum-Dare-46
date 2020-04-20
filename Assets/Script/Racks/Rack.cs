﻿using System.Collections.Generic;
using UnityEngine;

public abstract class Rack : Interactable
{
    public int MaxCapacity { get; protected set; }
    protected List<GameObject> Modules;
    protected List<GameObject> BayPositions;
    protected float Health;

    public RackModule.ModuleType CompatibleType;

    public float AnimationStartPosOffset = 0.0001f;

    protected virtual void Start()
    {
        BayPositions = new List<GameObject>();
        Modules = new List<GameObject>();
        InteractableFlag = true;
        InsertableFlag = true;
        MaxCapacity = 0;
        Health = 100.0f;
    }

    protected virtual void Update()
    {
        if (Modules.Count > 0)
        {
            //TODO Tweak Health Deg
            Health -= (0.05f * (float)Modules.Count) * Time.deltaTime;
        }

        if (Health < 50.0f)
        {
            Debug.Log("Low Health");
        }
        
    }

    protected virtual void FindBays()
    {
        for (int i = 1; i <= MaxCapacity; i++)
        {
            Transform tform = gameObject.transform.Find("Bay" + i);

            if (tform == null)
                Debug.LogError("Couldn't Find a Bay in Storage Server");

            BayPositions.Add(tform.gameObject);
        }
    }

    // -1 on fail
    protected virtual int FindFirstAvailableBay()
    {
        int result = Modules.Count;

        return result < MaxCapacity ? result - 1 : -1;
    }

    public override bool IsInsertable() => InsertableFlag && (Modules.Count < MaxCapacity);

    public override bool InsertItem(GameObject item)
    {
        if (!IsInsertable())
            return false;

        RackModule module = item.GetComponent<RackModule>();
        
        //Check if compatible with this rack type
        if (module == null || module.Type != CompatibleType)
        {
            Debug.Log("This Module is Incompatible with This Rack");
            return false;
        }


        //int bay = FindFirstAvailableBay();
        //if (bay == -1)
            //Debug.LogError("No Available Bay THIS SHOULDN'T EVER Happen");

        //Add To Rack
        Modules.Add(item);

        //Animation Setup
        Transform bayPos = BayPositions[Modules.Count-1].transform;

        item.transform.position = bayPos.position + (bayPos.forward * -AnimationStartPosOffset);
        item.transform.rotation = Quaternion.identity;

        item.transform.SetParent(bayPos);

        module.SetAnimationPoints(item.transform.position, bayPos.position);


        module.ActivateModule();
        //TODO Sound
        return true;
    }

    public override bool Interact()
    {
        //TODO: Implement interact on server to resolve a failure or dust it
        throw new System.NotImplementedException();
    }

    public virtual int GetTotalOutput()
    {
        //Iterate through all rack modules and total their performance, then subtract any modifiers (Dustiness)
        int output = 0;

        foreach (GameObject obj in Modules)
        {
            output += obj.GetComponent<RackModule>().GetOutput();
        }

        return output;
    }

    //Check if the rack should experience a local failure, if so trigger a local failure event
    public virtual bool LocalFailCheck()
    {
        foreach (GameObject obj in Modules)
        {
            if (obj.GetComponent<RackModule>().LocalFailCheck())
            {
                //TODO Display Module Failure
                return true;
            }
        }

        return false;
    }
}
