using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicItemsMover : DayNightChangeListener
{
    [SerializeField] private GameObject[] _dayObjects;
    [SerializeField] private GameObject[] _nightObjects;

    protected override void OnNightTransition()
    {
        foreach (GameObject o in _dayObjects)
        {
            o.SetActive(false);
        }
        foreach (GameObject o in _nightObjects)
        {
            o.SetActive(true);
        }
    }
}
