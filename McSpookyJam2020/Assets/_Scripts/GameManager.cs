using System;
using MichaelWolfGames;
using UnityEngine;

public class GameManager : SceneSingleton<GameManager>
{
    public DayNightController dayNightController = null;
    public SpookerAI monsterController;

    public AK.Wwise.Event DeathSound;
    public AK.Wwise.Event StopAMB;
    public AK.Wwise.Event StopMusic;
    public AK.Wwise.Switch Time;

    private void Start()
    {
        OnGameStart();
    }

    private void OnGameStart()
    {
        dayNightController.MakeDay();
        monsterController.gameObject.SetActive(false);
    }
    
    public void TransitionToNight()
    {
        dayNightController.MakeDay();
        monsterController.gameObject.SetActive(true); // Goes right in front of the door
    }
    
    
    public void OnPlayerDeath()
    {
        DeathSound.Post(gameObject);
        Debug.Log("PLAYER FUCKING DIED");
    }
}
