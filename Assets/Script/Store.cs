using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : Interactable
{
    private GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();

        InteractableFlag = true;
        InsertableFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool Interact()
    {
        //TODO Toggle Store UI
        gameData.Money-=1;
        Debug.Log("Money: " + gameData.Money);
        return false;
    }

    public override bool InsertItem(GameObject item)
    {
        gameData.SellModule(item.GetComponent<RackModule>().Type);

        Destroy(item);

        return true;
    }
}
