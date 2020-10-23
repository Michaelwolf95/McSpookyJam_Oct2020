using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableBase : MonoBehaviour
{
    [SerializeField]
    private GameObject activatedReticle = null;
    [SerializeField]
    private GameObject pointerTargetReticle = null;

    public bool isActivated { get; protected set; }
    public bool isInteracting { get; protected set; }
    
    public UnityEvent OnInteractStart = new UnityEvent();
    public UnityEvent OnInteractEnd = new UnityEvent();

    public AK.Wwise.Event InteractStartAudio;
    public AK.Wwise.Event InteractEndAudio;

    private void Awake()
    {
        activatedReticle.SetActive(false);
        pointerTargetReticle.SetActive(false);
    }

    protected virtual void Update()
    {
        if (Interactor.instance != null)
        {
            Interactor.instance.ProcessInteractableActivationRange(this);
        }
    }

    public virtual bool IsInteractable()
    {
        return true;
    }
    
    public virtual void OnEnterActivationRange()
    {
        //Debug.Log("OnEnterActivationRange");
        isActivated = true;
        activatedReticle.SetActive(true);
    }
    
    public virtual void OnExitActivationRange()
    {
        //Debug.Log("OnExitActivationRange");
        isActivated = false;
        activatedReticle.SetActive(false);
    }
    
    public virtual void OnBecomePointerTarget()
    {
        //Debug.Log("OnBecomePointerTarget");
        pointerTargetReticle.SetActive(true);
        activatedReticle.SetActive(false);
    }
    
    public virtual void OnNoLongerPointerTarget()
    {
        //Debug.Log("OnNoLongerPointerTarget");
        pointerTargetReticle.SetActive(false);
        activatedReticle.SetActive(false);
    }
    
    public virtual void OnBeginInteraction()
    {
        //Debug.Log("OnBeginInteraction");
        isInteracting = true;
        OnInteractStart.Invoke();
        InteractStartAudio.Post(gameObject);
        PerformInteraction();
}
    protected virtual void PerformInteraction()
    {
        //Debug.Log("PerformInteraction");

        Interactor.instance.QuitInteraction();
    }
    
    public virtual void OnFinishInteraction()
    {
        isInteracting = false;
        InteractEndAudio.Post(gameObject);
        OnInteractEnd.Invoke();
        //Debug.Log("OnFinishInteraction");
    }
}
