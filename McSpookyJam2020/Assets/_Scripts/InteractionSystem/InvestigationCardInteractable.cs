using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class InvestigationCardInteractable : InteractableBase
{
    [Space(5)]
    [SerializeField] 
    private Flowchart flowchartPrefab = null;

    [SerializeField] 
    private bool exitOnEscapeInput = false;

    private Flowchart flowchartInstance = null;

    protected override void Start()
    {
        base.Start();
        flowchartInstance = InvestigationCardCanvas.instance.InstantiateCard(flowchartPrefab);
    }

    protected override void Update()
    {
        base.Update();
        
        if (this.isInteracting && exitOnEscapeInput)
        {
            // ToDo: Move this behavior to Fungus flowchart?
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Interactor.instance.QuitInteraction();
            }
        }
    }

    protected override void PerformInteraction()
    {
        if (flowchartInstance != null)
        {
            GameManager.instance.OnEnterInvestigationCardScreen();
            
            flowchartInstance.ExecuteBlock("START");
        }
        else
        {
            Debug.LogError("No Flowchart Set!!!");
        }
    }

    public override void OnFinishInteraction()
    {
        base.OnFinishInteraction();
        
        GameManager.instance.OnExitInvestigationCardScreen();
        
        if (flowchartInstance != null)
        {
            flowchartInstance.ExecuteBlock("QUIT");
        }
    }
}
