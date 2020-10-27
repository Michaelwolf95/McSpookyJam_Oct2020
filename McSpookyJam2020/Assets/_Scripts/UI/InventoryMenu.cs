using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup rootCanvasGroup = null;
    [SerializeField] private InventoryButton[] hagueButtons = null;

    private InventoryButton currentSelection = null;

    private void Start()
    {
        for (int i = 0; i < hagueButtons.Length; i++)
        {
            int index = i;
            hagueButtons[i].button.onClick.AddListener(() =>
            {
                OpenButton(hagueButtons[index]);
            });
        }

        UpdateButtons();
    }
    
    public void OpenInventory()
    {
        UpdateButtons();
        rootCanvasGroup.gameObject.SetActive(true);
    }
    
    public void CloseInventory()
    {
        if (currentSelection)
        {
            currentSelection.ClosePanel();
        }
        rootCanvasGroup.gameObject.SetActive(false);
    }

    public void OpenButton(InventoryButton inventoryButton)
    {
        if (currentSelection != null)
        {
            currentSelection.ClosePanel();
        }
        currentSelection = inventoryButton;
        currentSelection.ShowPanel();
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < hagueButtons.Length; i++)
        {
            bool collected = i < InventoryManager.instance.GetHagueLetterCount();
            hagueButtons[i].SetLockedState(!collected);
        }
    }
}
