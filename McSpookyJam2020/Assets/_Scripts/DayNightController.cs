using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    [SerializeField] GameObject _dayLighting;
    [SerializeField] GameObject _nightLighting;

    public void MakeDay()
    {
        _dayLighting.SetActive(true);
        _nightLighting.SetActive(false);
    }

    public void MakeNight()
    {
        _dayLighting.SetActive(false);
        _nightLighting.SetActive(true);
    }
}
