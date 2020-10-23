using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    [SerializeField] GameObject _dayLighting;
    [SerializeField] GameObject _nightLighting;
    [SerializeField] Material _daySkybox;
    [SerializeField] Material _nightSkybox;

    [ContextMenu("DAY")]
    public void MakeDay()
    {
        _dayLighting.SetActive(true);
        _nightLighting.SetActive(false);
        RenderSettings.skybox = _daySkybox;
    }
    
    [ContextMenu("NIGHT")]
    public void MakeNight()
    {
        _dayLighting.SetActive(false);
        _nightLighting.SetActive(true);
        RenderSettings.skybox = _nightSkybox;
    }
}
