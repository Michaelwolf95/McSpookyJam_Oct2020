using System;
using System.Collections;
using MichaelWolfGames;
using UnityEngine;

public static class MenuTweenEffects
{
    public static IEnumerator ScalePressEffect(RectTransform button, Action onClickAction, float scaleFactor = 1.25f, float duration = 1f, float finishDelay = 0.5f)
    {
        Vector3 startScale = button.localScale;
        Vector3 endScale = startScale * scaleFactor;
        yield return CoroutineExtensionMethods.CoDoTween(lerp =>
        {
            button.localScale = Vector3.LerpUnclamped(startScale, endScale, lerp);
        }, null, duration, 0f, EaseType.punch);

        yield return new WaitForSeconds(finishDelay);
        if (onClickAction != null)
        {
            onClickAction();
        }
    }
    
}