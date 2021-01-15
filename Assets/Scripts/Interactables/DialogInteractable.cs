using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DialogInteractable : Interactable
{
    [SerializeField]
    [Multiline]
    private string m_message = "";

    public override void Interact(GameObject source)
    {
        base.Interact(source);
        UIDialogPanel.Instance.ShowText(m_message);
    }
}
