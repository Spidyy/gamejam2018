//
// Copyright (c) 2018 Tag Games Ltd. All rights reserved
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterUIController : MonoBehaviour
{
    public GameObject m_choicePanel;
    public TMPro.TMP_Text m_choicePanelText;
    public TMPro.TMP_Text m_choiceButton1Text;
    public TMPro.TMP_Text m_choiceButton2Text;

    public GameObject m_outcomePanel;
    public TMPro.TMP_Text m_outcomeText;

    private void Start ()
    {
        
    }

    private void Update ()
    {
        
    }

    public void ShowEncounter()
    {
        m_outcomePanel.SetActive(false);
        m_choicePanel.SetActive(true);

        // populate text
    }

    public void ShowOutcome()
    {
        m_outcomePanel.SetActive(true);
        m_choicePanel.SetActive(false);

        // populate text
    }

    public void Hide()
    {
        m_outcomePanel.SetActive(false);
        m_choicePanel.SetActive(false);
    }
}
