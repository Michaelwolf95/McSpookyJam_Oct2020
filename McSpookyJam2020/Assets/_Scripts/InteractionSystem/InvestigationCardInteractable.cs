﻿using System;
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

    public Flowchart flowchartInstance {get; set;}

    protected override void Start()
    {
        base.Start();
        if (flowchartPrefab)
        {
            flowchartInstance = InvestigationCardCanvas.instance.InstantiateCard(flowchartPrefab);
        }
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
        
        if (flowchartInstance != null)
        {
            flowchartInstance.ExecuteBlock(flowchartInstance.FindBlock("QUIT"), 0, () =>
            {
                GameManager.instance.OnExitInvestigationCardScreen();
            });
        }
        else
        {
            // Quit early if no flowchart.
            GameManager.instance.OnExitInvestigationCardScreen();
        }
    }
}
