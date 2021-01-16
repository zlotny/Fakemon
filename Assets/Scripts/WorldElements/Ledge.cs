using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Ledge : MonoBehaviour
{
    [SerializeField]
    private FacingDirection m_jumpableDirection = FacingDirection.South;


    public FacingDirection GetJumpableDirection()
    {
        return m_jumpableDirection;
    }
}
