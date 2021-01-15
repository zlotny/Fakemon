using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour
{
    public Vector2 destination;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterControls playerCharacterControls = other.gameObject.GetComponent<CharacterControls>();
        if(playerCharacterControls != null){
            FadeToWhite.Instance.Activate();
            playerCharacterControls.RequestTeleportToPoint(destination);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
