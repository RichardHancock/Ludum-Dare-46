using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeRack : Rack
{
    // Start is called before the first frame update
    void Start()
    {
        CompatibleType = RackModule.ModuleType.Compute;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
