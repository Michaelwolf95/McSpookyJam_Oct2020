using System;
using MichaelWolfGames;
using UnityEngine;

public class InventoryManager : SceneSingleton<InventoryManager>
{
    public bool hasFlashlight { get; private set; }
    public bool hasBasementKey { get; private set; }
    public bool hasRitualDagger { get; private set; }
    
    public void GetFlashlight()
    {
        hasFlashlight = true;
        Flashlight.instance.Toggle(true);
        
    }

    public void GetBasementKey()
    {
        hasBasementKey = true;
        //Flashlight.instance.Toggle(true);
        
    }
    
    public void GetRitualDagger()
    {
        hasRitualDagger = true;
        //Flashlight.instance.Toggle(true);
        
    }

}
