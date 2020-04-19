using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RackModule : Pickup
{
    [System.Serializable]
    public enum ModuleType
    {
        HardDrive,
        Compute,
        Core
    }

    public ModuleType Type;

    protected int RawOutput;

    protected int Age = 0;

    private bool Active = false;

    public int FailureThresholdMin;
    public int FailureThresholdMax;
    public int FailurePoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public void ActivateModule()
    {
        if (Active)
            return;

        InvokeRepeating("AgeModule", 1.0f, 10.0f);
    }

    public void ResetModule()
    {
        Active = false;
        Age = 0;
    }

    private void AgeModule()
    {
        Age += 1;
    }

    public bool LocalFailCheck()
    {
        if (!Active)
            return false;

        if ((Random.Range(FailureThresholdMin, FailureThresholdMax) + Age) > FailurePoint)
        {
            //TODO Display Failure and Test Algorithm
            Debug.Log(gameObject.name + "Failed");
            CancelInvoke();
            Active = false;
            return true;
        }

        return false;
    }

    public virtual int GetOutput()
    {
        //TODO (Maybe Done)have output get affected by failures
        if (!Active)
            return 0;

        return RawOutput;
    }
}
