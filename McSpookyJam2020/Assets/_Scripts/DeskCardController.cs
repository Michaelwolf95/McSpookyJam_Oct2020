using System;
using UnityEngine;

public class DeskCardController : MonoBehaviour
{
    public GameObject deskClosed = null;
    public GameObject deskOpenedKey = null;
    public GameObject deskOpenedNoKey = null;

    public AK.Wwise.Event PickupKey;
    public AK.Wwise.Event DrawerOpen;

    private void OnEnable()
    {
        deskClosed.SetActive(true);
        deskOpenedKey.SetActive(false);
        deskOpenedNoKey.SetActive(false);
    }

    public void OpenDesk()
    {
        deskClosed.SetActive(false);
        deskOpenedKey.SetActive(InventoryManager.instance.hasBasementKey == false);
        deskOpenedNoKey.SetActive(InventoryManager.instance.hasBasementKey);
        DrawerOpen.Post(gameObject);
    }
    
    public void GetKey()
    {
        deskClosed.SetActive(false);
        deskOpenedKey.SetActive(false);
        deskOpenedNoKey.SetActive(true);
        PickupKey.Post(gameObject);
        InventoryManager.instance.GetBasementKey();
        
    }
    
    
}