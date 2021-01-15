using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToWhite : MonoBehaviour
{
    private static FadeToWhite _instance;
    public static FadeToWhite Instance { get { return _instance; } }

    Animator animator = null;

    private void Awake()
    {
        if (_instance != null)
        {
            throw new UnityException("There's already an instance of FadeToWhite");
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        this.animator.SetTrigger("FadeToWhite");
    }
}
