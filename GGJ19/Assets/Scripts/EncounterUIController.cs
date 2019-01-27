
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterUIController : MonoBehaviour
{
    public GameObject m_choicePanel;
    public TMPro.TMP_Text m_choicePanelText;
    public Button m_choiceButton1;
    public TMPro.TMP_Text m_choiceButton1Text;
    public Button m_choiceButton2;
    public TMPro.TMP_Text m_choiceButton2Text;

    public GameObject m_outcomePanel;
    public TMPro.TMP_Text m_outcomeText;
    public TMPro.TMP_Text m_outcomeButtonText;

    public EncounterSystem encounterSystem;
    public Player player;

    public GameObject FloatingTextPrefab;
    public RectTransform FloatingTextSpawnTransform;

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
            m_choiceButton1.interactable = true;

            string text = current.Choices[0].Text;
            
            if(current.Choices[0].Outcomes != null && current.Choices[0].Outcomes.Length > 0)
            {
                int modifier = current.Choices[0].Outcomes[0].GoldModifier;
                if (modifier < 0)
                {
                    int cost = Mathf.Abs(modifier);
                    if (cost > player.Gold)
                    {
                        text += "(<color=\"red\"><sprite=0> " + cost.ToString() + "</color>)";
                        m_choiceButton1.interactable = false;
                    }
                    else
                    {
                        text += " <sprite=0>" + cost.ToString();
                    }
                }
            }

            m_choiceButton1Text.text = text;
        }
        else
        {
            Debug.LogError(current.Id + " is missing a first choices.");
        }

        if (current.Choices.Length >= 2)
        {
            m_choiceButton2.interactable = true;
            string text = current.Choices[1].Text;
            

            if (current.Choices[1].Outcomes != null && current.Choices[1].Outcomes.Length > 0)
            {
                int modifier = current.Choices[1].Outcomes[0].GoldModifier;
                if (modifier < 0)
                {
                    int cost = Mathf.Abs(modifier);
                    if (cost > player.Gold)
                    {
                        text += " <color=\"red\"><sprite=0>" + cost.ToString() + "</color>";
                        m_choiceButton2.interactable = false;
                    }
                    else
                    {
                        text += " <sprite=0>" + cost.ToString();
                    }
                }
            }

            m_choiceButton2Text.text = text;
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

    public void AddFloatingText(Stat stat, int modifier, float delay)
    {
        GameObject floatingTextObject = Instantiate(FloatingTextPrefab, FloatingTextSpawnTransform);
        floatingTextObject.transform.localPosition = Vector3.zero;

        FloatingText floatingText = floatingTextObject.GetComponent<FloatingText>();
        floatingText.Delay = delay;
        floatingText.Color = (modifier < 0) ? Color.red : Color.white;
        string sign = (modifier < 0)? "-" : "+";
        string value = sign + Mathf.Abs(modifier).ToString();
        switch (stat)
        {
            case Stat.HP:
                floatingText.Text = value + " Hp";
                break;
            case Stat.STA:
                floatingText.Text = value + " Stamina";
                break;
            case Stat.HUN:
                floatingText.Text = value + " Hunger";
                break;
            case Stat.GOLD:
                floatingText.Text = value + " Gold";
                break;
        }
    }

    public void Hide()
    {
        m_outcomePanel.SetActive(false);
        m_choicePanel.SetActive(false);
    }
}
