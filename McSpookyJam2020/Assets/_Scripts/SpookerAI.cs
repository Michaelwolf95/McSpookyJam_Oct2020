using System;
using System.Collections;
using System.Collections.Generic;
using MichaelWolfGames;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SpookerAI : LightReactor
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private GameObject visualRoot = null;
    [Space(5)]
    [SerializeField] private float killDistance = 1f;
    [SerializeField] private float unHideMinDistance = 8f;

    [Space(5)] 
    [SerializeField] private AgentStateParams disabledStateParams = new AgentStateParams();
    [SerializeField] private AgentStateParams wanderingStateParams = new AgentStateParams();
    [SerializeField] private AgentStateParams followingStateParams = new AgentStateParams();
    [SerializeField] private AgentStateParams fearedStateParams = new AgentStateParams();
    
    private Transform target;

    private static float MIN_FLOOR_HEIGHT = 1f;

    private static float MIN_FEAR_TIME = 5f;
    private static float MAX_FEAR_TIME = 15f;

    private bool readyToUnHide = false;

    [System.Serializable]
    public struct AgentStateParams
    {
        public float speed;
        public float angularSpeed;
        public float acceleration;
    }
    
    enum SpookerState
    {
        Disabled,
        Wandering,
        Following,
        Feared,
        Hiding
    }

    private Color defaultColor;
    private SpookerState currentState = SpookerState.Disabled;
    
    private void Start()
    {
        if (PlayerInstance.Instance != null)
        {
            target = PlayerInstance.Instance.transform;
        }

        defaultColor = spriteRenderer.color;
        // TEMP:
        ChangeState(SpookerState.Following);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || currentState == SpookerState.Disabled)
        {
            return;
        }

        Vector3 toTargetVector = (target.position - transform.position);
        switch (currentState)
        {
            case SpookerState.Disabled:
                break;
            case SpookerState.Wandering:
                break;
            case SpookerState.Following:
            {
                agent.SetDestination(target.position);

                if (Vector3.Distance(this.transform.position, target.position) <= killDistance)
                {
                    Debug.Log("KILLED PLAYER");
                    //currentState = SpookerState.Disabled;
                }
                break;
            }
            case SpookerState.Hiding:
            {
                if (readyToUnHide && (toTargetVector.magnitude > unHideMinDistance))
                {
                    ChangeState(SpookerState.Following);
                    return;
                }
                RunAway();
                break;
            }
            case SpookerState.Feared:
            {
                if ( currentState == SpookerState.Feared && IsVisible() == false)
                {
                    ChangeState(SpookerState.Hiding);
                }

                RunAway();
                break;
            }
            
            default:
                break;
        }
    }

    private void RunAway()
    {
        Vector3 toTargetVector = (target.position - transform.position);
        Vector3 awayDir = Vector3.zero;
        if (isInLight)
        {
            Vector3 lightOriginRelativePos = Vector3.ProjectOnPlane(transform.position - Camera.main.transform.position, Vector3.up);
            Vector3 lightNormal = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);

            Vector3 projRelativePos = Vector3.Project(lightOriginRelativePos, lightNormal);

            awayDir = (lightOriginRelativePos - projRelativePos).normalized;
                    
        }
        else
        {
            Vector3 awayFromPlayer = -Vector3.ProjectOnPlane(toTargetVector.normalized, Vector3.up).normalized;
                    
                    
            //float angleDelta = 45f;
            //float randAngle = (Random.Range((int) 0, 2) * 2 - 1) * Random.Range(90 - angleDelta, 90 + angleDelta);
                    
            awayDir = Quaternion.Euler(0f, Random.Range(-90, 90f), 0f) * awayFromPlayer;
        }
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (awayDir * 1f), out hit, 500f, NavMesh.AllAreas);
        if (hit.hit)
        {
            agent.SetDestination(hit.position);
            Debug.DrawLine(transform.position, hit.position, Color.magenta, 1f);
        }
    }

    private void ChangeState(SpookerState argState)
    {
        if (argState == currentState)
        {
            return;
        }
        
        switch (currentState) // EXITING STATE
        {
            case SpookerState.Disabled:
                break;
            case SpookerState.Wandering:
                break;
            case SpookerState.Following:
                break;
            case SpookerState.Feared:
                break;
            case SpookerState.Hiding:
                // Become visible.
                readyToUnHide = true;
                spriteRenderer.color = defaultColor;
                visualRoot.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        // STATE ENTER:
        currentState = argState;
        switch (currentState) // ENTERING STATE
        {
            case SpookerState.Disabled:
                AssignAgentStateParams(disabledStateParams);
                break;
            case SpookerState.Wandering:
                AssignAgentStateParams(wanderingStateParams);
                break;
            case SpookerState.Following:
                AssignAgentStateParams(followingStateParams);
                break;
            case SpookerState.Feared:
                AssignAgentStateParams(fearedStateParams);
                break;
            case SpookerState.Hiding:
                // Become invisible.
                readyToUnHide = false;
                visualRoot.SetActive(false);
                AssignAgentStateParams(fearedStateParams);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void AssignAgentStateParams(AgentStateParams stateParams)
    {
        agent.speed = stateParams.speed;
        agent.acceleration = stateParams.acceleration;
        agent.angularSpeed = stateParams.angularSpeed;
    }
    
    public override void OnEnterLight()
    {
        base.OnEnterLight();

        ChangeState(SpookerState.Feared);
        
//        Vector3 awayFromPlayer = -Vector3.ProjectOnPlane((target.position - transform.position).normalized, Vector3.up).normalized;
//        NavMeshHit hit;
//        NavMesh.SamplePosition(transform.position + awayFromPlayer * 100f, out hit, 500f, 1 << NavMesh.GetNavMeshLayerFromName("Default"));
//        agent.SetDestination(hit.position);

        this.DoTween(1f,(lerp) =>
        {
            spriteRenderer.color = Color.Lerp(defaultColor, Color.clear, lerp);
        },  () =>
        {
            ChangeState(SpookerState.Hiding);
        });
        

        // ToDo: Stop for a moment first?
        this.InvokeAction((() =>
        {
            readyToUnHide = true;
            //ChangeState(SpookerState.Following);
        }), Random.Range(MIN_FEAR_TIME, MAX_FEAR_TIME));
    }

    public override void OnExitLight()
    {
        base.OnExitLight();
        
    }

//    private Vector3 FindFurthestAwayPoint()
//    {
//        
//    }

    
    // http://wiki.unity3d.com/index.php/IsVisibleFrom
    private bool IsVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, spriteRenderer.bounds);
    }
}
