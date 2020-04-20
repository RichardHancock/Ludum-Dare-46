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

    public GameObject StoreDeliveryPoint;

    public int HardDrivePrice;
    public int CoreModulePrice;
    public int ComputeModulePrice;

    [HideInInspector]
    public bool DisableInput = false;
    private GameObject gameUI;

    public AudioSource PlayerAudio;

    public AudioClip Error;
    public AudioClip Buy;
    public AudioClip ServerStart;
    public AudioClip ServerFail;


    public struct StoreItem
    {
        public int Price;
        public GameObject Prefab;
        public string Name;
        public Texture2D Image;

        public StoreItem(int price, GameObject prefab, string name, Texture2D image)
        {
            Price = price;
            Prefab = prefab;
            Name = name;
            Image = image;
        }
    }

    public Dictionary<RackModule.ModuleType, StoreItem> StoreData { get; private set; }

    private List<Rack> storageRacks;
    private List<Rack> coreRacks;
    private List<Rack> computeRacks;


    protected void Start()
    {
        FindRacks();

        gameUI = GameObject.Find("GameUI");

        MoneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        Money = 1000;



        StoreData = new Dictionary<RackModule.ModuleType, StoreItem>()
        {
            { RackModule.ModuleType.Compute, new StoreItem(ComputeModulePrice, ComputeModulePrefab, "Compute Server", Resources.Load<Texture2D>("Textures/compute_image"))},
            { RackModule.ModuleType.Core, new StoreItem(CoreModulePrice, CoreModulePrefab, "Core Server", Resources.Load<Texture2D>("Textures/core_image"))},
            { RackModule.ModuleType.HardDrive, new StoreItem(HardDrivePrice, HardDrivePrefab, "Hard Drive", Resources.Load<Texture2D>("Textures/hdd_image"))},
        };

        LoadTextures();

        if (HardDrivePrefab == null || CoreModulePrefab == null || ComputeModulePrefab == null)
        {
            Debug.LogError("Module Prefabs not assigned to GameData Object");
        }

        CreateInitialServerLoadout();

        gameObject.GetComponent<ScoreSystem>().Begin();
    }

    private void Update()
    {
        if (Input.GetButtonDown("TestKey"))
        {
            foreach (StorageRack rack in storageRacks)
            {
                if (rack.LocalFailCheck())
                {
                    return;
                }
            }

            foreach (ComputeRack rack in computeRacks)
            {
                if (rack.LocalFailCheck())
                {
                    return;
                }
            }

            foreach (CoreRack rack in coreRacks)
            {
                if (rack.LocalFailCheck())
                {
                    return;
                }
            }
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

        /*
        if (Money < item.Price)
        {
            
            Debug.Log("Not enough money");
            return false;
        }
        */

        if (StoreDeliveryPoint.GetComponent<IsTriggered>().Triggered())
        {
            //TODO Inform player the Spawn is blocked
            PlayerAudio.clip = Error;
            PlayerAudio.Play();
            Debug.Log("Item in way of store spawn");
            return false;
        }

        //Charge the Money
        Money -= item.Price;

        PlayerAudio.clip = Buy;
        PlayerAudio.Play();

        //Spawn Item
        Instantiate(item.Prefab, StoreDeliveryPoint.transform.position, Quaternion.identity);

        return true;
    }

    public void SellModule(RackModule.ModuleType type)
    {
        StoreItem item = StoreData[type];
        Money += item.Price;
        PlayerAudio.clip = Buy;
        PlayerAudio.Play();
        Debug.Log("Item Sold");
    }

    public void ToggleGameUI(bool state)
    {
        gameUI.SetActive(state);
    }

    private void FindRacks()
    {
        storageRacks = new List<Rack>(GameObject.Find("StorageRacks").GetComponentsInChildren<Rack>());
        coreRacks = new List<Rack>(GameObject.Find("CoreRacks").GetComponentsInChildren<Rack>());
        computeRacks = new List<Rack>(GameObject.Find("ComputeRacks").GetComponentsInChildren<Rack>());

        Debug.Log(storageRacks.Count + "Storage Racks");
        Debug.Log(coreRacks.Count + "Core Racks");
        Debug.Log(computeRacks.Count + "Compute Racks");
    }

    public int GetTotalModuleTypeRackCapacity(RackModule.ModuleType type)
    {
        List<Rack> rackList;

        switch (type) { 
            case RackModule.ModuleType.HardDrive:
                rackList = storageRacks;
                break;
            case RackModule.ModuleType.Core:
                rackList = coreRacks;
                break;
            case RackModule.ModuleType.Compute:
                rackList = computeRacks;
                break;
            default:
                throw new System.ArgumentNullException("Invalid Module type");
        }

        int capacity = 0;

        foreach (Rack rack in rackList)
        {
            capacity += rack.GetTotalOutput();
        }

        return capacity;
    }


    private void CreateInitialServerLoadout()
    {
        for (int i=0; i< 12; i++)
        {
            GameObject hardDrive = Instantiate(StoreData[RackModule.ModuleType.HardDrive].Prefab);
            
            hardDrive.GetComponent<Rigidbody>().isKinematic = true;
            hardDrive.GetComponent<Rigidbody>().detectCollisions = false;
            hardDrive.tag = "Untagged";

            storageRacks[0].InsertItem(hardDrive);
        }

        GameObject compModule = Instantiate(StoreData[RackModule.ModuleType.Compute].Prefab);

        compModule.GetComponent<Rigidbody>().isKinematic = true;
        compModule.GetComponent<Rigidbody>().detectCollisions = false;
        compModule.tag = "Untagged";

        computeRacks[0].InsertItem(compModule);

        GameObject coreModule = Instantiate(StoreData[RackModule.ModuleType.Core].Prefab);

        coreModule.GetComponent<Rigidbody>().isKinematic = true;
        coreModule.GetComponent<Rigidbody>().detectCollisions = false;
        coreModule.tag = "Untagged";

        coreRacks[0].InsertItem(coreModule);
    }
}
