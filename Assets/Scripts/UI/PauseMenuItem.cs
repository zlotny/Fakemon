using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuItem : MonoBehaviour
{
    [SerializeField]
    private string m_menuItemInfoText = "";

    public string GetMenuItemInfoText()
    {
        return m_menuItemInfoText;
    }

}
