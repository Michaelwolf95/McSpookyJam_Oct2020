using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class InvestigationCardInteractable : InteractableBase
{
    [Space(5)]
    [SerializeField] 
    private Flowchart flowchart = null;

    [SerializeField] 
    private bool exitOnEscapeInput = true;
    
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
        FirstPersonAIO.instance.SetControllerPause(true);
        
        if (flowchart != null)
        {
            flowchart.ExecuteBlock("START");
        }
        else
        {
            Debug.LogError("No Flowchart Set!!!");
        }
    }

    public override void OnFinishInteraction()
    {
        base.OnFinishInteraction();
        
        // ToDo: Free player movement.
        FirstPersonAIO.instance.SetControllerPause(false);
        
        if (flowchart != null)
        {
            flowchart.ExecuteBlock("QUIT");
        }
    }
}
