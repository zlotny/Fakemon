using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    Transform m_target;
    
    [SerializeField]
    float m_horizontalOffset = 14f;
    [SerializeField]
    float m_verticalOffset = -11f;
    
    void Awake()
    {
        m_target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        this.transform.position = new Vector3(m_target.position.x + m_horizontalOffset, m_target.position.y + m_verticalOffset, this.transform.position.z);
    }
}
