using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    [SerializeField] GameObject _dayLighting = null;
    [SerializeField] GameObject _nightLighting = null;
    [SerializeField] Material _daySkybox = null;
    [SerializeField] Material _nightSkybox = null;

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
