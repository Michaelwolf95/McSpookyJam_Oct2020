using UnityEngine;

namespace MichaelWolfGames.Interaction_System
{
    public class LookAtInteractable : InteractableBase
    {
        [SerializeField] [TextArea] 
        private string sayContent;
        
        
        protected override void PerformInteraction()
        {
            ScreenTextManager.instance.SayText(sayContent);
            
            Interactor.instance.QuitInteraction();
        }
    }
}