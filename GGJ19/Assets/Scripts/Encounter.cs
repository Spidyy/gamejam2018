﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

[System.Serializable]
public class Encounter
{
    public static readonly Dictionary<Stat, string> sStatToString = new Dictionary<Stat, string>
    {
        {Stat.HP, "hp" },
        {Stat.STA, "stamina"},
        {Stat.HUN, "hunger"},
        {Stat.GOLD, "gold"}
    };
    public static readonly Dictionary<string, Stat> sStringToStat = new Dictionary<string, Stat>
    {
        {"hp",  Stat.HP},
        {"stamina", Stat.STA},
        {"hunger", Stat.HUN},
        {"gold", Stat.GOLD}
    };

    [System.Serializable]
    public class Outcome
    {
        public string Text;
        public string CloseText;
        public int Chance;
        public int HpModifier;
        public int StaminaModifier;
        public int HungerModifier;
        public int GoldModifier;

        public void Read(XmlNode iOutcomeNode, string id)
        {
            HpModifier = 0;
            StaminaModifier = 0;
            HungerModifier = 0;
            GoldModifier = 0;

            foreach(XmlNode childNode in iOutcomeNode)
            {
                if(childNode.Name == "text")
                {
                    Text = childNode.InnerText;
                }
                else if(childNode.Name == "close")
                {
                    CloseText = childNode.InnerText;
                }
                else if(childNode.Name == "hp")
                {
                    HpModifier += int.Parse(childNode.InnerText);
                }
                else if (childNode.Name == "stamina")
                {
                    StaminaModifier += int.Parse(childNode.InnerText);
                }
                else if (childNode.Name == "hunger")
                {
                    HungerModifier += int.Parse(childNode.InnerText);
                }
                else if (childNode.Name == "gold")
                {
                    GoldModifier += int.Parse(childNode.InnerText);
                }
                else if (childNode.Name != "#comment")
                {
                    Debug.LogWarning("Wrong node name : " + childNode.Name + " in outcome node of event " + id);
                }
            }

            foreach(XmlAttribute attribute in iOutcomeNode.Attributes)
            {
                if(attribute.Name == "chance")
                {
                    Chance = int.Parse(attribute.InnerText);
                }
            }
        }
    }

    [System.Serializable]
    public class Choice
    {
        public string Text;
        public Outcome[] Outcomes;
        public int TotalOutcomeChances
        {
            get;
            private set;
        }

        public void Read(XmlNode iChoiceNode, string id)
        {
            List<Outcome> tempOutcomes = new List<Outcome>();
            foreach (XmlNode childNode in iChoiceNode.ChildNodes)
            {
                if (childNode.Name == "text")
                {
                    Text = childNode.InnerText;
                }
                else if (childNode.Name == "outcome")
                {
                    Outcome newOutcome = new Outcome();
                    newOutcome.Read(childNode, id);
                    tempOutcomes.Add(newOutcome);
                }
                else if (childNode.Name != "#comment")
                {
                    Debug.LogWarning("Wrong node name : " + childNode.Name + " in choice node of event " + id);
                }
            }

            if(string.IsNullOrEmpty(Text))
            {
                Debug.LogAssertion("You are missing a text for a choice of event " + id);
            }

            if(tempOutcomes.Count > 0)
            {
                Outcomes = tempOutcomes.ToArray();
            }

            TotalOutcomeChances = 0;
            foreach (Outcome outcome in Outcomes)
            {
                TotalOutcomeChances += outcome.Chance;
            }
        }

        public Outcome GetRandomOutcome()
        {
            if(Outcomes.Length == 1)
            {
                return Outcomes[0];
            }
            else if (Outcomes.Length > 1)
            {
                float[] normalizedOutcomeChances = new float[Outcomes.Length];
                normalizedOutcomeChances[0] = (Outcomes[0].Chance / TotalOutcomeChances);
                for (int index = 1; index < Outcomes.Length; ++index)
                {
                    normalizedOutcomeChances[index] = (Outcomes[index].Chance / TotalOutcomeChances) + normalizedOutcomeChances[index - 1];
                }

                float random = Random.value;
                float previousOutcomeChance = 0f;
                for (int index = 0; index < Outcomes.Length; ++index)
                {
                    if (previousOutcomeChance < random && random < normalizedOutcomeChances[index])
                    {
                        Debug.Log(string.Format("Multiple outcomes, selected outcome {0}({1})", index, random));
                        return Outcomes[index];
                    }
                    previousOutcomeChance = normalizedOutcomeChances[index];
                }

                // Couldn't get an outcome from the random value, default to the first one.
                return Outcomes[0];
            }
            else
            {
                Debug.Log("No outcome, nothing to do.");
                return null;
            }
        }
    }

    public string Id;
    public string Text;
    public int Tier;
    public Choice[] Choices;

    public void Read(XmlNode iEncounterNode)
    {
        Id = "Unknown";
        Text = null;
        Tier = 1;

        List<Choice> tempChoices = new List<Choice>();
        foreach (XmlNode childNode in iEncounterNode.ChildNodes)
        {
            if(childNode.Name == "id")
            {
                Id = childNode.InnerText;
            }
            else if (childNode.Name == "text")
            {
                Text = childNode.InnerText;
            }
            else if(childNode.Name == "tier")
            {
                Tier = int.Parse(childNode.InnerText);
            }
            else if(childNode.Name == "choice")
            {
                Choice newChoice = new Choice();
                newChoice.Read(childNode, Id);
                tempChoices.Add(newChoice);
            }
            else if(childNode.Name != "#comment")
            {
                Debug.LogWarning("Wrong node name : " + childNode.Name + " in event node of event " + Id);
            }
        }

        if(tempChoices.Count == 0)
        {
            Debug.LogAssertion("You need to have at least one choice for event " + Id);
        }

        if(string.IsNullOrEmpty(Text))
        {
            Debug.LogAssertion("You are missing a text for event " + Id);
        }

        Choices = tempChoices.ToArray();
    }

    public void Write(XmlWriter iWriter)
    {
        iWriter.WriteStartElement("encounter");
        iWriter.WriteElementString("text", Text);
        iWriter.WriteElementString("tier", Tier.ToString());

        foreach(Choice choice in Choices)
        {
            iWriter.WriteStartElement("choice");
            iWriter.WriteElementString("text", choice.Text);

            if (choice.Outcomes != null)
            {
                foreach (Outcome outcome in choice.Outcomes)
                {
                    iWriter.WriteStartElement("outcome");
                    iWriter.WriteElementString("text", outcome.Text);
                    if (outcome.HpModifier != 0)
                    {
                        iWriter.WriteElementString("hp", outcome.HpModifier.ToString());
                    }
                    if (outcome.StaminaModifier != 0)
                    {
                        iWriter.WriteElementString("stamina", outcome.StaminaModifier.ToString());
                    }
                    if (outcome.HungerModifier != 0)
                    {
                        iWriter.WriteElementString("hunger", outcome.HungerModifier.ToString());
                    }
                    if (outcome.GoldModifier != 0)
                    {
                        iWriter.WriteElementString("gold", outcome.GoldModifier.ToString());
                    }
                    iWriter.WriteEndElement();
                }
            }

            iWriter.WriteEndElement();
        }

        iWriter.WriteEndElement();
    }
}
