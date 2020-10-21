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
    
    private InteractableBase currentTarget = null;
    private bool isInteracting = false;
    
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (isInteracting == false)
        {
            if (currentTarget != null)
            {
                // Listen for interaction key input.
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentTarget.OnBeginInteraction();
                }
            }
        }
        
    }

    public void FixedUpdate()
    {
        if (isInteracting)
        {
            return;
        }
        
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
                if (currentTarget != null && currentTarget != raycastResultTarget)
                {
                    currentTarget.OnNoLongerTarget();
                }

                currentTarget = raycastResultTarget;
                currentTarget.OnBecomeTarget();
            }
        }
    }

    public void BeginInteraction(InteractableBase interactable)
    {
        
    }
    
    public void CompleteInteraction(InteractableBase interactable)
    {
        
    }
}
