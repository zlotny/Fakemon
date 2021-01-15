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
    private ContinueButton m_continueButton;


    private void Awake()
    {
        if (_instance != null) throw new UnityException("There's already an instance of " + this.GetType().Name);
        _instance = this;
    }

    void Start()
    {
        m_dialogPanelText = GetComponentInChildren<TextMeshProUGUI>();
        if (!m_dialogPanelText) throw new UnityException(this.name + " does not have a child with an TextMeshoProUGUI component on it");
        m_continueButton = GetComponentInChildren<ContinueButton>(true);
        if (!m_continueButton) throw new UnityException(this.name + " does not have a child with an ContinueButton component on it");
        m_continueButton.Hide();
        Hide();
    }

    public void ShowText(string text)
    {
        m_currentMessage = text;
        m_dialogPanelText.SetText(m_currentMessage);
        this.gameObject.SetActive(true);
        CharacterControls.Instance.SetControlsEnabled(false);
    }

    public void EndDialog()
    {
        CharacterControls.Instance.SetControlsEnabled(true);
        Hide();
        this.m_dialogPanelText.pageToDisplay = 1;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        ComputeControls();
        ManageContinueButton();
    }

    private void ManageContinueButton()
    {
        if (m_dialogPanelText.textInfo.pageCount < 2)
        {
            m_continueButton.Hide();
            return;
        }

        if (m_dialogPanelText.pageToDisplay == m_dialogPanelText.textInfo.pageCount)
        {
            m_continueButton.Hide();
            return;
        }

        m_continueButton.Show();
    }

    private void ComputeControls()
    {
        if (Input.GetButtonDown("A"))
        {
            if (m_dialogPanelText.pageToDisplay == m_dialogPanelText.textInfo.pageCount)
            {
                EndDialog();
                return;
            }
            m_dialogPanelText.pageToDisplay++;
        }
    }
}
