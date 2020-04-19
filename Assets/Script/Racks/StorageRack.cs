using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageRack : Rack
{
    // Start is called before the first frame update
    protected override void Start()
    {
        Modules = new List<GameObject>();
        CompatibleType = RackModule.ModuleType.HardDrive;
        MaxCapacity = 4;
        InteractableFlag = true;
        InsertableFlag = true;

        //Get all storage modules and store them in order
        for (int i = 1; i <= MaxCapacity; i++)
        {
            Transform tform = gameObject.transform.Find("Bay" + i);

            if (tform == null)
                Debug.LogError("Couldn't Find a Bay in Storage Server");

            tform = tform.GetChild(0);
            
            if (tform == null)
                Debug.LogError("Couldn't Find a Module under Bay in Storage Server");

            Modules.Insert(i - 1, tform.gameObject);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        //This is here to disable health
    }

    //-1 if none available
    protected int findIndexOfFirstAvailableStorageBay()
    {
        for (int i = 0; i < MaxCapacity; i++)
        {
            if (Modules[i].GetComponent<StorageModuleHack>().isInsertable())
            {
                return i;
            }
        }

        return -1;
    }

    public override bool IsInsertable() => (findIndexOfFirstAvailableStorageBay() != -1);

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


        int bayIndex = findIndexOfFirstAvailableStorageBay();

        //Add To Rack
        Modules[bayIndex].GetComponent<StorageModuleHack>().AddHardDrive(item);

        //TODO Animation, Sound, Transform
        return true;
    }

    public override bool Interact()
    {
        //TODO: Implement interact on server to resolve a failure or dust it
        throw new System.NotImplementedException();
    }

    public override int GetTotalOutput()
    {
        //Iterate through all rack modules and total their performance, then subtract any modifiers (Dustiness)
        
        int output = 0;

        foreach (GameObject obj in Modules)
        {
            output += obj.GetComponent<StorageModuleHack>().GetTotalOutput();
        }

        return output;
    }

    public override bool LocalFailCheck()
    {
        foreach (GameObject obj in Modules)
        {
            if (obj.GetComponent<StorageModuleHack>().LocalFailCheck())
            {
                //TODO Display Module Failure
                return true;
            }
        }

        return false;
    }
}
