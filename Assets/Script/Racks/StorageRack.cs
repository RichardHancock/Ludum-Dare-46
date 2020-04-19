using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageRack : Rack
{
    // Start is called before the first frame update
    void Start()
    {
        CompatibleType = RackModule.ModuleType.HardDrive;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

        //Add To Rack
        Modules.Add(item);
        //TODO Animation, Sound, Transform
        return true;
    }
}
