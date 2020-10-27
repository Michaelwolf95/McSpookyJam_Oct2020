using System;
using UnityEngine;

public class AnimatedFootstepEmitter : UnityEngine.MonoBehaviour
{
    [SerializeField] private Transform raycastOrigin = null;
    [Space(5)]
    [SerializeField] private AK.Wwise.Event groundFloorFootSteps = null;
    [SerializeField] private PhysicMaterial groundFloorMaterial = null;
    [SerializeField] private AK.Wwise.Event upstairsFootSteps = null;
    [SerializeField] private PhysicMaterial upstairsMaterial = null;
    [SerializeField] private AK.Wwise.Event basementFootSteps = null;
    [SerializeField] private PhysicMaterial basementMaterial = null;
    [SerializeField] private AK.Wwise.Event stairsFootSteps = null;
    [SerializeField] private PhysicMaterial stairsFloorMaterial = null;
    [SerializeField] private LayerMask floorLayerMask;
    
    private AK.Wwise.Event currentFootStepEvent = null;

    private void Awake()
    {
        if (raycastOrigin == null)
        {
            raycastOrigin = this.transform;
        }
        currentFootStepEvent = groundFloorFootSteps;
    }

    private void FixedUpdate()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(raycastOrigin.position, Vector3.down, out hit, floorLayerMask.value))
        {
            if (hit.collider.sharedMaterial == groundFloorMaterial)
            {
                currentFootStepEvent = groundFloorFootSteps;
            }
            else if (hit.collider.sharedMaterial == upstairsMaterial && upstairsFootSteps.IsValid())
            {
                currentFootStepEvent = upstairsFootSteps;
            }
            else if (hit.collider.sharedMaterial == basementMaterial && basementFootSteps.IsValid())
            {
                currentFootStepEvent = basementFootSteps;
            }
            else if (hit.collider.sharedMaterial == stairsFloorMaterial && stairsFootSteps.IsValid())
            {
                currentFootStepEvent = stairsFootSteps;
            }
            
            if(currentFootStepEvent == null || !currentFootStepEvent.IsValid())
            {
                currentFootStepEvent = groundFloorFootSteps;
            }
        }
    }

    public void PlayFootstep()
    {
        currentFootStepEvent.Post(gameObject);
    }
    
}