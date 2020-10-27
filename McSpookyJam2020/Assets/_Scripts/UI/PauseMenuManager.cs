using System;
using MichaelWolfGames;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event resumeButtonSound;
    [SerializeField] private AK.Wwise.Event pauseMenuSound;
    [SerializeField] private AK.Wwise.Event mainMenuSound;
    [SerializeField] private AK.Wwise.State GameStatePaused;
    [SerializeField] private AK.Wwise.State GameStateInGame;

    [SerializeField] private Button resumeButton = null;
    [SerializeField] private Button inventoryButton = null;
    [SerializeField] private Button mainMenuButton = null;
    [SerializeField] private CanvasGroup pauseMenuPanel = null;
    [SerializeField] private CanvasGroup fadeOutPanel = null;
    [SerializeField] private InventoryMenu inventoryMenu = null;
    [SerializeField] KeyCode pauseKey = KeyCode.Escape;

    private void Start()
    {
        fadeOutPanel.alpha = 0f;
        fadeOutPanel.gameObject.SetActive(false);
        
        resumeButton.onClick.AddListener(ResumeGame);
        inventoryButton.onClick.AddListener(OpenInventory);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void Update()
    {
        if(Input.GetKey(pauseKey) && GameManager.instance.IsPlayerInMenu == false)
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        GameStatePaused.SetValue();
        pauseMenuSound.Post(gameObject);
        pauseMenuPanel.gameObject.SetActive(true);
        pauseMenuPanel.alpha = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;

        GameManager.instance.IsPlayerInMenu = true;
    }

    private void ReturnCursorState()
    {
        bool visible = GameManager.instance.IsPlayerOnCardScreen;
        Cursor.visible = visible;
        Cursor.lockState = (visible) ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void ToggleButtonsInteractive(bool argInteractable)
    {
        resumeButton.interactable = argInteractable;
        inventoryButton.interactable = argInteractable;
        mainMenuButton.interactable = argInteractable;
    }

    public void ResumeGame()
    {
        GameStateInGame.SetValue();
        resumeButtonSound.Post(gameObject);
        ToggleButtonsInteractive(false);
        StartCoroutine(MenuTweenEffects.ScalePressEffect((RectTransform)resumeButton.transform, () =>
        {
            ToggleButtonsInteractive(true);
            pauseMenuPanel.gameObject.SetActive(false);
            ReturnCursorState();
            GameManager.instance.IsPlayerInMenu = false;
            Time.timeScale = 1;
        }, 1.25f, 1f, 0f, true));
    }

    public void OpenInventory()
    {
        inventoryMenu.OpenInventory();
    }

    public void ReturnToMainMenu()
    {
        mainMenuSound.Post(gameObject);
        ToggleButtonsInteractive(false);
        StartCoroutine(MenuTweenEffects.ScalePressEffect((RectTransform)mainMenuButton.transform, null, 1.25f, 1f, 0f, true));
        
        fadeOutPanel.alpha = 0f;
        fadeOutPanel.gameObject.SetActive(true);
        
        this.DoTween(lerp =>
        {
            fadeOutPanel.alpha = Mathf.Lerp(0f, 1f, lerp);
            pauseMenuPanel.alpha = Mathf.Lerp(1f, 0f, lerp);
        }, (() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(GameManager.MAIN_MENU_SCENE_INDEX);
            
        }), 1f, 0.5f, EaseType.linear, true);
        
        //StartCoroutine(PauseButtonPress(mainMenuButton, 1));
    }

//    IEnumerator PauseButtonPress(RectTransform button, int buttonNumberPressed)
//    {
//        /* Get tween to work correctly with unscaled time
//        this.DoTween(lerp =>
//        {
//            button.localScale = Vector3.LerpUnclamped(_startScale, _endScale, lerp);
//        }, null, _animationDuration, EaseType.spring, true);
//        */
//
//        yield return new WaitForSecondsRealtime(1f);
//
//        // If resume is pressed
//        if (buttonNumberPressed == 0)
//        {
//            pauseMenuPanel.SetActive(false);
//            Cursor.visible = false;
//            Cursor.lockState = CursorLockMode.Locked;
//            Time.timeScale = 1;
//        }
//        else // Go to main menu
//        {
//            Time.timeScale = 1;
//            pauseMenuPanel.SetActive(false);
//            SceneManager.LoadScene("MAINMENU");
//        }
//    }
}
