using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeRack : Rack
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        CompatibleType = RackModule.ModuleType.Compute;
        MaxCapacity = 4;

        //This needs to be here rather than Base class to use different Capacity
        FindBays();
    }
}
