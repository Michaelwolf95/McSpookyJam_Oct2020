using System;
using MichaelWolfGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryMenu : MonoBehaviour
{
    [Space(5)]
    public CanvasGroup mainCanvasGroup = null;
    public CanvasGroup buttonCanvasGroup = null;
    public Button quitButton = null;
    
    private void Awake()
    {
        quitButton.onClick.AddListener(OnQuitToTitlePressed);
        
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
        quitButton.interactable = argInteractable;
    }

    private void OnQuitToTitlePressed()
    {
        StartCoroutine(MenuTweenEffects.ScalePressEffect((RectTransform)quitButton.transform, () =>
        {
            this.DoTween(lerp =>
            {
                buttonCanvasGroup.alpha = Mathf.Lerp(1f, 0f,lerp);
            }, ()=>
            {
                SceneManager.LoadScene(GameManager.MAIN_MENU_SCENE_INDEX);
            }, 1f);
        }));
    }
}