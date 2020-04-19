using System.Collections.Generic;
using UnityEngine;

public class Rack : Interactable
{
    public int MaxCapacity { get; protected set; }
    protected List<RackModule> Modules;

    protected float Health;

    public RackModule.ModuleType CompatibleType { get; protected set; }

    void Start()
    {
        InteractableFlag = true;
        InsertableFlag = true;
        MaxCapacity = 0;
        Health = 100.0f;
    }

    void Update()
    {
        
    }

    public override bool IsInsertable() => InsertableFlag && (Modules.Count < MaxCapacity);

    public override bool InsertItem(GameObject item)
    {
        if (!IsInsertable())
            return false;

        RackModule module = item.GetComponent<RackModule>();
        
        if (module == null || module.Type == CompatibleType)
        {
            Debug.Log("This Module is Incompatible with This Rack");
        }


        //Check if module compatible if so add to server
        return false;
    }

    public override bool Interact()
    {
        throw new System.NotImplementedException();
    }

    public virtual int GetTotalOutput()
    {
        //Iterate through all rack modules and total their performance, then subtract any modifiers (Dustiness)

        return 0;
    }
}
