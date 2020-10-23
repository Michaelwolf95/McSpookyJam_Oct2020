﻿using UnityEngine;
using System.Collections;
using System;

namespace MichaelWolfGames
{

    public static class CoroutineExtensionMethods
    {

        public static void InvokeAction(this MonoBehaviour invokedOn, Action action, float time, bool useRealTime = false)
        {
            if (time <= 0f)
            {
                action();
            }
            else
            {
                invokedOn.StartCoroutine(CoWaitForSeconds(action, time, useRealTime));
            }
        }
        public static void InvokeAction(this MonoBehaviour invokedOn, Action action, YieldInstruction yieldInstruction)
        {
            if (yieldInstruction == null)
            {
                Debug.LogError("[CoroutineExtensionMethods]: Invoked Enumerator is Null!");
                action();
            }
            else
            {
                invokedOn.StartCoroutine(CoWaitForYieldInstruction(action, yieldInstruction));
            }
        }

        public static void InvokeActionAtEndOfFrame(this MonoBehaviour invokedOn, Action action)
        {
            invokedOn.InvokeAction(
                action,
                new WaitForEndOfFrame()
                );
        }



        private static IEnumerator CoWaitForSeconds(Action actionCallback, float time, bool useRealTime = false)
        {
            if (useRealTime)
            {
                yield return new WaitForSecondsRealtime(time);
            }
            else
            {
                yield return new WaitForSeconds(time);
            }
            actionCallback();
        }

        private static IEnumerator CoWaitForYieldInstruction(Action actionCallback, YieldInstruction yieldInstruction)
        {
            yield return yieldInstruction;
            actionCallback();
        }

        public static void WaitForBool(this MonoBehaviour invokedOn, Func<bool> checkValue, Action onDoneCallback)
        {
            WaitForBool(invokedOn, checkValue, true, onDoneCallback);
        }

        public static void WaitForBool(this MonoBehaviour invokedOn, Func<bool> checkValue, bool targetValue,
            Action onDoneCallback)
        {
            invokedOn.StartCoroutine(CoWaitForBool(checkValue, targetValue, onDoneCallback));
        }

        private static IEnumerator CoWaitForBool(Func<bool> checkValue, bool targetValue, Action onDoneCallback)
        {
            while (checkValue() != targetValue)
            {
                yield return null;
            }
            onDoneCallback();
        }

        public static void DoWhile(this MonoBehaviour invokedOn, Action doAction, Func<bool> getWhileValue,
            Action onDoneCallback = null)
        {
            invokedOn.StartCoroutine(CoDoWhile(doAction, getWhileValue, onDoneCallback));
        }

        private static IEnumerator CoDoWhile(Action doAction, Func<bool> getWhileValue, Action onDoneCallback = null)
        {
            while (getWhileValue())
            {
                doAction();
                yield return null;
            }
            if (onDoneCallback != null)
                onDoneCallback();
        }
        
        public static void StartTimer(this MonoBehaviour invokedOn, float duration, Action onDoneCallback = null)
        {
            invokedOn.StartCoroutine(CoDoTimer(duration, onDoneCallback));
        }
        
        private static IEnumerator CoDoTimer(float duration, Action onDoneCallback = null)
        {
            yield return new WaitForSeconds(duration);
            
            if (onDoneCallback != null)
                onDoneCallback();
        }
        
        public static void DoTween(this MonoBehaviour invokedOn, Action<float> tweenAction, Action onDoneCallback = null, float duration = 0f, EaseType easeType = EaseType.linear)
        {
            invokedOn.StartCoroutine(CoDoTween( tweenAction, onDoneCallback, duration, easeType));
        }

        private static IEnumerator CoDoTween(Action<float> tweenAction, Action onDoneCallback = null, float duration = 0f, EaseType easeType = EaseType.linear)
        {
            TweenEasing.EasingFunction easeFunc = TweenEasing.GetEasingFunction(easeType);
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                switch (easeType)
                {
                    case EaseType.punch:
                        tweenAction(TweenEasing.punch(1f, timer / duration));
                        break;
                    default:
                        tweenAction(easeFunc(0,1, timer / duration));
                        break;
                }
                yield return null;
            }
            
            if (onDoneCallback != null)
                onDoneCallback();
        }
    }
}