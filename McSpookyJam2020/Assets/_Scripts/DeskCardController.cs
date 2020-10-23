using System;
using UnityEngine;

public class DeskCardController : MonoBehaviour
{
    public GameObject deskClosed = null;
    public GameObject deskOpenedKey = null;
    public GameObject deskOpenedNoKey = null;

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
        
        // ToDo: Open Desk Sound.
    }
    
    public void GetKey()
    {
        deskClosed.SetActive(false);
        deskOpenedKey.SetActive(false);
        deskOpenedNoKey.SetActive(true);
        
        InventoryManager.instance.GetBasementKey();
        // ToDo: Get Key Sound.
        
    }
    
    
}