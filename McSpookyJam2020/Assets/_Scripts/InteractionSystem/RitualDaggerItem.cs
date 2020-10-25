using UnityEngine;

public class RitualDaggerItem : InteractableBase
{
    [SerializeField] private GameObject daggerObject;
    
    protected override void PerformInteraction()
    {
        InventoryManager.instance.CollectRitualDagger();
        daggerObject.SetActive(false);
        this.SetInteractable(false);

        Interactor.instance.QuitInteraction();
    }
}