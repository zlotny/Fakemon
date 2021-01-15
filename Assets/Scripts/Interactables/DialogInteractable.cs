using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteractable : Interactable
{
    [SerializeField]
    private string m_message = "";

    public override void Interact(GameObject source)
    {
        base.Interact(source);
        Debug.Log("This should spawn a dialog on screen for text: " + this.m_message);
    }
}
