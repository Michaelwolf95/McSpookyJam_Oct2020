using MichaelWolfGames;
using UnityEngine.Events;

public class DayNightTrigger : DayNightChangeListener
{
        public UnityEvent onBecomeDay = new UnityEvent();
        public UnityEvent onBecomeNight = new UnityEvent();
        
        protected override void Start()
        {
                base.Start();
                
                // HACK: Wait until after start to override any thing that happens in start (like setting interactable.)
                this.StartTimer(0.1f, (() =>
                {
                        onBecomeDay.Invoke();
                }));
        }
        
        protected override void OnNightTransition()
        {
                base.OnNightTransition();
                onBecomeNight.Invoke();
        }
}