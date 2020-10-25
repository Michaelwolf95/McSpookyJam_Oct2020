using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform _startButton = null;
    [SerializeField] private RectTransform _creditsButton = null;
    [SerializeField] private GameObject _creditsPanel = null;

    public void LoadGameScene()
    {
        StartCoroutine(MenuTweenEffects.ScalePressEffect(_startButton, () =>
        {
            SceneManager.LoadScene(GameManager.GAME_SCENE_INDEX);
        }));
    }

    public void OpenCredits()
    {
        _creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        _creditsPanel.SetActive(false);
    }
    
    // ToDo: Create a way to load into credits from the main game scene.
    
}
