namespace MichaelWolfGames.Interaction_System
{
    public class DeskInvestigation : InvestigationCardInteractable
    {
        protected override void PerformInteraction()
        {
            base.PerformInteraction();
            
        }

        public override void OnFinishInteraction()
        {
            base.OnFinishInteraction();
            
            GameManager.instance.TransitionToNight();
        }
    }
}