using System;
using MichaelWolfGames;
using UnityEngine;

public class GameManager : SceneSingleton<GameManager>
{
    public DayNightController dayNightController = null;
    public SpookerAI monsterController;

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
        Debug.Log("PLAYER FUCKING DIED");
    }
}
