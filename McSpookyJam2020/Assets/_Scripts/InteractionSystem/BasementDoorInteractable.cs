using System;
using MichaelWolfGames;
using UnityEngine;

public class BasementDoorInteractable : InvestigationCardInteractable
{
    [SerializeField] private GameObject closedDoor = null;
    [SerializeField] private GameObject openDoor = null;
    
    [Space(5)]
    [SerializeField] [TextArea] 
    private string lockedSayContent = "";
    [SerializeField]
    private float lockedSayDelay = 0f;

    public AK.Wwise.Event lockedInteractStartAudio;
    public AK.Wwise.Event lockedInteractEndAudio;

    public bool isOpen { get; private set; }

    protected override void Start()
    {
        base.Start();
        SetOpenState(false);
    }

    public void OpenDoor()
    {
        SetOpenState(true);
        GameManager.instance.OnBasementDoorOpened();
    }

    private void SetOpenState(bool argIsOpen)
    {
        isOpen = argIsOpen;
        closedDoor.SetActive(isOpen == false);
        openDoor.SetActive(isOpen);
        SetInteractable(isOpen == false);
    }

    public override void OnBecomePointerTarget()
    {
        interactReticleType = (InventoryManager.instance.hasBasementKey)
            ? InteractReticleType.Hand
            : InteractReticleType.Look;
        reticle.SetInteractType(interactReticleType);
        base.OnBecomePointerTarget();
    }


    protected override void PerformInteraction()
    {
        if (InventoryManager.instance.hasBasementKey == false)
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
            // Show Card.
            base.PerformInteraction();
        }
    }

    public override void OnFinishInteraction()
    {
        if (InventoryManager.instance.hasBasementKey == false)
        {
            isInteracting = false;
            PlayFinishInteractionSound();
            OnInteractEnd.Invoke();
        }
        else
        {
            base.OnFinishInteraction();
            OpenDoor();
        }
    }

    protected override void PlayBeginInteractionSound()
    {
        if (InventoryManager.instance.hasBasementKey == false)
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
        if (InventoryManager.instance.hasBasementKey == false)
        {
            lockedInteractEndAudio.Post(gameObject);
        }
        else
        {
            base.PlayFinishInteractionSound();
        }
    }

}