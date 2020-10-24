using System;
using Fungus;
using MichaelWolfGames;
using TMPro;
using UnityEngine;

public class ScreenTextManager : SceneSingleton<ScreenTextManager>
{
    [SerializeField] private SayDialog sayDialog = null;
    
    public void SayText(string argText, Action argOnComplete = null)
    {
        sayDialog.SetActive(true);
        sayDialog.Say(argText, true, false, true, true, false, null, argOnComplete);
    }
    
    
}