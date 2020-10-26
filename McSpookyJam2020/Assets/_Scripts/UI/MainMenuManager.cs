﻿using System;
using MichaelWolfGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton = null;
    [SerializeField] private Button quitButton = null;
    [SerializeField] private Button creditsButton = null;
    [SerializeField] private CanvasGroup rootCanvasGroup = null;
    [SerializeField] private CanvasGroup buttonsCanvasGroup = null;
    [SerializeField] private CanvasGroup creditsPanel = null;

    [Header("Intro Animation")] 
    [SerializeField] private float startDelay = 0.5f;
    [SerializeField] private float sigilFadeDuration = 1f;
    [SerializeField] private float buttonsFadeDelay = 0.5f;
    [SerializeField] private float buttonFadeDuration = 0.5f;
    
    
    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonPressed);
        quitButton.onClick.AddListener(OnQuitButtonPressed);
        creditsButton.onClick.AddListener(OnCreditsButtonPressed);

        rootCanvasGroup.alpha = 0f;
        buttonsCanvasGroup.alpha = 0f;
        creditsPanel.gameObject.SetActive(false);

        ToggleButtonInteraction(false);
        this.DoTween(lerp => { rootCanvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp); }, (() =>
        {
            this.DoTween(lerp => { buttonsCanvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp); }, (() =>
            {
                ToggleButtonInteraction(true);
            }), buttonFadeDuration, buttonsFadeDelay);
        }), sigilFadeDuration, startDelay);
    }

    private void ToggleButtonInteraction(bool isInteractable)
    {
        startButton.interactable = isInteractable;
        quitButton.interactable = isInteractable;
        creditsButton.interactable = isInteractable;
    }
    
    private void OnStartButtonPressed()
    {
        ToggleButtonInteraction(false);
        StartCoroutine(MenuTweenEffects.ScalePressEffect((RectTransform) startButton.transform, () => { }));
        
        this.DoTween(lerp =>
        {
            rootCanvasGroup.alpha = Mathf.Lerp(1f, 0f, lerp);
            buttonsCanvasGroup.alpha = Mathf.Lerp(1f, 0f, lerp);
        }, (() =>
        {
            SceneManager.LoadScene(GameManager.GAME_SCENE_INDEX);
        }), 1f, 0.5f);
    }

    private void OnQuitButtonPressed()
    {
        StartCoroutine(MenuTweenEffects.ScalePressEffect((RectTransform)startButton.transform, () =>
        {
            
            Application.Quit();
            
        }, 1.25f, 1f, 0f));
    }
    
    private void OnCreditsButtonPressed()
    {
        OpenCredits();
    }

    public void OpenCredits()
    {
        creditsPanel.gameObject.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.gameObject.SetActive(false);
    }
    
   

    
    
}
