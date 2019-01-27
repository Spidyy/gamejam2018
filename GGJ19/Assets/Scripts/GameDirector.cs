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
    public enum State
    {
        Intro,
        FadeOut,
        PlayThrough
    }

    public float m_minEventInterval = 3f;
    public float m_maxEventInterval = 7f;

    private const float DELAY_OFFSET = 0.8f;
    private Player m_player;

    public EncounterUIController m_encounterUI;
    public EncounterSystem encounterSystem;
    public Animator introScreenAnimator;

    private ParallaxLayer[] m_scrollingObjects;
    private HudUIController m_hudUIController = null;
    private EndScreenUIController m_endScreenController = null;
    
    private float m_nextEventDistance;

    private Encounter.Outcome m_currentOutcome;

    public Player Player {  get { return m_player; } }

    public State m_state = State.Intro;

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
        LogUtils.Assert(m_player != null, "Player is null");

        m_scrollingObjects = FindObjectsOfType<ParallaxLayer>();
    }


    private void Start()
    {

        m_hudUIController = FindObjectOfTypeAndClearDuplicates<HudUIController>();
        m_hudUIController.Initialise();

        m_endScreenController = FindObjectOfTypeAndClearDuplicates<EndScreenUIController>();
        m_endScreenController.Initialise();

    }

    public void BeginPlaythrough()
    {
        m_state = State.PlayThrough;
        m_nextEventDistance = GenerateNextEventDistance();
        m_player.Move();
        SetScrollingActive(true);
    }

    private void Update()
    {
        if(m_state == State.Intro)
        {
            if (Input.anyKeyDown)
            {
                introScreenAnimator.SetTrigger("FadeOut");
                m_state = State.FadeOut;
            }
        }
        else if(m_state == State.PlayThrough)
        {
            if (m_player.m_currentDistance < m_nextEventDistance)
            {
                m_player.Move();
                if (m_player.m_currentDistance >= m_nextEventDistance)
                {
                    m_player.StopAt(m_nextEventDistance);
                    SetScrollingActive(false);

                    // trigger event
                    m_encounterUI.ShowEncounter();
                }
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
            m_currentOutcome = outcome;
            m_encounterUI.ShowOutcome(outcome);
        }
        else
        {
            m_encounterUI.Hide();
        }
    }

    public void OnOutcomeAccept()
    {
        ApplyOutcome(m_currentOutcome);
        encounterSystem.SolveCurrentEvent();
        m_currentOutcome = null;
        m_encounterUI.Hide();
        m_nextEventDistance = GenerateNextEventDistance();
        m_player.Move();
        SetScrollingActive(true);
    }

    private void ApplyOutcome(Encounter.Outcome outcome)
    {
        float delay = 0f;
        if(outcome.HpModifier != 0)
        {
            m_player.AlterStat(Stat.HP, outcome.HpModifier);
            m_encounterUI.AddFloatingText(Stat.HP, outcome.HpModifier, delay);
            delay += DELAY_OFFSET;
        }

        if (outcome.StaminaModifier != 0)
        {
            m_player.AlterStat(Stat.STA, outcome.StaminaModifier);
            m_encounterUI.AddFloatingText(Stat.STA, outcome.StaminaModifier, delay);
            delay += DELAY_OFFSET;
        }

        if (outcome.HungerModifier != 0)
        {
            m_player.AlterStat(Stat.HUN, outcome.HungerModifier);
            m_encounterUI.AddFloatingText(Stat.HUN, outcome.HungerModifier, delay);
            delay += DELAY_OFFSET;
        }

        if (outcome.GoldModifier != 0)
        {
            m_player.AlterStat(Stat.GOLD, outcome.GoldModifier);
            m_encounterUI.AddFloatingText(Stat.GOLD, outcome.GoldModifier, delay);
        }
    }

    private float GenerateNextEventDistance()
    {
        return m_nextEventDistance + Random.Range(m_minEventInterval, m_maxEventInterval);
    }

    //
    //
    private T FindObjectOfTypeAndClearDuplicates<T>() where T : MonoBehaviour
    {
        T instance = null;
        var foundInstances = FindObjectsOfType<T>();

        if(foundInstances.Length > 0)
        {
            instance = foundInstances[0];

            for(int i = 1; i < foundInstances.Length; ++i)
            {
                Destroy(foundInstances[i]);
            }
        }

        Debug.Assert(instance != null, "Unable to find references of " + typeof(T).ToString());

        return instance;
    }

}
