using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

[CommandInfo("Custom", 
    "Quit Interaction", 
    "Quit current interaction.")]
public class QuitInteractionCommand :  Command
{
    public override void OnEnter()
    {
        Interactor.instance.QuitInteraction();
        
        Continue();
    }
}
