using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public float m_speedMetresPerSecond = 1f;

    public bool m_moving = false;
    public float m_currentDistance = 0f;

    private int[] m_currentStats;
    private Animator m_animator;

    public void Awake()
    {
        m_currentStats = GenerateStartingStats();
        m_animator = GetComponent<Animator>();
    }

    public int[] GenerateStartingStats()
    {
        int num = Enum.GetNames(typeof(Stat)).Length;
        var stats = new int[num];

        for(int i = 0; i < num; ++i)
        {
            stats[i] = StatConsts.k_startingValues[i];
        }

        return stats;
    }

    public void Move()
    {
        m_currentDistance += Time.deltaTime * m_speedMetresPerSecond;
        m_moving = true;
    }

    public void StopAt(float distance)
    {
        m_currentDistance = distance;
        m_moving = false;
    }

    public void AlterStat(Stat stat, int delta)
    {
        m_currentStats[(int)stat] = Mathf.Clamp(m_currentStats[(int)stat] + delta, 0, StatConsts.k_maxValues[(int)stat]);
    }

    public bool IsDead()
    {
        return m_currentStats[(int)Stat.HP] <= 0 || m_currentStats[(int)Stat.STA] <= 0 || m_currentStats[(int)Stat.HUN] <= 0 ;
    }

    public void Update()
    {
        m_animator.SetFloat("Speed", m_moving ? 1f : 0f);
    }
}
