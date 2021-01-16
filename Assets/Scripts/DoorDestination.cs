using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDestination : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
    public Vector2 Destination()
    {
        return this.transform.position;
    }
}
