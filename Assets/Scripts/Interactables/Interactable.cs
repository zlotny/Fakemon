using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour
{
    public virtual void Interact(GameObject source)
    {
        Debug.Log(source.name + " interacted with me: " + this.name);
    }
}
