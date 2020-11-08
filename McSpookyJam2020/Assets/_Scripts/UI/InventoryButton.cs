using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public Button button= null;
    public GameObject lockedGameObject= null;
    public CanvasGroup panel = null;

    public void Init(Action argOnClickAction)
    {
        button.onClick.AddListener(()=>
        {
            argOnClickAction();
        });
        SetLockedState(true);
        ClosePanel();
    }

    public void SetLockedState(bool argLocked)
    {
        //Debug.Log("SetLockedState: " + argLocked);
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