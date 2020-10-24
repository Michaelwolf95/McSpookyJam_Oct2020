using MichaelWolfGames;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] RectTransform _resumeButton;
    [SerializeField] RectTransform _mainMenuButton;
    [SerializeField] GameObject _pauseMenuPanel;
    [SerializeField] Vector3 _startScale, _endScale;
    [SerializeField] float _animationDuration;
    [SerializeField] KeyCode _pauseButton;

    private void Update()
    {
        if(Input.GetKey(_pauseButton))
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        _pauseMenuPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        StartCoroutine(PauseButtonPress(_resumeButton, 0));
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(PauseButtonPress(_mainMenuButton, 1));
    }

    IEnumerator PauseButtonPress(RectTransform button, int buttonNumberPressed)
    {
        this.DoTween(lerp =>
        {
            button.localScale = Vector3.LerpUnclamped(_startScale, _endScale, lerp);
        }, null, _animationDuration, EaseType.easeInOutCirc);

        yield return new WaitForSecondsRealtime(1f);

        // If resume is pressed
        if (buttonNumberPressed == 0)
        {
            _pauseMenuPanel.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        else // Go to main menu
        {
            _pauseMenuPanel.SetActive(false);
            SceneManager.LoadScene("MAINMENU");
        }

    }
}
