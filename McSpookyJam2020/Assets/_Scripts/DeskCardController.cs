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
        if (InventoryManager.instance.hasBasementKey)
        {
            deskClosed.SetActive(false);
            deskOpenedKey.SetActive(false);
            deskOpenedNoKey.SetActive(true);
        }
        else
        {
            deskClosed.SetActive(true);
            deskOpenedKey.SetActive(false);
            deskOpenedNoKey.SetActive(false);
        }
    }

    public void OpenDesk()
    {
        deskClosed.SetActive(false);
        deskOpenedKey.SetActive(true);
        deskOpenedNoKey.SetActive(false);
        DrawerOpen.Post(gameObject);
        // ToDo: Open Desk Sound.
    }
    
    public void GetKey()
    {
        deskClosed.SetActive(false);
        deskOpenedKey.SetActive(false);
        deskOpenedNoKey.SetActive(true);
        PickupKey.Post(gameObject);
        InventoryManager.instance.GetBasementKey();
        // ToDo: Get Key Sound.
        
    }
    
    
}