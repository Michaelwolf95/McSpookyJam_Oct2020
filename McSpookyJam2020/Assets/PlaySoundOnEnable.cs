using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
	public AK.Wwise.Event onEnableEvent;
	public AK.Wwise.Event onDisableEvent;
	
	private void OnEnable()
	{
		onEnableEvent.Post(gameObject);
	}
	
	private void OnDisable()
	{
		onDisableEvent.Post(gameObject);
	}
}
