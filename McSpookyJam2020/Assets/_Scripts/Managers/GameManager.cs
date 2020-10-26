using System;
using System.Collections;
using Fungus;
using MichaelWolfGames;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;

public class GameManager : SceneSingleton<GameManager>
{
    public static int MAIN_MENU_SCENE_INDEX = 0;
    public static int GAME_SCENE_INDEX = 1;
    
    public DayNightController dayNightController = null;
    public SpookerAI monsterController;

    public CanvasGroup attackEffectCanvasGroup = null;

    public GameOverMenu gameOverMenu = null;
    public VictoryMenu victoryMenu = null;

    public bool IsPlayerOnCardScreen { get; private set; }
    public bool IsPlayerInMenu { get; set; }
    
    public event Action OnTransitionToNightEvent = delegate {  };

    [Header("Wwise Events")]
    public AK.Wwise.Switch Day;
    public AK.Wwise.Switch Night;
    public AK.Wwise.Event DeathSound;
    public AK.Wwise.Event StopAMB;
    public AK.Wwise.Event PlayAMB;
    public AK.Wwise.Event StopMusic;
    public AK.Wwise.Event PlayMusic;
    public AK.Wwise.Event GameStart;

    [Header("Debug Waypoints")] 
    [SerializeField] private Transform startWaypoint = null;
    [SerializeField] private Transform studyWaypoint = null;
    [SerializeField] private Transform ritualWaypoint = null;
    
    public bool isNight { get; private set; }

    private void Start()
    {
        OnGameStart();
    }
    
    private void Update()
    {
        // DEBUG BUTTONS
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            bool visible = IsPlayerInMenu || IsPlayerOnCardScreen;
            Cursor.visible = visible;
            Cursor.lockState = (visible) ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            StartAttackEffect(1f);
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            CancelAttackEffect();
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            TransitionToNight();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InventoryManager.instance.CollectFlashlight();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            InventoryManager.instance.CollectBasementKey();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            InventoryManager.instance.CollectRitualDagger();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            InventoryManager.instance.CollectHagueLetter(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            FindObjectOfType<BasementDoorInteractable>().OpenDoor();
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            DebugTeleportPlayer(startWaypoint);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            DebugTeleportPlayer(studyWaypoint);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            DebugTeleportPlayer(ritualWaypoint);
        }
        
#endif
    }

    private void OnGameStart()
    {
        dayNightController.MakeDay();
        monsterController.gameObject.SetActive(false);
        GameStart.Post(gameObject);
        PlayAMB.Post(gameObject);
        PlayMusic.Post(gameObject);
        Day.SetValue(gameObject);
        // ToDo: Ambiance start
    }

    public void OnEnterInvestigationCardScreen()
    {
        // Lock player movement
        FirstPersonAIO.instance.SetControllerPause(true);
        Flashlight.instance.Toggle(false);
        IsPlayerOnCardScreen = true;
    }

    public void OnExitInvestigationCardScreen()
    {
        // Free player movement.
        FirstPersonAIO.instance.SetControllerPause(false);
        Flashlight.instance.Toggle(InventoryManager.instance.hasFlashlight);
        IsPlayerOnCardScreen = false;
    }

    public void TransitionToNight()
    {
        if (isNight == false)
        {
            isNight = true;
            dayNightController.MakeNight();
            monsterController.gameObject.SetActive(true); // Goes right in front of the door
            Night.SetValue(gameObject);
            StopAMB.Post(gameObject);
            PlayAMB.Post(gameObject);

            if (OnTransitionToNightEvent != null)
            {
                OnTransitionToNightEvent.Invoke();
            }
        }
    }

    public void OnBasementDoorOpened()
    {
        
    }
    
    public void OnRitualStarted()
    {
        monsterController.gameObject.SetActive(false);
    }
    
    public void OnRitualFinished()
    {
        victoryMenu.FadeInMenu();
    }

#if UNITY_EDITOR

    private void DebugTeleportPlayer(Transform waypoint)
    {
        FirstPersonAIO.instance.SetControllerPause(true);
        FirstPersonAIO.instance.transform.position = waypoint.position;
        FirstPersonAIO.instance.transform.rotation = waypoint.rotation;
        FirstPersonAIO.instance.SetControllerPause(false);
    }
#endif


    #region Camera Effects

    
    // ToDo: Move these effects somewhere else.

    private Coroutine attackEffectCoroutine = null;
    private Action onAttackEffectCancelled = null;
    
    private Coroutine recoverEffectCoroutine = null;
    
    public void StartAttackEffect(float duration, Action onComplete = null, Action onCancelled = null)
    {
        if (attackEffectCoroutine != null)
        {
            //Debug.Log("Starting attack effect while it's running!!!");
            return;
        }

        if (recoverEffectCoroutine != null)
        {
            StopCoroutine(recoverEffectCoroutine);
            recoverEffectCoroutine = null;
        }

        onAttackEffectCancelled = onCancelled;
        
        Debug.Log("Attack Started.");
        attackEffectCoroutine = StartCoroutine(CoAttackEffect(duration, onComplete));
    }
    
    public void CancelAttackEffect()
    {
        if (recoverEffectCoroutine != null)
        {
            //Debug.Log("Cancelling Attack while in recover state???");
            return;
        }
        
        if (attackEffectCoroutine == null)
        {
            //Debug.Log("Starting attack effect while it's running!!!");
            return;
        }

        Debug.Log("Attack Cancelled.");
        
        StopCoroutine(attackEffectCoroutine);
        CleanupAttackEffect();

        if (onAttackEffectCancelled != null)
            onAttackEffectCancelled();
        
        // START RECOVER
        recoverEffectCoroutine = StartCoroutine(CoRecoverEffect(0.5f));
        
    }
    
    private IEnumerator CoAttackEffect(float duration, Action onDoneCallback = null)
    {
        attackEffectCanvasGroup.gameObject.SetActive(true);
        float startAlpha = attackEffectCanvasGroup.alpha;
        
        PostProcessVolume ppVolume = Camera.main.GetComponentInChildren<PostProcessVolume>();
        PostProcessProfile profile = ppVolume.profile;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float lerp = TweenEasing.easeInQuad(0f, 1f,timer / duration);
            profile.GetSetting<Vignette>().intensity.value = Mathf.Lerp(0f, 1f, lerp);

            attackEffectCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, lerp);
            
            float magnitude = Mathf.Lerp(0.01f, 0.1f, lerp);
            Camera.main.transform.localPosition = new Vector3(Random.Range(-1f, 1f) * magnitude, Random.Range(-1f, 1f) * magnitude, 0f);
            
            //Debug.Log(lerp);
            yield return null;
        }

        CleanupAttackEffect();
        
        if (onDoneCallback != null)
            onDoneCallback();
        
        OnPlayerDeath();
    }

    private void CleanupAttackEffect()
    {
        Camera.main.transform.localPosition = Vector3.zero;
        attackEffectCoroutine = null;
        onAttackEffectCancelled = null;
        
        PostProcessVolume ppVolume = Camera.main.GetComponentInChildren<PostProcessVolume>();
        PostProcessProfile profile = ppVolume.profile;
        profile.GetSetting<Vignette>().intensity.value = 0f;
    }
    
    private IEnumerator CoRecoverEffect(float duration, Action onDoneCallback = null)
    {
        attackEffectCanvasGroup.gameObject.SetActive(true);
        float startAlpha = attackEffectCanvasGroup.alpha;
        
        //PostProcessVolume ppVolume = Camera.main.GetComponentInChildren<PostProcessVolume>();
        //PostProcessProfile profile = ppVolume.profile;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float lerp = timer / duration;
            //profile.GetSetting<Vignette>().intensity.value = Mathf.Lerp(0f, 1f, lerp);
            attackEffectCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, lerp);
            yield return null;
        }

        recoverEffectCoroutine = null;
    }
    
    
    private void OnPlayerDeath()
    {
        DeathSound.Post(gameObject);
        StopAMB.Post(gameObject);
        StopMusic.Post(gameObject);
        Debug.Log("PLAYER FUCKING DIED");
        
        FirstPersonAIO.instance.SetControllerPause(true);
        monsterController.DisableController();
        
        gameOverMenu.FadeInMenu();
    }


    #endregion

}
