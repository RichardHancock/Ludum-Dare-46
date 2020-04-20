using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : Interactable
{
    private GameData gameData;

    private GameObject UICanvas;

    private RackModule.ModuleType currentItem = RackModule.ModuleType.HardDrive;

    public Button HardDriveButton, CoreModuleButton, ComputeModuleButton, BuyButton, ExitButton;
    public Text ItemName, ItemPrice;
    public RawImage ItemImage;

    

    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();

        InteractableFlag = true;
        InsertableFlag = true;

        UICanvas = GameObject.Find("StoreUI");
        if (!UICanvas)
            throw new System.NullReferenceException("Canvas Not Passed To Store");
        UICanvas.SetActive(false);

        //In Future use m_YourSecondButton.onClick.AddListener(delegate {TaskWithParameters("Hello"); });

        HardDriveButton.onClick.AddListener(HardDriveButtonPressed);
        CoreModuleButton.onClick.AddListener(CoreModuleButtonPressed);
        ComputeModuleButton.onClick.AddListener(ComputeModuleButtonPressed);
        BuyButton.onClick.AddListener(BuyButtonPressed);
        ExitButton.onClick.AddListener(ExitButtonPressed);

    }

    // Update is called once per frame
    void Update()
    {
        if (UICanvas.gameObject.activeSelf && Input.GetButtonDown("Cancel"))
        {
            ExitButtonPressed();
        }
    }

    public override bool Interact()
    {
        gameData.ToggleGameUI(false);
        UICanvas.gameObject.SetActive(true);
        gameData.DisableInput = true;
        SwitchCurrentItem(RackModule.ModuleType.HardDrive);
        HardDriveButton.Select();

        return true;
    }

    public override bool InsertItem(GameObject item)
    {
        gameData.SellModule(item.GetComponent<RackModule>().Type);

        Destroy(item);

        return true;
    }

    private void SwitchCurrentItem(RackModule.ModuleType itemType)
    {
        currentItem = itemType;

        GameData.StoreItem item = gameData.StoreData[itemType];
        ItemName.text = item.Name;
        ItemPrice.text = "£" + item.Price;
        ItemImage.texture = item.Image;
    }

    public void HardDriveButtonPressed()
    {
        SwitchCurrentItem(RackModule.ModuleType.HardDrive);
    }
    public void CoreModuleButtonPressed()
    {
        SwitchCurrentItem(RackModule.ModuleType.Core);
    }
    public void ComputeModuleButtonPressed()
    {
        SwitchCurrentItem(RackModule.ModuleType.Compute);
    }


    public void BuyButtonPressed()
    {
        gameData.BuyModule(currentItem);

        UICanvas.gameObject.SetActive(false);
        gameData.ToggleGameUI(true);
        gameData.DisableInput = false;
    }

    public void ExitButtonPressed()
    {
        UICanvas.gameObject.SetActive(false);
        gameData.ToggleGameUI(true);
        gameData.DisableInput = false;
    }
}
