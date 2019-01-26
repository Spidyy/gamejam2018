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
    private Player m_player;

    public EncounterUIController m_encounterUI;
    public EncounterSystem encounterSystem;
    public float m_eventInterval = 5f;

    private ScrollingObject[] m_scrollingObjects;
    private HudUIController m_hudUIController = null;

    private float m_nextEventDistance;

    public Player Player {  get { return m_player; } }

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
        LogUtils.Assert(m_player != null, "Player is null");

        m_scrollingObjects = FindObjectsOfType<ScrollingObject>();
    }

    private void Start()
    {
        m_nextEventDistance = GenerateNextEventDistance();
        m_player.Move();
        SetScrollingActive(true);

        var hudControlelrs = FindObjectsOfType<HudUIController>();
        if(hudControlelrs.Length == 0)
        {
            m_hudUIController = HudUIControllerFactory.CreateHudUIController();
        }
        else if(hudControlelrs.Length > 0)
        {
            m_hudUIController = hudControlelrs[0];

            for(int i = 1; i < hudControlelrs.Length; ++i)
            {
                Destroy(hudControlelrs[i]);
            }
        }

        Debug.Assert(m_hudUIController != null, "m_hudUIController is null");
        m_hudUIController.Initialise();
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

    public void OnOptionChosen(int choice)
    {
        Encounter current = encounterSystem.CurrentEncounter;
        Encounter.Outcome outcome = current.Choices[choice].GetRandomOutcome();
        if (outcome != null)
        {
            m_encounterUI.ShowOutcome(outcome);
        }
        else
        {
            m_encounterUI.Hide();
        }
    }

    public void OnOutcomeAccept()
    {
        encounterSystem.SolveCurrentEvent();
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
