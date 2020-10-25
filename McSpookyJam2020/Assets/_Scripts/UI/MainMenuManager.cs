using System;
using MichaelWolfGames;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // ERIC - there's no point in using SerializeField unless these are private.
    [SerializeField] private RectTransform _startButton = null;
    [SerializeField] private RectTransform _creditsButton = null;

    public void LoadGameScene()
    {
        StartCoroutine(MenuTweenEffects.ScalePressEffect(_startButton, () =>
        {
            SceneManager.LoadScene(GameManager.GAME_SCENE_INDEX);
        }));
        //StartCoroutine(OnPress(GameManager.GAME_SCENE_INDEX, _startButton));
    }

    public void LoadCredits()
    {
        Debug.LogWarning("HEY ERIC, READ THIS ToDo!");
        // ToDo: Change this to just swap the menu to show credits.
        //StartCoroutine(OnPress("CREDITS", _creditsButton));
    }
    
    // ToDo: Create a way to load into credits from the main game scene.
    
}
