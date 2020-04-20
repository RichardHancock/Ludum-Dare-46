using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private readonly string textureNameBase = "Textures/StorageServer/server_component_hdd_uv_";
    
    // Start is called before the first frame update
    void Start()
    {
        GameData.Money = 1000;

        LoadTextures();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadTextures()
    {
        GameData.storageModuleTextures = new List<Texture2D>();

        for (int i = 0; i <= 12; i++)
        {
            Texture2D texture = Resources.Load<Texture2D>(textureNameBase + i);

            if (texture == null)
                Debug.LogError("Could not load Storage Module Texture " + i);

            GameData.storageModuleTextures.Add(texture);
        }
    }
}
