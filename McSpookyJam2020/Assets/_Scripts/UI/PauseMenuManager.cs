using MichaelWolfGames;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] KeyCode _pauseButton;
    
    private void Update()
    {
        if(Input.GetKey(_pauseButton) && GameManager.instance.IsPlayerInMenu == false)
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
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
        mainMenuButton.interactable = argInteractable;
    }

    public void ResumeGame()
    {
        ToggleButtonsInteractive(false);
        StartCoroutine(MenuTweenEffects.ScalePressEffect((RectTransform)resumeButton.transform, () =>
        {
            ToggleButtonsInteractive(true);
            pauseMenuPanel.SetActive(false);
            ReturnCursorState();
            GameManager.instance.IsPlayerInMenu = false;
            Time.timeScale = 1;
        }, 1.25f, 1f, 0f, true));
    }

    public void ReturnToMainMenu()
    {
        ToggleButtonsInteractive(false);
        StartCoroutine(MenuTweenEffects.ScalePressEffect((RectTransform)mainMenuButton.transform, () =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(GameManager.MAIN_MENU_SCENE_INDEX);
        }, 1.25f, 1f, 0.25f, true));
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
