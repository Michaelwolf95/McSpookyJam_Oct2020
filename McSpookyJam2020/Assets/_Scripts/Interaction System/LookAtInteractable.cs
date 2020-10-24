using System;
using UnityEngine;

namespace MichaelWolfGames.Interaction_System
{
    public class LookAtInteractable : InteractableBase
    {
        [SerializeField] [TextArea] 
        private string sayContent = "";
        
        [SerializeField]
        private float sayDelay = 0f;
        
        protected override void PerformInteraction()
        {
            Action performSay = () =>
            {
                ScreenTextManager.instance.SayText(sayContent, (() =>
                {
                    // On Complete?
                    Interactor.instance.QuitInteraction();
                }));

            };

            if (sayDelay <= 0f)
            {
                performSay();
            }
            else
            {
                this.StartTimer(sayDelay, performSay);
            }
        }
    }
}