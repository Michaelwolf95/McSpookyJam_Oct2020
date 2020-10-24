using Fungus;
using MichaelWolfGames;
using UnityEngine;

public class InvestigationCardCanvas : SceneSingleton<InvestigationCardCanvas>
{
    
    public Flowchart InstantiateCard(Flowchart cardFlowchartPrefab)
    {
        Flowchart flowchart = GameObject.Instantiate(cardFlowchartPrefab.gameObject, this.transform).GetComponent<Flowchart>();
        
        
        return flowchart;
    }
}