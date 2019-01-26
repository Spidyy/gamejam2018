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
    public TMPro.TMP_Text m_choiceButton1Cost;
    public TMPro.TMP_Text m_choiceButton2Text;
    public TMPro.TMP_Text m_choiceButton2Cost;

    public GameObject m_outcomePanel;
    public TMPro.TMP_Text m_outcomeText;
    public TMPro.TMP_Text m_outcomeButtonText;

    public EncounterSystem encounterSystem;
    public Player player;

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

        Encounter current = encounterSystem.CurrentEncounter;

        // populate text
        m_choicePanelText.text = current.Text;
        if(current.Choices.Length >= 1)
        {
            m_choiceButton1Text.text = current.Choices[0].Text;

            if(current.Choices[0].Outcomes != null && current.Choices[0].Outcomes.Length > 0)
            {
                int modifier = current.Choices[0].Outcomes[0].GoldModifier;
                if (modifier < 0)
                {
                    m_choiceButton1Cost.gameObject.SetActive(true);
                    int cost = Mathf.Abs(modifier);
                    if (cost > player.Gold)
                    {
                        m_choiceButton1Cost.text = "This action require " + Mathf.Abs(current.Choices[0].Outcomes[0].GoldModifier).ToString() + "golds.";
                        m_choiceButton1Cost.color = Color.red;
                    }
                    else
                    {
                        m_choiceButton1Cost.text = "You will pay " + Mathf.Abs(current.Choices[0].Outcomes[0].GoldModifier).ToString() + "golds.";
                        m_choiceButton1Cost.color = Color.white;
                    }
                }
                else
                {
                    m_choiceButton1Cost.gameObject.SetActive(false);
                }
            }
            else
            {
                m_choiceButton1Cost.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError(current.Id + " is missing a first choices.");
        }

        if (current.Choices.Length >= 2)
        {
            m_choiceButton2Text.text = current.Choices[1].Text;

            if (current.Choices[1].Outcomes != null && current.Choices[1].Outcomes.Length > 0)
            {
                int modifier = current.Choices[1].Outcomes[0].GoldModifier;
                if (modifier < 0)
                {
                    m_choiceButton2Cost.gameObject.SetActive(true);

                    int cost = Mathf.Abs(modifier);
                    if (cost > player.Gold)
                    {
                        m_choiceButton2Cost.text = "This action require " + cost.ToString() + "golds.";
                        m_choiceButton2Cost.color = Color.red;
                    }
                    else
                    {
                        m_choiceButton2Cost.text = "You will pay " + cost.ToString() + "golds.";
                        m_choiceButton2Cost.color = Color.white;
                    }
                }
                else
                {
                    m_choiceButton2Cost.gameObject.SetActive(false);
                }
            }
            else
            {
                m_choiceButton2Cost.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError(current.Id + " is missing a second choices.");
        }
    }

    public void ShowOutcome(Encounter.Outcome outcome)
    {
        m_outcomePanel.SetActive(true);
        m_choicePanel.SetActive(false);

        // populate text
        m_outcomeText.text = outcome.Text;
        if (string.IsNullOrEmpty(outcome.CloseText))
        {
            m_outcomeButtonText.text = "Continue.";
        }
        else
        {
            m_outcomeButtonText.text = outcome.CloseText;
        }
    }

    public void Hide()
    {
        m_outcomePanel.SetActive(false);
        m_choicePanel.SetActive(false);
    }
}
