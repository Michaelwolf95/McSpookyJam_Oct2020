using Fungus;
using MichaelWolfGames;
using UnityEngine;

public class HagueLetterInvestigatable : InvestigationCardInteractable
{
    public int collectionIndex { get; set; }
    
    [SerializeField] private GameObject letterObject = null;
    
    protected override void PerformInteraction()
    {
        if (flowchartInstance == null)
        {
            collectionIndex = InventoryManager.instance.GetHagueLetterCount();
            flowchartInstance = InventoryManager.instance.GetHagueLetterCardInstance(collectionIndex);
        }

        base.PerformInteraction();
        letterObject.SetActive(false);
        this.SetInteractable(false);
        this.StartTimer(1f, () =>
        {
            InventoryManager.instance.CollectHagueLetter(collectionIndex);
        });
        
        GameManager.instance.ToggleMonster(false);
    }

    public override void OnFinishInteraction()
    {
        base.OnFinishInteraction();
        GameManager.instance.ToggleMonster(true);
    }
}