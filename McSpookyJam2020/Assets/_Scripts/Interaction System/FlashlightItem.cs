using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightItem : InteractableBase
{
    protected override void PerformInteraction()
    {
        base.PerformInteraction();
        
        InventoryManager.instance.CollectFlashlight();
        this.gameObject.SetActive(false);
        
        // ToDo: Pickup SFX
        
        Interactor.instance.QuitInteraction();
    }
}
