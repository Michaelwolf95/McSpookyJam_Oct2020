using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    public virtual bool IsInteractable()
    {
        return true;
    }
    
    public virtual void OnEnterActivationRange()
    {
        
    }
    
    public virtual void OnExitActivationRange()
    {
        
    }
    
    public virtual void OnBecomeTarget()
    {
        
    }
    
    public virtual void OnNoLongerTarget()
    {
        
    }
    
    public virtual void OnBeginInteraction()
    {
        
    }
    
    public virtual void OnFinishInteraction()
    {
        
    }
}
