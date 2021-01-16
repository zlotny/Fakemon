using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour
{
    DoorDestination m_doorDestination;
    Rigidbody2D m_rigidBody;

    void Start()
    {
        m_doorDestination = GetComponentInChildren<DoorDestination>();
        if (m_doorDestination == null) throw new UnityException("Component Door has no DoorDestination in its children");
        m_rigidBody = GetComponent<Rigidbody2D>();
        if (m_rigidBody == null) throw new UnityException("Component Door has no RigidBody2D in its children");
        m_rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterMover playerCharacterMover = other.gameObject.GetComponent<CharacterMover>();
        if (playerCharacterMover == null)
            return;

        FadeToWhite.Instance.Activate();
        playerCharacterMover.RequestTeleportToPoint(m_doorDestination.Destination());
    }
}
