﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightItem : InteractableBase
{
    protected override void PerformInteraction()
    {
        InventoryManager.instance.CollectFlashlight();
        this.gameObject.SetActive(false);

        Interactor.instance.QuitInteraction();
    }
}
