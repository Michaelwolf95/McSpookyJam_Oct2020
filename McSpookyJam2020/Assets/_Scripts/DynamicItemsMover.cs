using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicItemsMover : DayNightChangeListener
{
    [SerializeField] private GameObject[] _dayObjects;
    [SerializeField] private GameObject[] _nightObjects;

    protected override void Start()
    {
        base.Start();
        ToggleObjects(false);
    }

    protected override void OnNightTransition()
    {
       ToggleObjects(true);
    }

    private void ToggleObjects(bool isNight)
    {
        foreach (GameObject o in _dayObjects)
        {
            o.SetActive(!isNight);
        }
        foreach (GameObject o in _nightObjects)
        {
            o.SetActive(isNight);
        }
    }
}
