using System;
using MichaelWolfGames;
using UnityEngine;

public class InventoryManager : SceneSingleton<InventoryManager>
{
    public bool hasFlashlight { get; private set; }
    
    public void GetFlashlight()
    {
        hasFlashlight = true;
        Flashlight.instance.Toggle(true);
        
    }



}
