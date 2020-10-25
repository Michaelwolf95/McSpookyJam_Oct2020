using MichaelWolfGames;
using UnityEngine;

public class DayNightChangeListener : SubscriberBase<GameManager>
{
    protected override void FetchSubscribableObject()
    {
        SubscribableObject = GameManager.instance;
    }

    protected override void SubscribeEvents()
    {
        SubscribableObject.OnTransitionToNightEvent += OnNightTransition;
    }

    protected override void UnsubscribeEvents()
    {
        SubscribableObject.OnTransitionToNightEvent -= OnNightTransition;
    }

    protected virtual void OnNightTransition()
    {
        // OVERRIDE THIS.
    }
    
}