using System;
using UnityEngine;

[Serializable]
public enum InteractReticleType
{
    Look,
    Investigate,
    Hand,
}

public class InteractableReticle : MonoBehaviour
{
    [SerializeField] private GameObject activatedReticle;
    [Space(3)]
    [SerializeField] private GameObject lookReticle;
    [SerializeField] private GameObject investigateReticle;
    [SerializeField] private GameObject handReticle;

    private InteractReticleType currentInteractType = InteractReticleType.Look;

    private void Awake()
    {
        activatedReticle.gameObject.SetActive(false);
        lookReticle.gameObject.SetActive(false);
        investigateReticle.gameObject.SetActive(false);
        handReticle.gameObject.SetActive(false);
    }

    public GameObject GetActivatedReticleGameObject()
    {
        return activatedReticle;
    }

    public GameObject GetInteractReticleGameObject(InteractReticleType argType)
    {
        switch (argType)
        {
            case InteractReticleType.Look:
                return lookReticle;
            case InteractReticleType.Investigate:
                return investigateReticle;
            case InteractReticleType.Hand:
                return handReticle;
            default:
                throw new ArgumentOutOfRangeException(nameof(argType), argType, null);
        }
    }
    
    public void ToggleActivatedReticle(bool argEnable)
    {
        activatedReticle.gameObject.SetActive(argEnable);
    }
    
    public void ToggleInteractReticle(bool argEnable)
    {
        GetInteractReticleGameObject(currentInteractType).gameObject.SetActive(argEnable);
    }

    public void SetInteractType(InteractReticleType argType)
    {
        if (currentInteractType != argType)
        {
            bool prevActiveState = GetInteractReticleGameObject(currentInteractType).gameObject.activeSelf;
            ToggleInteractReticle(false);
            currentInteractType = argType;
            ToggleInteractReticle(prevActiveState);
        }
    }
    
}
