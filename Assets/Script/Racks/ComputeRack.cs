using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeRack : Rack
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        CompatibleType = RackModule.ModuleType.Compute;
        MaxCapacity = 4;
    }
}
