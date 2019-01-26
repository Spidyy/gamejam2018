using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

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

    public class Outcome
    {
        public string Text;
        public string CloseText;
        public int Chance;
        public Dictionary<Stat, int> StatChanges;

        public void Read(XmlNode iOutcomeNode)
        {
            StatChanges = new Dictionary<Stat, int>();

            foreach(XmlNode childNode in iOutcomeNode)
            {
                if(childNode.Name == "text")
                {
                    Text = childNode.Value;
                }
                else if(childNode.Name == "close")
                {
                    CloseText = childNode.Value;
                }
                else if(childNode.Name == "hp")
                {
                    StatChanges.Add(Stat.HP, int.Parse(childNode.Value));
                }
                else if (childNode.Name == "stamina")
                {
                    StatChanges.Add(Stat.STA, int.Parse(childNode.Value));
                }
                else if (childNode.Name == "hunger")
                {
                    StatChanges.Add(Stat.HUN, int.Parse(childNode.Value));
                }
                else if (childNode.Name == "gold")
                {
                    StatChanges.Add(Stat.GOLD, int.Parse(childNode.Value));
                }
                else
                {
                    Debug.LogError("Wrong node name : " + childNode.Name + " in outcome node.");
                }
            }
        }
    }

    public class Choice
    {
        public string Text;
        public Outcome[] Outcomes;
        public int totalOutcomeChances;

        public void Read(XmlNode iChoiceNode)
        {
            List<Outcome> tempOutcomes = new List<Outcome>();
            foreach (XmlNode childNode in iChoiceNode.ChildNodes)
            {
                if (childNode.Name == "text")
                {
                    Text = childNode.Value;
                }
                else if (childNode.Name == "outcome")
                {
                    Outcome newOutcome = new Outcome();
                    newOutcome.Read(childNode);
                    tempOutcomes.Add(newOutcome);
                }
                else
                {
                    Debug.LogError("Wrong node name : " + childNode.Name + " in choice node.");
                }
            }

            if(string.IsNullOrEmpty(Text))
            {
                Debug.LogAssertion("You are missing a text for an outcome.");
            }

            if(tempOutcomes.Count > 0)
            {
                Outcomes = tempOutcomes.ToArray();
            }

            totalOutcomeChances = 0;
            foreach (Outcome outcome in Outcomes)
            {
                totalOutcomeChances += outcome.Chance;
            }
        }
    }

    public string Id;
    public string Text;
    public int Tier;
    public Choice[] Choices;

    public void Read(XmlNode iEncounterNode)
    {
        List<Choice> tempChoices = new List<Choice>();
        foreach (XmlNode childNode in iEncounterNode.ChildNodes)
        {
            if(childNode.Name == "id")
            {
                Id = childNode.Value;
            }
            if (childNode.Name == "text")
            {
                Text = childNode.Value;
            }
            else if(childNode.Name == "tier")
            {
                Tier = int.Parse(childNode.Value);
            }
            else if(childNode.Name == "choice")
            {
                Choice newChoice = new Choice();
                newChoice.Read(childNode);
                tempChoices.Add(newChoice);
            }
            else
            {
                Debug.LogError("Wrong node name : " + childNode.Name + " in encounter node.");
            }
        }

        if(tempChoices.Count == 0)
        {
            Debug.LogAssertion("You need to have at least one choice for encounter " + Id);
        }

        if(string.IsNullOrEmpty(Text))
        {
            Debug.LogAssertion("You are missing a text for encounter " + Id);
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
