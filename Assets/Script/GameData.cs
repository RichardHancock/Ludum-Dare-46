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

    private Text MoneyText;

    public GameObject HardDrivePrefab;
    public GameObject CoreModulePrefab;
    public GameObject ComputeModulePrefab;

    public Transform StoreDeliveryPoint;

    public int HardDrivePrice;
    public int CoreModulePrice;
    public int ComputeModulePrice;

    public struct StoreItem
    {
        public int Price;
        public GameObject Prefab;

        public StoreItem(int price, GameObject prefab)
        {
            Price = price;
            Prefab = prefab;
        }
    }

    public Dictionary<RackModule.ModuleType, StoreItem> StoreData { get; private set; }

    protected void Start()
    {
        MoneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        Money = 1000;

        StoreData = new Dictionary<RackModule.ModuleType, StoreItem>()
        {
            { RackModule.ModuleType.Compute, new StoreItem(ComputeModulePrice, ComputeModulePrefab)},
            { RackModule.ModuleType.Core, new StoreItem(CoreModulePrice, CoreModulePrefab)},
            { RackModule.ModuleType.HardDrive, new StoreItem(HardDrivePrice, HardDrivePrefab)},
        };

        LoadTextures();

        if (HardDrivePrefab == null || CoreModulePrefab == null || ComputeModulePrefab == null)
        {
            Debug.LogError("Module Prefabs not assigned to GameData Object");
        }
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

    public bool BuyModule(RackModule.ModuleType type)
    {
        StoreItem item = StoreData[type];

        if (Money < item.Price)
        {
            //TODO Inform player they don't have enough money to buy module
            Debug.Log("Not enough money");
            return false;
        }

        //Charge the Money
        Money -= item.Price;

        //Spawn Item
        Instantiate(item.Prefab, StoreDeliveryPoint.position, Quaternion.identity);

        return true;
    }

    public void SellModule(RackModule.ModuleType type)
    {
        StoreItem item = StoreData[type];
        Money += item.Price;
    }
}
