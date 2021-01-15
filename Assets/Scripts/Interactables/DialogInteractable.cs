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
        UIDialogPanel.Instance.ShowText(m_message);
    }
}
