using UnityEngine;

public class HagueLetterInvestigatable : InvestigationCardInteractable
{
    public int collectionIndex { get; set; }

    public void OnCollected()
    {
        InventoryManager.instance.CollectHagueLetter(collectionIndex);
    }
}