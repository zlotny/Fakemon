using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FadeToWhite : MonoBehaviour
{
    private static FadeToWhite _instance;
    public static FadeToWhite Instance { get { return _instance; } }

    Animator m_animator = null;

    private void Awake()
    {
        if (_instance != null) throw new UnityException("There's already an instance of " + this.GetType().Name);
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.m_animator = this.GetComponent<Animator>();
    }

    public void Activate()
    {
        this.m_animator.SetTrigger("FadeToWhite");
    }
}
