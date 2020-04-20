using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    private int money;
    public int Money { 
        get
        {
            return money;
        }
        set
        {
            money = value;

            UpdateMoneyText();
        }
    }

    public List<Texture2D> StorageModuleTextures { get; private set; }
    private readonly string textureNameBase = "Textures/StorageServer/server_component_hdd_uv_";

    public Text MoneyText;

    protected void Start()
    {
        MoneyText = GameObject.Find("MoneyText").GetComponent<Text>();

        Money = 1000;

        LoadTextures();
    }

    private void UpdateMoneyText()
    {
        if (MoneyText == null)
        {
            Debug.LogError("Update Money Text called before text object fetched");
            return;
        }

        MoneyText.text = "£" + Money;
    }

    private void LoadTextures()
    {
        StorageModuleTextures = new List<Texture2D>();

        for (int i = 0; i <= 12; i++)
        {
            Texture2D texture = Resources.Load<Texture2D>(textureNameBase + i);

            if (texture == null)
                Debug.LogError("Could not load Storage Module Texture " + i);

            StorageModuleTextures.Add(texture);
        }
    }
}
