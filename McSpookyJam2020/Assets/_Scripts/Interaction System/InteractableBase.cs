using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    [SerializeField]
    private GameObject activatedReticle = null;
    [SerializeField]
    private GameObject pointerTargetReticle = null;

    public bool isActivated { get; protected set; }
    public bool isInteracting { get; protected set; }

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
        
        PerformInteraction();
    }
    
    protected virtual void PerformInteraction()
    {
        //Debug.Log("PerformInteraction");
        // ToDo: Do this after a delay?
        Interactor.instance.QuitInteraction();
    }
    
    public virtual void OnFinishInteraction()
    {
        isInteracting = false;
        //Debug.Log("OnFinishInteraction");
    }
}
