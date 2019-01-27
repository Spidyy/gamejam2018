using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class EncounterSystem : MonoBehaviour
{
    public const string k_folderPath = "Events";

    public float[] TierChances = new float[] { 50f, 30f, 20f };

    public int TotalTierChances
    {
        get {
            int totalChances = 0;
            foreach(int chance in TierChances)
            {
                totalChances += chance;
            }
            return totalChances;
        }
    }

    public int MaxTier
    {
        get
        {
            return TierChances.Length - 1;
        }
    }

    [SerializeField]
    private Encounter[] mEncounters = null;

    [SerializeField]
    private List<int>[] mFreeIndicePerTier = new List<int>[3];

    [SerializeField]
    private int mHighestTier = 0;

    public int HighestTier
    {
        get { return mHighestTier; }
        set { mHighestTier = Mathf.Clamp(value, 0, MaxTier); }
    }

    public Encounter CurrentEncounter
    {
        get;
        private set;
    }

    public void DebugDrawTier()
    {
        int tier = RandomlySelectTier(MaxTier);

        Debug.Log("Drawn tier : " + tier.ToString());
    }

    public void SolveCurrentEvent()
    {
        CurrentEncounter = GetNextEncounter(mHighestTier);
        Debug.Log(string.Format("New encounter : {0} (tier {1})", CurrentEncounter.Id, CurrentEncounter.Tier));
    }

    void Start()
    {
        mEncounters = LoadData(k_folderPath);

        if (mEncounters == null)
        {
            Debug.LogAssertion("No events could be loaded !");
        }

        RefreshAllTiersIndice();

        Debug.Log(mEncounters.Length.ToString() + " encounter(s) loaded.");

        CurrentEncounter = GetNextEncounter(0);
    }

    private Encounter[] LoadData(string path)
    {
        TextAsset[] xmlFiles = Resources.LoadAll<TextAsset>(path);

        if (xmlFiles.Length > 0)
        {
            List<Encounter> tempEncounter = new List<Encounter>();
            foreach (TextAsset xmlFile in xmlFiles)
            {
                XmlDocument xmlDocument = new XmlDocument();
                //Debug.Log("Loading file " + xmlFile.name);
                xmlDocument.LoadXml(xmlFile.text);

                if (xmlDocument.DocumentElement.Name == "events")
                {
                    foreach (XmlNode childNode in xmlDocument.DocumentElement)
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
                else if (xmlDocument.DocumentElement.Name == "event")
                {
                    Encounter newEncounter = new Encounter();
                    newEncounter.Read(xmlDocument.DocumentElement);
                    tempEncounter.Add(newEncounter);
                }
                else if (xmlDocument.DocumentElement.Name != "#comment")
                {
                    Debug.LogError("Wrong node name : " + xmlDocument.DocumentElement.Name + " in xml document.");
                }
            }

            if (tempEncounter.Count > 0)
            {
                return tempEncounter.ToArray();
            }
        }

        return null;
    }

    private int RandomlySelectTier(int maxTier)
    {
        // Total of tiers
        float totalChances = 0f;
        for(int tier = 0; tier <= maxTier; ++tier)
        {
            totalChances += TierChances[tier];
        }

        float[] normalizedChances = new float[maxTier+1];
        normalizedChances[0] = TierChances[0] / totalChances;

        
        for (int tier = 1; tier <= maxTier; ++tier)
        {
            normalizedChances[tier] = (TierChances[tier] / totalChances) + normalizedChances[tier - 1];
        }

        float random = Random.value;
        float previousTierChance = 0f;
        for (int tier = 0; tier <= maxTier; ++tier)
        {
            if (previousTierChance < random && random < normalizedChances[tier])
            {
                return tier;
            }
            previousTierChance = normalizedChances[tier];
        }

        // If we don't find a tier, we just throw a low one.
        return 0;
    }

    private Encounter GetNextEncounter(int maxTier)
    {
        // Select a tier
        int selectedTier = RandomlySelectTier(maxTier);
        List<int> selectedTierList = mFreeIndicePerTier[selectedTier];

        Debug.Log("selected tier : " + selectedTier);

        // If we used up all our encounter for this tier, refresh it.
        if(selectedTierList.Count == 0)
        {
            Debug.Log("Refreshing list of tier : " + selectedTier);
            selectedTierList = RefreshTierIndice(selectedTier);
        }

        // Select event from tier
        int encounterIndicesIndex = Random.Range(0, selectedTierList.Count);

        Debug.Log("Selected tier list size : " + selectedTierList.Count.ToString() + ", encounter Indices Index : " + encounterIndicesIndex.ToString());
        int encounterIndex = selectedTierList[encounterIndicesIndex];
        selectedTierList.RemoveAt(encounterIndicesIndex);

        return mEncounters[encounterIndex];
    }

    private List<int> RefreshTierIndice(int iTier)
    {
        // Create or clear tier indices
        if (mFreeIndicePerTier[iTier] == null)
        {
            mFreeIndicePerTier[iTier] = new List<int>();
        }
        else
        {
            mFreeIndicePerTier[iTier].Clear();
        }

        for (int index = 0; index < mEncounters.Length; ++index)
        {
            Encounter encounter = mEncounters[index];
            if (encounter.Tier == (iTier+1))
            {
                mFreeIndicePerTier[iTier].Add(index);
            }
        }

        return mFreeIndicePerTier[iTier];
    }

    private void RefreshAllTiersIndice()
    {
        // Create or clear all arrays
        for (int index = 0; index < mFreeIndicePerTier.Length; ++index)
        {
            if (mFreeIndicePerTier[index] == null)
            {
                mFreeIndicePerTier[index] = new List<int>();
            }
            else
            {
                mFreeIndicePerTier[index].Clear();
            }
        }

        for (int index = 0; index < mEncounters.Length; ++index)
        {
            Encounter encounter = mEncounters[index];
            int encounterTier = Mathf.Clamp(encounter.Tier - 1, 0, MaxTier);

            mFreeIndicePerTier[encounterTier].Add(index);
        }
    }
}
