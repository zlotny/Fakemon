using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDialogPanel : MonoBehaviour
{
    private static UIDialogPanel _instance;
    public static UIDialogPanel Instance { get { return _instance; } }

    private string m_currentMessage;
    private TextMeshProUGUI m_dialog_panel_text;


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

    public void ShowText(string text)
    {
        m_currentMessage = text;
        m_dialog_panel_text.SetText(m_currentMessage);
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_dialog_panel_text = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(m_dialog_panel_text);
        Hide();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
