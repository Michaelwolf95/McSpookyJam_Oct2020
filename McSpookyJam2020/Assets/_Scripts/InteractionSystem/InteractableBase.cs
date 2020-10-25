using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InteractableBase : MonoBehaviour
{
    [SerializeField] 
    private Transform reticleRoot = null;
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
    
//    [System.Serializable]
//    public class ViewRange
//    {
//        public Vector2 xRange = new
//    }
    
    public Vector2 visibleAngleRange = new Vector2(360f, 360f);
    
    protected virtual void Awake()
    {
        activatedReticle.SetActive(false);
        pointerTargetReticle.SetActive(false);
    }
    
    protected virtual void Start()
    {
        if (Interactor.instance != null)
        {
            Interactor.instance.onBeginInteractionEvent += HideReticles;
            Interactor.instance.onFinishInteractionEvent += UnhideReticles;
        }
    }

    protected virtual void OnDestroy()
    {
        if (Interactor.instance != null)
        {
            Interactor.instance.onBeginInteractionEvent -= HideReticles;
            Interactor.instance.onFinishInteractionEvent -= UnhideReticles;
        }
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

    public virtual Transform GetLookTarget()
    {
        return (reticleRoot != null) ? reticleRoot : this.transform;
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

    protected virtual void HideReticles()
    {
        reticleRoot.gameObject.SetActive(false);
    }
    
    protected virtual void UnhideReticles()
    {
        reticleRoot.gameObject.SetActive(true);
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Transform lookTarget = GetLookTarget();
        if (visibleAngleRange.y < 360f)
        {
            Handles.color = new Color(0f, 1f, 0f, 0.35f);
            Handles.DrawSolidArc(lookTarget.position, lookTarget.up,
                Quaternion.AngleAxis(-visibleAngleRange.y / 2f, Vector3.up) * (lookTarget.forward), 
                visibleAngleRange.y, 0.2f);
        }

        if (visibleAngleRange.x < 360f)
        {
            Handles.color  = new Color(1f, 0f, 0f, 0.35f);
            Handles.DrawSolidArc(lookTarget.position, lookTarget.right,
                Quaternion.AngleAxis(-visibleAngleRange.x / 2f, Vector3.right) * (lookTarget.forward),
                visibleAngleRange.x, 0.2f);
        }
    }


#endif
    
}
