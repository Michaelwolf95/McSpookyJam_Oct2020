using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public Button button= null;
    public GameObject lockedGameObject= null;
    public CanvasGroup panel = null;

    private void Awake()
    {
        SetLockedState(true);
        ClosePanel();
    }

    public void SetLockedState(bool argLocked)
    {
        button.interactable = !argLocked;
        button.gameObject.SetActive(!argLocked);
        lockedGameObject.SetActive(argLocked);
    }

    public void ShowPanel()
    {
        panel.gameObject.SetActive(true);
    }
    
    public void ClosePanel()
    {
        panel.gameObject.SetActive(false);
    }
    
}