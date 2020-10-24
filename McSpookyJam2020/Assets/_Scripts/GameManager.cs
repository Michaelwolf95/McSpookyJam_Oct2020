using System;
using System.Collections;
using Fungus;
using MichaelWolfGames;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : SceneSingleton<GameManager>
{
    public DayNightController dayNightController = null;
    public SpookerAI monsterController;

    public AK.Wwise.Switch Day;
    public AK.Wwise.Switch Night;
    public AK.Wwise.Event DeathSound;
    public AK.Wwise.Event StopAMB;
    public AK.Wwise.Event PlayAMB;
    public AK.Wwise.Event StopMusic;
    public AK.Wwise.Event PlayMusic;
    public AK.Wwise.Event GameStart;
    
    public bool isNight { get; private set; }

    private void Start()
    {
        OnGameStart();
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
    }

    public void OnExitInvestigationCardScreen()
    {
        // Free player movement.
        FirstPersonAIO.instance.SetControllerPause(false);
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
        }
    }

    public void OnPlayerDeath()
    {
        DeathSound.Post(gameObject);
        StopAMB.Post(gameObject);
        StopMusic.Post(gameObject);
        Debug.Log("PLAYER FUCKING DIED");
        
        
        this.DoTween((lerp =>
        {
            
        } ));
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartAttackEffect(1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CancelAttackEffect();
        }
    }

    private Coroutine attackEffectCoroutine = null;
    private Action onAttackEffectCancelled = null;
    
    
    public void StartAttackEffect(float duration, Action onComplete = null, Action onCancelled = null)
    {
        if (attackEffectCoroutine != null)
        {
            Debug.Log("Starting attack effect while it's running!!!");
            return;
        }

        onAttackEffectCancelled = onCancelled;
        StartCoroutine(CoAttackEffect(duration, onComplete));
    }
    
    public void CancelAttackEffect()
    {
        if (attackEffectCoroutine == null)
        {
            //Debug.Log("Starting attack effect while it's running!!!");
            return;
        }

        Debug.Log("Attack Cancelled.");
        
        StopCoroutine(attackEffectCoroutine);

        if (onAttackEffectCancelled != null)
            onAttackEffectCancelled();
        
        attackEffectCoroutine = null;
        onAttackEffectCancelled = null;
    }
    
    private IEnumerator CoAttackEffect(float duration, Action onDoneCallback = null)
    {
        
        CameraManager cameraManager = FungusManager.Instance.CameraManager;
        cameraManager.ScreenFadeTexture = CameraManager.CreateColorTexture(Color.black, 32, 32);
        
//        cameraManager.Fade(targetAlpha, duration, delegate { 
//            if (waitUntilFinished)
//            {
//                Continue();
//            }
//        }, fadeTweenType);
        
        
        PostProcessVolume ppVolume = Camera.main.GetComponentInChildren<PostProcessVolume>();
        PostProcessProfile profile = ppVolume.profile;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            profile.GetSetting<Vignette>().intensity.value = Mathf.Lerp(0f, 1f, timer / duration);
//            VignetteModel.Settings vign = profile.vignette.settings;
//            vign.intensity = vignIntensity;
//            profile.vignette.settings = vign;
            yield return null;
        }
        
        attackEffectCoroutine = null;
        onAttackEffectCancelled = null;
        
        if (onDoneCallback != null)
            onDoneCallback();
        
        OnPlayerDeath();
    }
    
}
