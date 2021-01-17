using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    public static PlayerInformation Instance { get; private set; }

    public string m_name = "GOLD";
    public int m_money = 10000;
    public int m_trainerId = -1;
    public float m_secondsPlayed = 0f;
    // FIXME: This is wrong. Just create a container for it.    
    public bool m_gotMedal1 = false;
    public bool m_gotMedal2 = false;
    public bool m_gotMedal3 = false;
    public bool m_gotMedal4 = false;
    public bool m_gotMedal5 = false;
    public bool m_gotMedal6 = false;
    public bool m_gotMedal7 = false;
    public bool m_gotMedal8 = false;
    public bool m_gotMedal9 = false;
    public bool m_gotMedal10 = false;
    public bool m_gotMedal11 = false;
    public bool m_gotMedal12 = false;
    public bool m_gotMedal13 = false;
    public bool m_gotMedal14 = false;
    public bool m_gotMedal15 = false;
    public bool m_gotMedal16 = false;

    private void Awake()
    {
        if (Instance != null) throw new UnityException("There's already an instance of " + this.GetType().Name);
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_trainerId = Random.Range(0, 65535);

    }

    // Update is called once per frame
    void Update()
    {
        m_secondsPlayed += Time.deltaTime;
    }
}
