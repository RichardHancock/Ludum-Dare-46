using System.Collections.Generic;
using UnityEngine;

public abstract class Rack : Interactable
{
    public int MaxCapacity { get; protected set; }
    protected List<GameObject> Modules;

    protected float Health;

    public RackModule.ModuleType CompatibleType;

    public AnimationCurve ModuleAnimationCurve;

    void Start()
    {
        InteractableFlag = true;
        InsertableFlag = true;
        MaxCapacity = 0;
        Health = 100.0f;
    }

    void Update()
    {
        if (Modules.Count > 0)
        {
            //TODO Tweak Health Deg
            Health -= (0.05f * (float)Modules.Count) * Time.deltaTime;
        }

        if (Health < 50.0f)
        {
            Debug.Log("Low Health");
        }
        
    }

    public override bool IsInsertable() => InsertableFlag && (Modules.Count < MaxCapacity);

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

    public override bool Interact()
    {
        //TODO: Implement interact on server to resolve a failure or dust it
        throw new System.NotImplementedException();
    }

    public virtual int GetTotalOutput()
    {
        //Iterate through all rack modules and total their performance, then subtract any modifiers (Dustiness)
        //TODO: Implement once rack module is implemented
        int output = 0;

        foreach (GameObject obj in Modules)
        {
            output += obj.GetComponent<RackModule>().GetOutput();
        }

        return output;
    }

    //Check if the rack should experience a local failure, if so trigger a local failure event
    public virtual bool LocalFailCheck()
    {
        foreach (GameObject obj in Modules)
        {
            if (obj.GetComponent<RackModule>().LocalFailCheck())
            {
                //TODO Display Module Failure
                break;
            }
        }

        return true;
    }
}
