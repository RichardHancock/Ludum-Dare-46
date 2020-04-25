using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : Interactable
{
    private GameObject UICanvas;

    private RackModule.ModuleType currentItem = RackModule.ModuleType.HardDrive;

    public Button HardDriveButton, CoreModuleButton, ComputeModuleButton, BuyButton, ExitButton;
    public Text ItemName, ItemPrice;
    public RawImage ItemImage;

    

    // Start is called before the first frame update
    void Start()
    {
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
        GameManager.Instance.ToggleGameUI(false);
        UICanvas.gameObject.SetActive(true);
        GameManager.Instance.DisableInput = true;
        SwitchCurrentItem(RackModule.ModuleType.HardDrive);
        HardDriveButton.Select();

        return true;
    }

    public override bool InsertItem(GameObject item)
    {
        GameManager.Instance.SellModule(item.GetComponent<RackModule>().Type);

        Destroy(item);

        return true;
    }

    private void SwitchCurrentItem(RackModule.ModuleType itemType)
    {
        currentItem = itemType;

        GameManager.StoreItem item = GameManager.Instance.StoreData[itemType];
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
        GameManager.Instance.BuyModule(currentItem);

        UICanvas.gameObject.SetActive(false);
        GameManager.Instance.ToggleGameUI(true);
        GameManager.Instance.DisableInput = false;
    }

    public void ExitButtonPressed()
    {
        UICanvas.gameObject.SetActive(false);
        GameManager.Instance.ToggleGameUI(true);
        GameManager.Instance.DisableInput = false;
    }
}
