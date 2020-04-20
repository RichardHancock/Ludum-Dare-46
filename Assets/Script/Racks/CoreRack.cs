using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreRack : Rack
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        CompatibleType = RackModule.ModuleType.Core;
        MaxCapacity = 4;

        //This needs to be here rather than Base class to use different Capacity
        FindBays();
    }
}
