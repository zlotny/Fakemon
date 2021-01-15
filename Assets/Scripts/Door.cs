using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour
{
    public Vector2 m_destination;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterControls playerCharacterControls = other.gameObject.GetComponent<CharacterControls>();
        if (playerCharacterControls == null)
            return;

        FadeToWhite.Instance.Activate();
        playerCharacterControls.RequestTeleportToPoint(m_destination);
    }
}
