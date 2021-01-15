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
    private TextMeshProUGUI m_dialogPanelText;


    private void Awake()
    {
        if (_instance != null) throw new UnityException("There's already an instance of " + this.GetType().Name);
        _instance = this;
    }

    void Start()
    {
        m_dialogPanelText = GetComponentInChildren<TextMeshProUGUI>();
        if (!m_dialogPanelText) throw new UnityException(this.name + " does not have a child with an TextMeshoProUGUI component on it");
        Hide();
    }

    public void ShowText(string text)
    {
        m_currentMessage = text;
        m_dialogPanelText.SetText(m_currentMessage);
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }


}
