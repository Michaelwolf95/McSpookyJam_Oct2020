using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerGlowSFX : MonoBehaviour
{
	public AK.Wwise.Event startGlowEvent;
	public AK.Wwise.Event stopGlowEvent;
	
	private void OnEnable()
	{
		startGlowEvent.Post(gameObject);
	}
	
	private void OnDisable()
	{
		stopGlowEvent.Post(gameObject);
	}
}
