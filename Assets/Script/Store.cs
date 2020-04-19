using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.money > 999)
        {
            
        }
    }

    public override bool Interact()
    {
        GameData.money-=1;
        Debug.Log("Money: " + GameData.money);
        return false;
    }

    public override bool InsertItem(GameObject item)
    {
        Debug.LogWarning("InsertItem called on Store");
        return false;
    }
}
