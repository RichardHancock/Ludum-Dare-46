﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeModule : RackModule
{
    // Start is called before the first frame update
    void Start()
    {
        Type = ModuleType.Compute;
        Animated = true;
    }

}
