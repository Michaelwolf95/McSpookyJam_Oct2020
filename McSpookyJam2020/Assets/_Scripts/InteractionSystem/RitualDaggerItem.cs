using MichaelWolfGames;
using UnityEngine;

public class RitualDaggerItem : InvestigationCardInteractable
{
    [SerializeField] private GameObject daggerObject = null;

    protected override void PerformInteraction()
    {
        base.PerformInteraction();
        
        daggerObject.SetActive(false);
        this.SetInteractable(false);
        
        this.StartTimer(1f, () =>
        {
            InventoryManager.instance.CollectRitualDagger();
        });
    }

    public override void OnFinishInteraction()
    {
        base.OnFinishInteraction();
    }
    
}