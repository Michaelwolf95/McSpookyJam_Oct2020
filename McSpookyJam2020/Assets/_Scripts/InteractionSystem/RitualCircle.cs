using System;
using MichaelWolfGames;
using UnityEngine;

public class RitualCircle : InvestigationCardInteractable
{
    [Space(5)]
    [SerializeField] [TextArea] 
    private string lockedSayContent = "";
    [SerializeField]
    private float lockedSayDelay = 0f;

    public AK.Wwise.Event lockedInteractStartAudio;
    public AK.Wwise.Event lockedInteractEndAudio;
    [SerializeField] AK.Wwise.Event stopMonsterDying;

    public bool isOpen { get; private set; }

    public bool playerHasKey => InventoryManager.instance.hasRitualDagger;

    protected override void Start()
    {
        base.Start();
        SetOpenState(false);
    }

    private void OnRitualPerformed()
    {
        stopMonsterDying.Post(gameObject);
        SetOpenState(true);
        
        GameManager.instance.OnRitualFinished();
    }

    private void SetOpenState(bool argIsOpen)
    {
        isOpen = argIsOpen;
        SetInteractable(isOpen == false);
    }

    public override void OnBecomePointerTarget()
    {
        interactReticleType = (playerHasKey)
            ? InteractReticleType.Hand
            : InteractReticleType.Look;
        reticle.SetInteractType(interactReticleType);
        base.OnBecomePointerTarget();
    }


    protected override void PerformInteraction()
    {
        if (playerHasKey == false)
        {
            // Copied from say interactable.
            Action performSay = () =>
            {
                ScreenTextManager.instance.SayText(lockedSayContent, (() =>
                {
                    // On Complete?
                    Interactor.instance.QuitInteraction();
                }));
            };

            if (lockedSayDelay <= 0f)
            {
                performSay();
            }
            else
            {
                this.StartTimer(lockedSayDelay, performSay);
            }
        }
        else
        {
            GameManager.instance.OnRitualStarted();
            // Show Card.
            base.PerformInteraction();
        }
    }

    public override void OnFinishInteraction()
    {
        if (playerHasKey == false)
        {
            isInteracting = false;
            PlayFinishInteractionSound();
            OnInteractEnd.Invoke();
        }
        else
        {
            // NOTE: THIS NEVER FINISHES THE INTERACTION!
            //base.OnFinishInteraction();
            
            OnRitualPerformed();
        }
    }

    protected override void PlayBeginInteractionSound()
    {
        if (playerHasKey == false)
        {
            lockedInteractStartAudio.Post(gameObject);
        }
        else
        {
            base.PlayBeginInteractionSound();
        }
    }

    protected override void PlayFinishInteractionSound()
    {
        if (playerHasKey == false)
        {
            lockedInteractEndAudio.Post(gameObject);
        }
        else
        {
            base.PlayFinishInteractionSound();
        }
    }
}