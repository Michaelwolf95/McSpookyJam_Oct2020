using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public static Interactor instance = null;
    
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private float activationRange = 2f;
    [SerializeField] private float interactRange = 1f;

    [SerializeField] private LayerMask interactionLayerMask;
    
    private InteractableBase currentPointerTarget = null;        // Currently being looked at.
    private InteractableBase currentInteractionTarget = null;    // Actively interacting with
    private bool isInteracting => currentInteractionTarget != null;
    
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (isInteracting == false)
        {
            if (currentPointerTarget != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    BeginInteraction(currentPointerTarget);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitInteraction();
            }
        }
        
    }

    public void FixedUpdate()
    {
        if (isInteracting)
        {
            return;
        }

        //RaycastHit[] hits = Physics.SphereCastAll(mainCamera.transform.position, activationRange, Vector3.up, 0f, interactionLayerMask, )
        
        //Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, interactRange, interactionLayerMask);
        if (hits.Length > 0)
        {
            InteractableBase raycastResultTarget = null;
            float nearestDist = float.MaxValue;
            for (int i = 0; i < hits.Length; i++)
            {
                InteractableBase raycastInteractable = hits[i].transform.GetComponent<InteractableBase>();
                if (raycastInteractable != null && raycastInteractable.IsInteractable())
                {
                    if (hits[i].distance < nearestDist)
                    {
                        raycastResultTarget = raycastInteractable;
                        nearestDist = hits[i].distance;
                    }
                }
            }

            if (raycastResultTarget != null)
            {
                if (currentPointerTarget != null && currentPointerTarget != raycastResultTarget)
                {
                    ClearPointerTarget();
                }

                SetPointerTarget(raycastResultTarget);
            }
        }
        else
        {
            if (currentPointerTarget != null)
            {
                ClearPointerTarget();
            }
        }
    }

    public void BeginInteraction(InteractableBase interactable)
    {
        if (isInteracting)
        {
            QuitInteraction();
        }
        
        if (currentPointerTarget == interactable)
        {
            ClearPointerTarget();
        }
        
        currentInteractionTarget = interactable;
        currentInteractionTarget.OnBeginInteraction();
    }
    
    public void QuitInteraction()
    {
        if (isInteracting)
        {
            currentInteractionTarget.OnFinishInteraction();
            currentInteractionTarget = null;
        }
    }
    
    
    private void SetPointerTarget(InteractableBase interactable)
    {
        currentPointerTarget = interactable;
        currentPointerTarget.OnBecomePointerTarget();
    }
    
    private void ClearPointerTarget()
    {
        if (currentPointerTarget != null)
        {
            currentPointerTarget.OnNoLongerPointerTarget();
            if (IsInteractableWithinActivationRange(currentPointerTarget))
            {
                currentPointerTarget.OnExitActivationRange();
            }
            currentPointerTarget = null;
        }
    }

    public bool IsInteractableWithinActivationRange(InteractableBase interactable)
    {
        return Vector3.Distance(interactable.transform.position, mainCamera.transform.position) <= activationRange;
    }

    public void ProcessInteractableActivationRange(InteractableBase interactable)
    {
        if (IsInteractableWithinActivationRange(interactable))
        {
            if (interactable.isActivated == false)
            {
                interactable.OnEnterActivationRange();
            }
        }
        else
        {
            if (interactable.isActivated)
            {
                interactable.OnExitActivationRange();
                
                if (interactable == currentPointerTarget)
                {
                    ClearPointerTarget();
                }
            }
        }
    }
}
