using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public const string k_filePath = "Events";

    [System.Serializable]
    public struct EventData
    {
        public Event[] Events;
    }

    [System.Serializable]
    public struct Event
    {
        public string Title;
        public string Description;
        public int Tier;
        public EventChoice ChoiceA;
        public EventChoice ChoiceB;
    }

    [System.Serializable]
    public struct EventChoice
    {
        public string ButtonText;
        public string OutcomeText;
        public string[] OutcomeStats;
        public int[] StatsDelta;
        public bool OutcomesShown;
    }

    private EventData m_data;

    public EventSystem()
    {
        m_data = LoadData(k_filePath);

        /*
        for(int i = 0; i < m_data.Events.Length; ++i)
        {
            Debug.Log(m_data.Events[i].Title);
        }
        */
    }

    public EventData LoadData(string path)
    {
        var textAsset = Resources.Load<TextAsset>(path);
        return JsonUtility.FromJson<EventData>(textAsset.text);
    }
}
