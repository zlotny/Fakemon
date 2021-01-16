using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DialogInteractable : Interactable
{
    [SerializeField]
    [Multiline]
    private string m_message = "";

    private WanderingNPC m_wanderingNPC;

    void Awake()
    {
        m_wanderingNPC = GetComponent<WanderingNPC>();
    }

    public override void Interact(GameObject source)
    {
        base.Interact(source);
        CharacterMover player = source.GetComponent<CharacterMover>();

        if (m_wanderingNPC != null && player != null)
        {
            m_wanderingNPC.StopMoving();
            // FIXME: Remember to restore movement!
            m_wanderingNPC.FaceTo(player.FacingDirectionToVector(player.GetFacingDirection()) * -1);
        }
        UIDialogPanel.Instance.ShowText(m_message);
    }
}
