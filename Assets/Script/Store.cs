using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        InteractableFlag = true;
        InsertableFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Money > 999)
        {
            
        }
    }

    public override bool Interact()
    {
        GameData.Money-=1;
        Debug.Log("Money: " + GameData.Money);
        return false;
    }

    public override bool InsertItem(GameObject item)
    {
        Debug.LogWarning("InsertItem called on Store");
        return false;
    }
}
