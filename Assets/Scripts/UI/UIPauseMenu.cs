using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class UIPauseMenu : MonoBehaviour
{
    public static UIPauseMenu Instance { get; private set; }

    private Action m_onFinishCallbacks;
    private int m_currentlySelectedOption = 0;
    private int m_numberOfChildMenuItems = -1;
    private bool m_upAxisDown = false;
    private bool m_downAxisDown = false;
    private PauseMenuItem[] m_pauseMenuItems;
    private TextMeshProUGUI m_infoText;

    private void Awake()
    {
        if (Instance != null) throw new UnityException("There's already an instance of " + this.GetType().Name);
        Instance = this;
        m_infoText = GetComponentInChildren<PauseInformationPanelLabel>().GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
        SubstituteKeyStrings();
        InitializeMenuItems();
        UpdateSelectorStatus();
    }

    private void InitializeMenuItems()
    {
        m_pauseMenuItems = GetComponentsInChildren<PauseMenuItem>();
        m_numberOfChildMenuItems = m_pauseMenuItems.Length;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        UpdateSelectorStatus();
    }

    public void SetOnFinishCallback(Action action)
    {
        m_onFinishCallbacks += action;
    }

    void SubstituteKeyStrings()
    {
        foreach (TextMeshProUGUI tmproText in GetComponentsInChildren<TextMeshProUGUI>())
        {
            tmproText.SetText(StringKeysReplacer.Replace(tmproText.text));
        }
    }

    // Update is called once per frame
    void Update()
    {
        ComputeControls();
    }

    private void UpdateSelectorStatus()
    {
        int i = 0;
        foreach (PauseMenuItem item in m_pauseMenuItems)
        {
            Image imageInItem = item.GetComponentInChildren<Image>(true);
            if (i == m_currentlySelectedOption)
            {
                imageInItem.enabled = true;
                m_infoText.SetText(item.GetMenuItemInfoText());
            }
            else
            {
                imageInItem.enabled = false;
            }
            i++;
        }
    }

    private void OpenPokedexMenu()
    {
        Debug.Log("Executing OpenPokedexMenu()");
    }

    private void OpenPokemonMenu()
    {
        Debug.Log("Executing OpenPokemonMenu()");
    }

    private void OpenBagMenu()
    {
        Debug.Log("Executing OpenBagMenu()");
    }

    private void OpenPokeGearMenu()
    {
        Debug.Log("Executing OpenPokeGearMenu()");
    }

    private void OpenPlayerMenu()
    {
        Debug.Log("Executing OpenPlayerMenu()");
    }

    private void OpenSaveMenu()
    {
        Debug.Log("Executing OpenSaveMenu()");
    }

    private void OpenOptionsMenu()
    {
        Debug.Log("Executing OpenOptionsMenu()");
    }

    private void ExitPauseMenu()
    {
        this.Hide();
        m_onFinishCallbacks();
    }

    private void ActivateItem()
    {
        switch (m_currentlySelectedOption)
        {
            case 0:
                OpenPokedexMenu();
                break;
            case 1:
                OpenPokemonMenu();
                break;
            case 2:
                OpenBagMenu();
                break;
            case 3:
                OpenPokeGearMenu();
                break;
            case 4:
                OpenPlayerMenu();
                break;
            case 5:
                OpenSaveMenu();
                break;
            case 6:
                OpenOptionsMenu();
                break;
            case 7:
                ExitPauseMenu();
                break;
            default:
                break;
        }
    }

    private bool GetUpPressed()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            // Debug.Log(m_axisDown);
            if (!m_upAxisDown)
            {
                m_upAxisDown = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            m_upAxisDown = false;
            return false;
        }
    }

    private bool GetDownPressed()
    {
        if (Input.GetAxis("Vertical") < 0)
        {
            if (!m_downAxisDown)
            {
                m_downAxisDown = true;
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            m_downAxisDown = false;
            return false;
        }
    }

    private void ComputeControls()
    {
        if (GetUpPressed())
        {
            if (m_currentlySelectedOption > 0) m_currentlySelectedOption--;
            UpdateSelectorStatus();
        }
        if (GetDownPressed())
        {
            if (m_currentlySelectedOption < m_numberOfChildMenuItems - 1) m_currentlySelectedOption++;
            UpdateSelectorStatus();
        }
        if (Input.GetButtonDown("A"))
        {
            ActivateItem();
        }
        if (Input.GetButtonDown("B") || Input.GetButtonDown("Start"))
        {
            ExitPauseMenu();
        }
    }
}
