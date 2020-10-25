using MichaelWolfGames;
using UnityEngine;

public class HagueLetterInvestigatable : InvestigationCardInteractable
{
    public int collectionIndex { get; set; }
    
    [SerializeField] private GameObject letterObject = null;

    public void OnCollected()
    {
        InventoryManager.instance.CollectHagueLetter(collectionIndex);
    }

    protected override void PerformInteraction()
    {
        base.PerformInteraction();
        letterObject.SetActive(false);
        this.SetInteractable(false);
        this.StartTimer(1f, () =>
        {
            OnCollected();
        });
    }

    public override void OnFinishInteraction()
    {
        base.OnFinishInteraction();
    }
}