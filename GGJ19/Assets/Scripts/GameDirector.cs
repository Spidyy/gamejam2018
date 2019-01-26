//
// Copyright (c) 2018 Tag Games Ltd. All rights reserved
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TagFramework;

// runs the core loop of the game
//
public class GameDirector : MonoBehaviour
{
    [HideInInspector]
    public Player m_player;
    private int m_nextEventDistance;

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
        LogUtils.Assert(m_player != null, "Player is null");
    }

    private void Start ()
    {
        m_nextEventDistance = GenerateNextEventDistance();
    }

    private void Update ()
    {
        if(m_player.m_currentDistance < m_nextEventDistance)
        {
            m_player.Move();
            if(m_player.m_currentDistance >= m_nextEventDistance)
            {
                m_player.StopAt(m_nextEventDistance);
                // trigger event
            }
        }
    }

    private int GenerateNextEventDistance()
    {
        return 20;
    }
}
