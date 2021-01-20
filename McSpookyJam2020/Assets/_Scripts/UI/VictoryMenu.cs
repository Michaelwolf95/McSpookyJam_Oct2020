using System;
using MichaelWolfGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryMenu : MonoBehaviour
{
    private static string FEEDBACK_FORM_URL = "https://forms.gle/6vCiTUnRPwgJroSf6";

    [SerializeField] private AK.Wwise.Event exitButtonSound = null;

    public CanvasGroup mainCanvasGroup = null;
    public CanvasGroup buttonCanvasGroup = null;
    public CanvasGroup creditsPanel = null;
    public Button quitButton = null;
    public Button creditsButton = null;
    public Button feedbackButton = null;
    
    private void Awake()
    {
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitToTitlePressed);
        }
        if (creditsButton != null)
        {
            creditsButton.onClick.AddListener(OnCreditsButtonPressed);
        }
        if (feedbackButton != null)
        {
            feedbackButton.onClick.AddListener(OnFeedbackButtonPressed);
        }
        
        mainCanvasGroup.gameObject.SetActive(false);
    }

    public void FadeInMenu()
    {
        mainCanvasGroup.gameObject.SetActive(true);
        buttonCanvasGroup.alpha = 0f;
        mainCanvasGroup.alpha = 0f;
        ToggleMenuInteractable(false);
        this.DoTween(lerp => { mainCanvasGroup.alpha = lerp; }, () =>
        {
            this.DoTween(lerp => { buttonCanvasGroup.alpha = lerp; }, (() =>
                {
                    ToggleMenuInteractable(true);
                }),
                0.5f);
        }, 0.5f, 1f);
    }

    private void FadeOutMenu(Action onFadeComplete = null)
    {
        mainCanvasGroup.gameObject.SetActive(true);
        ToggleMenuInteractable(false);
        this.DoTween(lerp =>
        {
            mainCanvasGroup.alpha = Mathf.Lerp(1f, 0f,lerp);
            buttonCanvasGroup.alpha = Mathf.Lerp(1f, 0f,lerp);
        }, ()=>
        {
            ToggleMenuInteractable(true);
            if (onFadeComplete != null)
            {
                onFadeComplete();
            }
        }, 1f);
    }

    private void ToggleMenuInteractable(bool argInteractable)
    {
        if (quitButton != null)
        {
            quitButton.interactable = argInteractable;
        }
        if (creditsButton != null)
        {
            creditsButton.interactable = argInteractable;
        }
        if (feedbackButton != null)
        {
            feedbackButton.interactable = argInteractable;
        }
    }
    
    
    private void OnQuitToTitlePressed()
    {
        exitButtonSound.Post(gameObject);
        ToggleMenuInteractable(false);
        StartCoroutine(MenuTweenEffects.ScalePressEffect((RectTransform)quitButton.transform, () =>
        {
            this.DoTween(lerp =>
            {
                buttonCanvasGroup.alpha = Mathf.Lerp(1f, 0f,lerp);
            }, ()=>
            {
                ToggleMenuInteractable(true);
                SceneManager.LoadScene(GameManager.MAIN_MENU_SCENE_INDEX);
            }, 0.5f);
        }));
    }
    
    private void OnCreditsButtonPressed()
    {
        ToggleMenuInteractable(false);
        StartCoroutine(MenuTweenEffects.ScalePressEffect((RectTransform)quitButton.transform, () =>
        {
            ToggleMenuInteractable(true);
            // ToDo: Open Credits Panel

            if (creditsPanel != null)
            {
                creditsPanel.gameObject.SetActive(true);
            }
        }));
    }
    
    private void OnFeedbackButtonPressed()
    {
        Application.OpenURL(FEEDBACK_FORM_URL);
    }
}