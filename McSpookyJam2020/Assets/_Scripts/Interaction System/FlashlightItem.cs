using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightItem : InteractableBase
{
    protected override void PerformInteraction()
    {
        base.PerformInteraction();
        
        InventoryManager.instance.GetFlashlight();
        this.gameObject.SetActive(false);
        
        // ToDo: Pickup SFX
        
        Interactor.instance.QuitInteraction();
    }
}
