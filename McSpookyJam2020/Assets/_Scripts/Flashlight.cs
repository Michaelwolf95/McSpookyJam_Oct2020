using System;
using System.Collections;
using System.Collections.Generic;
using MichaelWolfGames;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    //public float maxRange = 5f;
    [SerializeField]
    private Light spotLight = null;

    [SerializeField] 
    private LayerMask layerMask = new LayerMask();
    private List<LightReactor> currentCollisions = null;
    

    private void Awake()
    {
        currentCollisions = new List<LightReactor>();
        if (spotLight == null)
        {
            spotLight = GetComponent<Light>();
        }
    }

    private void FixedUpdate()
    {
        if (spotLight == null)
        {
            return;
        }
        RaycastHit[] coneHits = ConeCastAll(transform.position, transform.forward, spotLight.range, spotLight.spotAngle, layerMask);
        
        // Remove all currentCollisions unless they're hit by the cast.
        List<LightReactor> currentCollisionsToRemove = new List<LightReactor>(currentCollisions);
        if (coneHits.Length > 0)
        {
            for (int i = 0; i < coneHits.Length; i++)
            {
                LightReactor reactor = coneHits[i].collider.gameObject.GetComponent<LightReactor>();
                if (reactor == null && coneHits[i].rigidbody != null)
                {
                    reactor = coneHits[i].rigidbody.gameObject.GetComponent<LightReactor>();
                }
                if (reactor)
                {
                    if (currentCollisions.Contains(reactor) == false)
                    {
                        currentCollisions.Add(reactor);
                        reactor.OnEnterLight();
                        
                        Debug.DrawLine(transform.position, coneHits[i].point, Color.red, 1f);
                    }
                    else
                    {
                        currentCollisionsToRemove.Remove(reactor);
                    }
                }
            }
        }

        foreach (LightReactor reactorToRemove in currentCollisionsToRemove)
        {
            reactorToRemove.OnExitLight();
            currentCollisions.Remove(reactorToRemove);
        }
    }
    
    private RaycastHit[] ConeCastAll(Vector3 origin, Vector3 direction, float maxDistance, float coneAngle, LayerMask layerMask)
    {
        float maxRadius = maxDistance * Mathf.Abs(Mathf.Tan(coneAngle / 2f));
        RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin - (direction*maxRadius), maxRadius, direction, maxDistance, layerMask.value, QueryTriggerInteraction.Collide);
        List<RaycastHit> coneCastHitList = new List<RaycastHit>();
        
        if (sphereCastHits.Length > 0)
        {
            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                //Debug.Log("SphereCast Hit");
                Vector3 hitPoint = sphereCastHits[i].point;
                Vector3 directionToHit = hitPoint - origin;
                float angleToHit = Vector3.Angle(direction, directionToHit);

                if (angleToHit < coneAngle)
                {
                    // Fire a direct ray to verify the hit.
                    RaycastHit verifyRayHit;
                    if (Physics.Raycast(origin, direction, out verifyRayHit, maxDistance, layerMask.value, QueryTriggerInteraction.Collide))
                    {
                        if (verifyRayHit.collider == sphereCastHits[i].collider)
                        {
                            //Debug.Log("Conecast Hit");
                            coneCastHitList.Add(sphereCastHits[i]);
                        }
                    }
                }
            }
        }
        return coneCastHitList.ToArray();

    }
}
