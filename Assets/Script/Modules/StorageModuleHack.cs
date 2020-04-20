﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageModuleHack : MonoBehaviour
{
    private List<GameObject> hardDrives;

    private int maxCapacity = 12;

    // Start is called before the first frame update
    void Start()
    {
        hardDrives = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddHardDrive(GameObject module)
    {
        if (!isInsertable())
            return false;

        //Module validity is checked in calling class (HACKY I KNOW)
        hardDrives.Add(module);
        module.SetActive(false);

        UpdateTexture();

        return true;
    }

    public bool isInsertable() => hardDrives.Count < maxCapacity;

    public int GetTotalOutput()
    {
        int output = 0;

        foreach (GameObject hd in hardDrives)
        {
            output += hd.GetComponent<RackModule>().GetOutput();
        }

        return output;
    }

    internal bool LocalFailCheck()
    {
        foreach (GameObject hd in hardDrives)
        {
            if (hd.GetComponent<RackModule>().LocalFailCheck())
            {
                //TODO Display Module Failure
                return true;
            }
        }

        return false;
    }

    private void UpdateTexture()
    {
        Texture2D texture = GameData.storageModuleTextures[hardDrives.Count];

        gameObject.GetComponent<Renderer>().material.mainTexture = texture;
    }
}