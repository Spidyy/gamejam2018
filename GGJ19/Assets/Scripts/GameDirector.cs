//
// Copyright (c) 2018 Tag Games Ltd. All rights reserved
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TagFramework;

// runs the core loop of the game
//
public class GameDirector : MonoSingleton<GameDirector>
{
    [HideInInspector]
    public Player m_player;

    public EncounterUIController m_encounterUI;
    public float m_eventInterval = 5f;

    private ScrollingObject[] m_scrollingObjects;

    private float m_nextEventDistance;

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
        LogUtils.Assert(m_player != null, "Player is null");

        m_scrollingObjects = FindObjectsOfType<ScrollingObject>();
    }

    private void Start ()
    {
        m_nextEventDistance = GenerateNextEventDistance();
        m_player.Move();
        SetScrollingActive(true);
    }

    private void Update ()
    {
        if(m_player.m_currentDistance < m_nextEventDistance)
        {
            m_player.Move();
            if(m_player.m_currentDistance >= m_nextEventDistance)
            {
                m_player.StopAt(m_nextEventDistance);
                SetScrollingActive(false);

                // trigger event
                m_encounterUI.ShowEncounter();
            }
        }
    }

    private void SetScrollingActive(bool active)
    {
        for(int i = 0; i < m_scrollingObjects.Length; ++i)
        {
            m_scrollingObjects[i].SetScrollingActive(active);
        }
    }

    public void OnOption1Chosen()
    {
        m_encounterUI.ShowOutcome();
    }

    public void OnOption2Chosen()
    {
        m_encounterUI.ShowOutcome();
    }

    public void OnOutcomeAccept()
    {
        m_encounterUI.Hide();
        m_nextEventDistance = GenerateNextEventDistance();
        m_player.Move();
        SetScrollingActive(true);
    }

    private float GenerateNextEventDistance()
    {
        return m_nextEventDistance + m_eventInterval;
    }

}
