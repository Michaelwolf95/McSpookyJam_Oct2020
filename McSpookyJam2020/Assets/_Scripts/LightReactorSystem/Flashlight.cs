using System;
using System.Collections;
using System.Collections.Generic;
using MichaelWolfGames;
using UnityEngine;

public class Flashlight : SceneSingleton<Flashlight>
{
    //public float maxRange = 5f;
    [SerializeField]
    private Light spotLight = null;

    [SerializeField]
    private float triggerRange = 5f;
    
    [SerializeField, Range(0, 180f)] 
    private float triggerAngle = 25f;

    [SerializeField] 
    private LayerMask layerMask = new LayerMask();

    [SerializeField] 
    private bool isEnabled = false;
    
    private List<LightReactor> currentCollisions = null;

    
    protected override void Awake()
    {
        base.Awake();
        currentCollisions = new List<LightReactor>();
        if (spotLight == null)
        {
            spotLight = GetComponent<Light>();
        }
        
        Toggle(isEnabled);
    }

    public void Toggle(bool argEnabled)
    {
        isEnabled = argEnabled;
        spotLight.enabled = isEnabled;
    }

    private void FixedUpdate()
    {
        if (spotLight == null || isEnabled == false)
        {
            return;
        }
        RaycastHit[] coneHits = ConeCastAll(transform.position, transform.forward, triggerRange, triggerAngle, layerMask);
        
        DebugDrawCone(transform, transform.forward, triggerRange, triggerAngle, Color.yellow, Time.fixedDeltaTime);
        
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
                        
                        Debug.DrawLine(transform.position, coneHits[i].point, Color.green, 3f);
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
            currentCollisions.Remove(reactorToRemove);
            reactorToRemove.OnExitLight();
        }
    }
    
    private RaycastHit[] ConeCastAll(Vector3 origin, Vector3 direction, float maxDistance, float coneAngle, LayerMask layerMask)
    {
        float maxRadius = maxDistance * Mathf.Abs(Mathf.Tan((coneAngle / 2f)* Mathf.Deg2Rad));
        RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin - (direction*maxRadius), maxRadius, direction, maxDistance, layerMask.value, QueryTriggerInteraction.Collide);
        List<RaycastHit> coneCastHitList = new List<RaycastHit>();
        
        List<RaycastHit> raycastsToCheck = new List<RaycastHit>(sphereCastHits);
        raycastsToCheck.AddRange(Physics.RaycastAll(origin, direction, maxDistance, layerMask.value, QueryTriggerInteraction.Collide));

        if (raycastsToCheck.Count > 0)
        {
            for (int i = 0; i < raycastsToCheck.Count; i++)
            {
                if (raycastsToCheck[i].collider == null)
                {
                    continue;
                }
                //Debug.Log("SphereCast Hit");
                Vector3 hitPoint = raycastsToCheck[i].point;
                Vector3 directionToHit = (hitPoint - origin).normalized;
                float angleToHit = Vector3.Angle(direction, directionToHit);

                if (angleToHit < coneAngle)
                {
                    // Fire a direct ray to verify the hit.
                    RaycastHit verifyRayHit;
                    if (Physics.Raycast(origin, directionToHit, out verifyRayHit, maxDistance, layerMask.value, QueryTriggerInteraction.Collide))
                    {
                        if (verifyRayHit.collider == raycastsToCheck[i].collider)
                        {
                            //Debug.Log("Conecast Hit");
                            coneCastHitList.Add(raycastsToCheck[i]);
                            //Debug.DrawLine(origin, verifyRayHit.point, Color.yellow, 1f);
                        }
                        else
                        {
                            LightReactor reactor = raycastsToCheck[i].collider.gameObject.GetComponent<LightReactor>();
                            if (reactor == null && raycastsToCheck[i].rigidbody != null)
                            {
                                reactor = raycastsToCheck[i].rigidbody.gameObject.GetComponent<LightReactor>();
                            }
                            if (reactor)
                            {
                                Debug.DrawLine(origin, raycastsToCheck[i].point, Color.blue, 1f);
                                //Debug.Log(string.Format("Cone: {0}, Ray: {1}",raycastsToCheck[i].collider, verifyRayHit.collider));
                            }
                        }
                    }
                }
                else
                {
                    // Check if we hit a valid object but appears out of range.
//                    LightReactor reactor = raycastsToCheck[i].collider.gameObject.GetComponent<LightReactor>();
//                    if (reactor == null && raycastsToCheck[i].rigidbody != null)
//                    {
//                        reactor = raycastsToCheck[i].rigidbody.gameObject.GetComponent<LightReactor>();
//                    }
//                    if (reactor)
//                    {
//                        //Debug.DrawLine(origin, raycastsToCheck[i].point, Color.red, 1f);
//                        //Debug.Log(string.Format("Angle: {0}", angleToHit));
//                    }
                }
            }
        }
        return coneCastHitList.ToArray();

    }


    public void DebugDrawCone(Transform originTransform, Vector3 direction, float maxDistance, float coneAngle, Color color, float duration = 0.2f)
    {
        Vector3 origin = originTransform.position;
        float maxRadius = maxDistance * Mathf.Abs(Mathf.Tan((coneAngle / 2f)* Mathf.Deg2Rad));
        Vector3 endPoint = origin + direction * maxDistance;
        
        // Center
        Debug.DrawLine(origin, endPoint, new Color(0.95f, 0.6f, 0.1f), duration);
        
        Vector3 up = originTransform.up;
        Vector3 right = originTransform.right;

        int numPoints = 16;
        Vector3[] points = new Vector3[numPoints];
        
        float theta = 0f;
        float deltaTheta = (Mathf.PI * 2f) / numPoints;
        for(int i = 0; i < numPoints; i++)
        {
            float x = maxRadius * Mathf.Cos(theta);
            float y = maxRadius * Mathf.Sin(theta);
            points[i] = endPoint + (right * x) + (up * y);
            theta += deltaTheta;
        }

        for (int i = 0; i < numPoints; i++)
        {
            int j = (i + 1) % numPoints;
            Debug.DrawLine(origin, points[i], color, duration);
            Debug.DrawLine(points[i], points[j], color, duration);
        }
    }
}
