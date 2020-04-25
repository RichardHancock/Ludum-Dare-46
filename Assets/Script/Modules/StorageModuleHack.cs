using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageModuleHack : MonoBehaviour
{
    private List<GameObject> hardDrives;

    private int maxCapacity = 12;

    private void Awake()
    {
        hardDrives = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        module.GetComponent<RackModule>().ActivateModule();
        module.SetActive(false);
        hardDrives.Add(module);
        

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
                return true;
            }
        }

        return false;
    }

    public virtual bool ResetFailedModuleIfExists()
    {
        foreach (GameObject obj in hardDrives)
        {
            RackModule rm = obj.GetComponent<RackModule>();
            if (!rm.Active)
            {
                rm.ResetModule();
                rm.ActivateModule();
                return true;
            }
        }

        return false;
    }

    private void UpdateTexture()
    {
        Texture2D texture = GameManager.Instance.StorageModuleTextures[hardDrives.Count];

        gameObject.GetComponent<Renderer>().material.mainTexture = texture;
    }
}
