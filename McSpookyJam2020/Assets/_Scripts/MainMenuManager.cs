using MichaelWolfGames;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] RectTransform _startButton;
    [SerializeField] RectTransform _creditsButton;
    [SerializeField] Vector3 _startScale, _endScale;
    [SerializeField] float _animationDuration;

    public void LoadGameScene()
    {
        StartCoroutine(OnPress("MAIN", _startButton));
    }

    public void LoadCredits()
    {
        StartCoroutine(OnPress("CREDITS", _creditsButton));
    }

    public IEnumerator OnPress(string sceneName, RectTransform button)
    {
        this.DoTween(lerp =>
        {
            button.localScale = Vector3.LerpUnclamped(_startScale, _endScale, lerp);
        }, null, _animationDuration, EaseType.punch);

        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(sceneName);
    }
}
