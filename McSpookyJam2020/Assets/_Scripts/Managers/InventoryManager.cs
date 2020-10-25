using System;
using System.Collections.Generic;
using System.Linq;
using Fungus;
using MichaelWolfGames;
using TMPro;
using UnityEngine;

public class InventoryManager : SceneSingleton<InventoryManager>
{
    [SerializeField] private GameObject flashlightIcon = null;
    [SerializeField] private GameObject basementKeyIcon = null;
    [SerializeField] private GameObject ritualDaggerIcon = null;
    
    [SerializeField] private GameObject lettersIcon = null;
    [SerializeField] private TextMeshProUGUI letterCountText = null;
    [SerializeField] private TextMeshProUGUI letterTotalText = null;
    
    public bool hasFlashlight { get; private set; }
    public bool hasBasementKey { get; private set; }
    public bool hasRitualDagger { get; private set; }
    public bool hasLetters => GetHagueLetterCount() > 0;
    
    private HagueLetterInvestigatable[] hagueLetters = null;
    private bool[] hagueLettersCollectedFlags = null;
    
    private void Start()
    {
        InitHagueLetterCollection();
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        flashlightIcon.SetActive(hasFlashlight);
        basementKeyIcon.SetActive(hasBasementKey);
        ritualDaggerIcon.SetActive(hasRitualDagger);
        lettersIcon.SetActive(hasLetters);
    }
    
    public void CollectFlashlight()
    {
        hasFlashlight = true;
        Flashlight.instance.Toggle(true);
        
        UpdateIcons();
        PopInTween(flashlightIcon.transform);
    }

    public void CollectBasementKey()
    {
        hasBasementKey = true;
        UpdateIcons();
        PopInTween(basementKeyIcon.transform);
    }
    
    public void CollectRitualDagger()
    {
        hasRitualDagger = true;
        UpdateIcons();
        PopInTween(ritualDaggerIcon.transform);
    }
    
    private void InitHagueLetterCollection()
    {
        hagueLetters = FindObjectsOfType<HagueLetterInvestigatable>();
        hagueLettersCollectedFlags = new bool[hagueLetters.Length];

        int totalCount = hagueLetters.Length;
        // ToDo: Let each object try to set it's own index?
        for (int i = 0; i < totalCount; i++)
        {
            hagueLetters[i].collectionIndex = i;
            hagueLettersCollectedFlags[i] = false;
        }

        if (totalCount > 0)
        {
            letterTotalText.text = string.Format("/{0}", totalCount);
        }
        else
        {
            letterTotalText.gameObject.SetActive(false);
            letterCountText.gameObject.SetActive(false);
        }

    }
    
    public void CollectHagueLetter(int letterIndex)
    {
        bool hadLetters = hasLetters;
        int beforeCount = GetHagueLetterCount();
        hagueLettersCollectedFlags[letterIndex] = true;
        
        int count = GetHagueLetterCount();
        if (count > 0)
        {
            UpdateIcons();
            letterCountText.text = count.ToString();
            if (hadLetters == false)
            {
                PopInTween(lettersIcon.transform);
            }
            else
            {
                PopInTween(letterCountText.transform);
            }
        }
    }

    private int GetHagueLetterCount()
    {
        if (hagueLettersCollectedFlags == null)
            return 0;
        return hagueLettersCollectedFlags.Count(flag => flag == true);
    }

    private void PopInTween(Transform target)
    {
        float popDuration = 1f;
        float popFactor = 2f;
        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.one * 2f;
        this.DoTween(lerp =>
        {
            target.localScale = Vector3.LerpUnclamped(startScale, endScale, lerp);
        }, null, popDuration, 0f, EaseType.punch);
    }
    
}
