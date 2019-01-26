using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class EncounterSystem : MonoBehaviour
{
    public const string k_folderPath = "Events";

    [SerializeField]
    private Encounter[] mEncounters = null;

    void Start()
    {
        mEncounters = LoadData(k_folderPath);

        if (mEncounters == null)
        {
            Debug.LogAssertion("No events could be loaded !");
        }

        Debug.Log(mEncounters.Length.ToString() + " encounter(s) loaded.");
    }

    public Encounter[] LoadData(string path)
    {
        TextAsset[] xmlFiles = Resources.LoadAll<TextAsset>(path);

        if (xmlFiles.Length > 0)
        {
            List<Encounter> tempEncounter = new List<Encounter>();
            foreach (TextAsset xmlFile in xmlFiles)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlFile.text);

                foreach(XmlNode childNode in xmlDocument.DocumentElement)
                {
                    if (childNode.Name == "event")
                    {
                        Encounter newEncounter = new Encounter();
                        newEncounter.Read(childNode);
                        tempEncounter.Add(newEncounter);
                    }
                    else if (childNode.Name != "#comment")
                    {
                        Debug.LogError("Wrong node name : " + childNode.Name + " in xml document.");
                    }
                }
            }

            if (tempEncounter.Count > 0)
            {
                return tempEncounter.ToArray();
            }
        }

        return null;
    }
}
