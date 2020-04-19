using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackModule : Pickup
{
    public enum ModuleType
    {
        HardDrive,
        Compute,
        Core
    }

    public ModuleType Type { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
