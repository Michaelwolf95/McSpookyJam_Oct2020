using System;
using System.Collections;
using MichaelWolfGames;
using UnityEngine;

public static class MenuTweenEffects
{
    public static IEnumerator ScalePressEffect(RectTransform button, Action onClickAction, float scaleFactor = 1.25f, float duration = 1f, float finishDelay = 0.1f, bool useRealTime = false)
    {
        Vector3 startScale = button.localScale;
        Vector3 endScale = startScale * scaleFactor;
        yield return CoroutineExtensionMethods.CoDoTween(lerp =>
        {
            button.localScale = Vector3.LerpUnclamped(startScale, endScale, lerp);
        }, null, duration, 0f, EaseType.punch, useRealTime);
        if (finishDelay > 0f)
        {
            if (useRealTime)
            {
                yield return new WaitForSecondsRealtime(finishDelay);
            }
            else
            {
                yield return new WaitForSeconds(finishDelay);
            }
        }
        if (onClickAction != null)
        {
            onClickAction();
        }
    }
    
}