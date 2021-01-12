using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Follows the player around
[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    Transform target;
    
    [SerializeField]
    float m_horizontalOffset = 13.6f;
    [SerializeField]
    float m_verticalOffset = -11f;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(target.position.x + m_horizontalOffset, target.position.y + m_verticalOffset, this.transform.position.z);
    }
}
