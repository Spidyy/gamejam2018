using System.Collections;
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
        public KeyValuePair<Stat, int>[] StatChanges;

        public void Read(XmlNode iOutcomeNode, string id)
        {
            List<KeyValuePair<Stat, int>>  tempStatChanges = new List<KeyValuePair<Stat, int>>();

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
                    tempStatChanges.Add(new KeyValuePair<Stat, int>(Stat.HP, int.Parse(childNode.InnerText)));
                }
                else if (childNode.Name == "stamina")
                {
                    tempStatChanges.Add(new KeyValuePair<Stat, int>(Stat.STA, int.Parse(childNode.InnerText)));
                }
                else if (childNode.Name == "hunger")
                {
                    tempStatChanges.Add(new KeyValuePair<Stat, int>(Stat.HUN, int.Parse(childNode.InnerText)));
                }
                else if (childNode.Name == "gold")
                {
                    tempStatChanges.Add(new KeyValuePair<Stat, int>(Stat.GOLD, int.Parse(childNode.InnerText)));
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

            if(tempStatChanges.Count > 0)
            {
                StatChanges = tempStatChanges.ToArray();
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
            if (childNode.Name == "text")
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

                    if (outcome.StatChanges != null)
                    {
                        foreach(KeyValuePair<Stat, int> statChange in outcome.StatChanges)
                        {
                            iWriter.WriteElementString(sStatToString[statChange.Key], statChange.Value.ToString());
                        }
                    }
                    iWriter.WriteEndElement();
                }
            }

            iWriter.WriteEndElement();
        }

        iWriter.WriteEndElement();
    }
}
