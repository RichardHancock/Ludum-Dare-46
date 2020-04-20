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
        InsertableFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameData.Money > 999)
        {
            
        }
    }

    public override bool Interact()
    {
        gameData.Money-=1;
        Debug.Log("Money: " + gameData.Money);
        return false;
    }

    public override bool InsertItem(GameObject item)
    {
        Debug.LogWarning("InsertItem called on Store");
        return false;
    }
}
