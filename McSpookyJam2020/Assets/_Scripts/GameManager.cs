using System;
using MichaelWolfGames;
using UnityEngine;

public class GameManager : SceneSingleton<GameManager>
{
    public DayNightController dayNightController = null;
    public SpookerAI monsterController;

    public AK.Wwise.Switch Day;
    public AK.Wwise.Switch Night;
    public AK.Wwise.Event DeathSound;
    public AK.Wwise.Event StopAMB;
    public AK.Wwise.Event PlayAMB;
    public AK.Wwise.Event StopMusic;
    public AK.Wwise.Event PlayMusic;
    public AK.Wwise.Event GameStart;


    private void Start()
    {
        OnGameStart();
    }

    private void OnGameStart()
    {
        dayNightController.MakeDay();
        monsterController.gameObject.SetActive(false);
        GameStart.Post(gameObject);
        PlayAMB.Post(gameObject);
        PlayMusic.Post(gameObject);
        Day.SetValue(gameObject);
        // ToDo: Ambiance start
    }

    public void OnEnterInvestigationCardScreen()
    {
        // Lock player movement
        FirstPersonAIO.instance.SetControllerPause(true);
    }

    public void OnExitInvestigationCardScreen()
    {
        // Free player movement.
        FirstPersonAIO.instance.SetControllerPause(false);
    }

    public void TransitionToNight()
    {
        dayNightController.MakeNight();
        monsterController.gameObject.SetActive(true); // Goes right in front of the door
        Night.SetValue(gameObject);
        StopAMB.Post(gameObject);
        PlayAMB.Post(gameObject);
    }
        
    // ToDo: Ambiance swap


    
    
    public void OnPlayerDeath()
    {
    DeathSound.Post(gameObject);
    StopAMB.Post(gameObject);
    StopMusic.Post(gameObject);
    Debug.Log("PLAYER FUCKING DIED");
    }
}
